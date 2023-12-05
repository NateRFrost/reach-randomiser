﻿using Bungie;
using Bungie.Tags;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using static ReachTesting.FilePathsForReach;
namespace ReachTesting
{
    

    internal partial class Program
    {
        static void Main(string[] args)
        {
            Random rand = new Random();
            ManagedBlamCrashCallback del = info => {

            };

            List<EnemyObjectPaths> runtimeEnemyObjects = FilePathsForReach.enemyObjectPaths.ToList();
            List<EnemyObjectPaths> runtimeGhostEnemyObjects = FilePathsForReach.ghostenemyObjectPaths.ToList();
            List<EnemyObjectPaths> runtimeWraithEnemyObjects = FilePathsForReach.wraithenemyObjectPaths.ToList();
            List<WeaponDetails> runtimeWeapons = FilePathsForReach.weapons.ToList();

            List<VehicleObjectPaths> runtimeVehicleObjectPaths = FilePathsForReach.vehicleObjectPaths.ToList();
            var param = new ManagedBlamStartupParameters();
            Bungie.ManagedBlamSystem.Start(@"C:\Program Files (x86)\Steam\steamapps\common\HREK", del, param);

            foreach (var levelnameStatic in FilePathsForReach.LevelNames)
            {
                Console.WriteLine();
                Console.WriteLine("Randomising " + levelnameStatic);

                string levelname = levelnameStatic;

                string levelpath = @"levels\solo\" + levelname + @"\" + levelname;
                string levelpath2 = @"levels\solo\" + levelname + @"\resources\" + levelname;



                // e.g. G:\SteamLibrary\steamapps\common\HREK

                var test_path = Bungie.Tags.TagPath.FromPathAndType(levelpath, "scnr*");
                using (var tagFile = new Bungie.Tags.TagFile(test_path))
                {




                    var vehiclefile = Bungie.Tags.TagPath.FromPathAndType(levelpath2, "*ehi");
                    using (var secondaryPalette = new Bungie.Tags.TagFile(vehiclefile))
                    {
                        var newVehiclePalette = secondaryPalette.Fields.Where(x => x.DisplayName.Contains("vehicle palette")).FirstOrDefault();
                        if (newVehiclePalette != null)
                        {
                            AddVehiclesToPalette(newVehiclePalette, runtimeVehicleObjectPaths);
                        }

                        secondaryPalette.Save();

                    }

                    var weaponfile = Bungie.Tags.TagPath.FromPathAndType(levelpath2, "*eap");
                    using (var secondaryPalette = new Bungie.Tags.TagFile(weaponfile))
                    {
                        var newWeaponPalette = secondaryPalette.Fields.Where(x => x.DisplayName.Contains("weapon palette")).FirstOrDefault();
                        if (newWeaponPalette != null)
                        {
                            AddWeaponsToPalette(newWeaponPalette, runtimeWeapons);
                        }

                        secondaryPalette.Save();

                    }



                    //Get the second weapon palette



                    //Get Pallettes

                    TagFieldBlock characterPalette = GetCharacterPalette(tagFile);
                    if (characterPalette == null)
                    {
                        Console.WriteLine("Character Palette is null");
                        return;
                    }
                    TagFieldBlock weaponPalette = GetWeaponPalette(tagFile);
                    if (weaponPalette == null)
                    {
                        Console.WriteLine("Weapon Palette is null");
                        return;
                    }
                    TagFieldBlock vehiclePalette = GetVehiclePalette(tagFile);
                    if (vehiclePalette == null)
                    {
                        Console.WriteLine("Vehicle Palette is null");
                        return;
                    }
                    //Add the vehicles to the palette
                    //Add the characters to the palette
                    AddCharactersToPalette(characterPalette, runtimeEnemyObjects);
                    //Add the weapons to the palette

                    AddWeaponsToPalette(weaponPalette, runtimeWeapons);

                    AddVehiclesToPalette(vehiclePalette, runtimeVehicleObjectPaths);


                    tagFile.Save();
                }

                using (var tagFile = new Bungie.Tags.TagFile(test_path))
                {
                    //Get Designer zones
                    TagField designerZones = GetDesignerZones(tagFile);
                    if (designerZones == null)
                    {
                        Console.WriteLine("Designer Zones is null");
                        return;
                    }
                    //Loop through the elements of the designer zones
                    foreach (var designerZone in ((Bungie.Tags.TagFieldBlock)designerZones).Elements)
                    {
                        //Get fields Block:weapon, Block:character, Block:vehicle
                        var weapon = designerZone.Fields.Where(x => x.FieldPathWithoutIndexes.Contains("Block:weapon")).FirstOrDefault();
                        var character = designerZone.Fields.Where(x => x.FieldPathWithoutIndexes.Contains("Block:character")).FirstOrDefault();
                        var vehicle = designerZone.Fields.Where(x => x.FieldPathWithoutIndexes.Contains("Block:vehicle")).FirstOrDefault();
                        ((Bungie.Tags.TagFieldBlock)weapon).RemoveAllElements();
                        ((Bungie.Tags.TagFieldBlock)character).RemoveAllElements();
                        ((Bungie.Tags.TagFieldBlock)vehicle).RemoveAllElements();



                    }
                    TagField squads = GetSquads(tagFile);
                    if (squads == null)
                    {
                        Console.WriteLine("Squads is null");
                        return;
                    }

                    //Loop through the elements of the squads
                    foreach (var squad in ((Bungie.Tags.TagFieldBlock)squads).Elements)
                    {
                        //Don't change the squad if it contains any of the keywords due to story progression
                        if (skipKeyWords.Any(x => squad.ElementHeaderText.ToLower().Contains(x.ToLower())))
                        {
                            continue;
                        }
                        string template = HasTemplate(squad);
                        int enemyCount = 0;
                        bool hasVehicle = false;
                        if (!string.IsNullOrEmpty(template))
                        {
                            //Check if the template name is in the skip list
                            if (skipKeyWords.Any(x => template.ToLower().Contains(x.ToLower())))
                            {
                                continue;
                            }


                            hasVehicle = TemplateHasVehicle(template);

                            enemyCount = GetEnemyCountFromTemplate(template);
                            //Remove the template
                            RemoveTemplate(squad);

                        }


                        //Get the designer field
                        var designer = squad.Fields.Where(x => x.DisplayName == "designer").FirstOrDefault();
                        //Get the cells
                        if (designer != null)
                        {
                            var cells = ((Bungie.Tags.TagFieldStruct)designer).Elements[0].Fields[0];
                            //If count is 0, add a cell
                            if (((Bungie.Tags.TagFieldBlock)cells).Elements.Count == 0)
                            {
                                ((Bungie.Tags.TagFieldBlock)cells).AddElement();
                            }
                            //Loop through the cells elements
                            foreach (var cell in ((Bungie.Tags.TagFieldBlock)cells).Elements)
                            {
                                //0.1 chance to have a vehicle
                                bool shouldHaveVehicle = rand.Next(0, 15) == 0;
                                bool shouldSpawnHunter = rand.Next(0, 15) == 0;
                                bool shouldSpawnEngineer = rand.Next(0, 15) == 0;
                                bool shouldSpawnMule = rand.Next(0, 15) == 0;



                                var enemies = cell.Fields.Where(x => x.DisplayName == "character type").FirstOrDefault();
                                if (enemies != null)
                                {

                                    if ((shouldHaveVehicle && !hasVehicle) || hasVehicle)
                                    {
                                        RemoveAllCharactersOfCell(cell);
                                        RemoveAllWeaponsOfCell(cell);

                                        //Set the weapon to the plasma pistol
                                        SetWeaponOfCell(cell, rand, runtimeWeapons.Where(o => o.Name == "plasma_pistol").FirstOrDefault().PaletteIndex);
                                        //Set the enemy count to 1
                                        SetNormalDiffCountOfCell(cell, 1);

                                        //Check if there are elements, if not add one
                                        if (((Bungie.Tags.TagFieldBlock)enemies).Elements.Count == 0)
                                        {
                                            ((Bungie.Tags.TagFieldBlock)enemies).AddElement();
                                        }
                                        foreach (var element in ((Bungie.Tags.TagFieldBlock)enemies).Elements)
                                        {
                                            ((Bungie.Tags.TagFieldBlockIndex)element.Fields[1]).Value = (short)runtimeEnemyObjects[rand.Next(0, runtimeEnemyObjects.Count)].PaletteIndex;
                                        }
                                        //Get the vehicle type field and set its index to a random vehicle
                                        var vehicleType = cell.Fields.Where(x => x.DisplayName == "vehicle type").FirstOrDefault();
                                        if (vehicleType != null)
                                        {
                                            ((TagFieldBlockIndex)vehicleType).Value = (short)runtimeVehicleObjectPaths[rand.Next(0, runtimeVehicleObjectPaths.Count)].PaletteIndex;
                                        }

                                    }
                                    else if (shouldSpawnMule)
                                    {
                                        RemoveAllCharactersOfCell(cell);
                                        RemoveAllWeaponsOfCell(cell);
                                        SetNormalDiffCountOfCell(cell, rand.Next(1, 4));
                                        //Check if there are elements, if not add one
                                        if (((Bungie.Tags.TagFieldBlock)enemies).Elements.Count == 0)
                                        {
                                            ((Bungie.Tags.TagFieldBlock)enemies).AddElement();
                                        }
                                        //Set all enemies to hunter
                                        foreach (var element in ((Bungie.Tags.TagFieldBlock)enemies).Elements)
                                        {
                                            ((Bungie.Tags.TagFieldBlockIndex)element.Fields[1]).Value = (short)runtimeEnemyObjects.Where(o => o.Name.Contains("mule")).FirstOrDefault().PaletteIndex;
                                        }
                                    }
                                    else if (shouldSpawnHunter)
                                    {
                                        RemoveAllCharactersOfCell(cell);
                                        RemoveAllWeaponsOfCell(cell);
                                        SetNormalDiffCountOfCell(cell, rand.Next(1, 4));
                                        //Check if there are elements, if not add one
                                        if (((Bungie.Tags.TagFieldBlock)enemies).Elements.Count == 0)
                                        {
                                            ((Bungie.Tags.TagFieldBlock)enemies).AddElement();
                                        }
                                        //Set all enemies to hunter
                                        foreach (var element in ((Bungie.Tags.TagFieldBlock)enemies).Elements)
                                        {
                                            ((Bungie.Tags.TagFieldBlockIndex)element.Fields[1]).Value = (short)runtimeEnemyObjects.Where(o => o.Name.Contains("hunter")).FirstOrDefault().PaletteIndex;
                                        }
                                    }
                                    else if (shouldSpawnEngineer)
                                    {
                                        RemoveAllCharactersOfCell(cell);
                                        RemoveAllWeaponsOfCell(cell);
                                        SetNormalDiffCountOfCell(cell, 1);

                                        //Check if there are elements, if not add one
                                        if (((Bungie.Tags.TagFieldBlock)enemies).Elements.Count == 0)
                                        {
                                            ((Bungie.Tags.TagFieldBlock)enemies).AddElement();
                                        }
                                        //Set all enemies to hunter
                                        foreach (var element in ((Bungie.Tags.TagFieldBlock)enemies).Elements)
                                        {
                                            ((Bungie.Tags.TagFieldBlockIndex)element.Fields[1]).Value = (short)runtimeEnemyObjects.Where(o => o.Name.Contains("engineer")).FirstOrDefault().PaletteIndex;
                                        }
                                    }
                                    else
                                    {
                                        RemoveAllWeaponsOfCell(cell);
                                        var randomWeapon = runtimeWeapons[rand.Next(0, runtimeWeapons.Count)];
                                        randomWeapon.CompatibleEnemies.RemoveAll(o => o.Name.Contains("hunter"));
                                        randomWeapon.CompatibleEnemies.RemoveAll(o => o.Name.Contains("engineer"));
                                        randomWeapon.CompatibleEnemies.RemoveAll(o => o.Name.Contains("mule"));


                                        //If the chosen weapon is not a plasma pistol ,remove buggers from compatible enemies list
                                        if (randomWeapon.Name != "plasma_pistol")
                                        {
                                            randomWeapon.CompatibleEnemies.RemoveAll(o => o.Name.Contains("bugger"));
                                        }
                                        SetWeaponOfCell(cell, rand, randomWeapon.PaletteIndex);
                                        int enemyCountForCell = 0;

                                        if (!string.IsNullOrEmpty(template))
                                        {
                                            enemyCountForCell = enemyCount;
                                        }
                                        else
                                        {
                                            enemyCountForCell = rand.Next(1, 4);
                                        }

                                        SetNormalDiffCountOfCell(cell, enemyCountForCell);


                                        randomWeapon.CompatibleEnemies = randomWeapon.CompatibleEnemies.OrderBy(x => rand.Next()).ToList();

                                        //Randomise order of compatible enemies
                                        RemoveAllCharactersOfCell(cell);


                                        List<string> EnemiesAdded = new List<string>();


                                        foreach (var compatibleEnemy in randomWeapon.CompatibleEnemies)
                                        {
                                            //Add element
                                            var enemyElements = ((Bungie.Tags.TagFieldBlock)enemies);


                                            //If enemy is already in the cell, skip it
                                            foreach (var element in enemyElements.Elements)
                                            {
                                                if (element.ElementHeaderText.ToLower() == compatibleEnemy.Name.ToLower())
                                                {
                                                    continue;
                                                }
                                            }

                                            //Get the count of the elements
                                            var count = enemyElements.Elements.Count;
                                            //Max enemies in a cell is 8
                                            if (count >= 8)
                                            {
                                                break;
                                            }



                                            enemyElements.AddElement();

                                            //Get the last element
                                            var tag = ((Bungie.Tags.TagFieldBlockElement)enemyElements.Elements[count]);
                                            //Get the character type field
                                            var tgb = (Bungie.Tags.TagFieldBlockIndex)tag.Fields[1];

                                            tgb.Value = (short)compatibleEnemy.PaletteIndex;

                                            EnemiesAdded.Add(compatibleEnemy.Name);
                                        }

                                        //Find all other weapons that are compatible with the enemies added
                                        var tempcompatibleWeapons = runtimeWeapons.Where(o => o.CompatibleEnemies.Any(x => EnemiesAdded.Contains(x.Name))).ToList();
                                        var compatibleWeapons = tempcompatibleWeapons.ToList();
                                        //Make sure that the weapons found are actually compatible with all the enemies
                                        foreach (var weapon in tempcompatibleWeapons)
                                        {
                                            foreach (var enemy in EnemiesAdded)
                                            {
                                                if (!weapon.CompatibleEnemies.Any(x => x.Name == enemy))
                                                {
                                                    compatibleWeapons.Remove(weapon);
                                                }
                                            }
                                        }

                                        //Remove the weapon that was chosen
                                        compatibleWeapons.Remove(randomWeapon);

                                        //Randomise the order of the compatible weapons
                                        compatibleWeapons = compatibleWeapons.OrderBy(x => rand.Next()).ToList();

                                        int i = 0;
                                        foreach(var weapon in compatibleWeapons)
                                        {
                                            if (i >= 4)
                                            {
                                                break;
                                            }
                                            AddWeaponToCell(cell, rand, weapon.PaletteIndex);
                                            i++;
                                        }
                                    }


                                }





















                            }
                        }





                    }







                    tagFile.Save();

                }





            }
            Bungie.ManagedBlamSystem.Stop();
        }

        




       

       
    }
    
    
}
