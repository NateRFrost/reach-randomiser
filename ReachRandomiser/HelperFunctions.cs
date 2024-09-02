using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using Bungie;
using Bungie.Tags;
using static ReachTesting.FilePathsForReach;
namespace ReachTesting
{
    internal partial class Program
    {
        //Get TagFieldBlock from a TagFile by name(only top level)
        public static TagFieldBlock GetTagFieldBlock(TagFile tagFile, string blockName)
        {
            var field = tagFile.Fields.Where(x => x.DisplayName.ToLower().Contains(blockName)).FirstOrDefault();
            if (field != null)
            {
                return (TagFieldBlock)field;

            }
            return null;
        }


        //Get TagField from a TagFile by name(only top level)
        public static TagField GetTagField(TagFile tagFile, string fieldName)
        {
            var field = tagFile.Fields.Where(x => x.DisplayName.ToLower().Contains(fieldName)).FirstOrDefault();
            if (field != null)
            {
                return (TagField)field;

            }
            return null;
        }


        //Get Squads from a TagFile, return a tagfield
        public static TagField GetSquads(TagFile tagFile)
        {
            return GetTagField(tagFile, "squads");
        }

        //Get Equipment from a TagFile, return a tagfield
        public static TagField GetEquipment(TagFile tagFile)
        {
            return GetTagField(tagFile, "equipments");
        }

        //Get Weapons from a TagFile, return a tagfield
        public static TagField GetWeapons(TagFile tagFile)
        {
            return GetTagField(tagFile, "weapons");
        }

        //Get Scenery from a TagFile, return a tagfield
        public static TagField GetScenery(TagFile tagFile)
        {
            return GetTagFieldBlock(tagFile, "scenerys");
        }

        //Get Crates from a TagFile, return a tagfield
        public static TagField GetCrates(TagFile tagFile)
        {
            return GetTagFieldBlock(tagFile, "crates");
        }

        //Get Vehicle Palette from a TagFile, return a tagfield
        public static TagFieldBlock GetVehiclePalette(TagFile tagFile)
        {
            return GetTagFieldBlock(tagFile, "vehicle palette");
        }

        //Get Character Palette from a TagFile, return a tagfield
        public static TagFieldBlock GetCharacterPalette(TagFile tagFile)
        {
            return GetTagFieldBlock(tagFile, "character palette");
        }

        //Get Weapon Palette from a TagFile, return a tagfield
        public static TagFieldBlock GetWeaponPalette(TagFile tagFile)
        {
            return GetTagFieldBlock(tagFile, "weapon palette");
        }

        //Get Equipment Palette from a TagFile, return a tagfield
        public static TagFieldBlock GetEquipmentPalette(TagFile tagFile)
        {
            return GetTagFieldBlock(tagFile, "equipment palette");
        }


        //Get Scenery Palette from a TagFile, return a tagfield
        public static TagFieldBlock GetSceneryPalette(TagFile tagFile)
        {
            return GetTagFieldBlock(tagFile, "scenery palette");
        }

        //Get Crate Palette from a TagFile, return a tagfield
        public static TagFieldBlock GetCratePalette(TagFile tagFile)
        {
            return GetTagFieldBlock(tagFile, "crate palette");
        }

        //Add characters to a palette
        public static void AddCharactersToPalette(TagField tagField, List<EnemyObjectPaths> enemyList)
        {
            //Get the elements of the field
            var tagFieldBlock = (Bungie.Tags.TagFieldBlock)tagField;
            foreach (var EnemyType in enemyList)
            {
                bool found = false;
                //Check if the element is already in the palette
                foreach (var element in tagFieldBlock.Elements)
                {
                    if (element.ElementHeaderText.ToLower().Contains(EnemyType.Name.ToLower()))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    //Get the count of the elements
                    var count = tagFieldBlock.Elements.Count;
                    tagFieldBlock.AddElement();

                    //Get the last element
                    var tag = ((Bungie.Tags.TagFieldBlockElement)tagFieldBlock.Elements[count]);
                    var tgb = (Bungie.Tags.TagFieldReference)tag.Fields[0];

                    tgb.Path = Bungie.Tags.TagPath.FromPathAndType(EnemyType.Path, "char");
                }
            }

            //Loop through and set the palette index
            foreach (var element in tagFieldBlock.Elements)
            {
                foreach (var EnemyType in enemyList)
                {
                    if (element.ElementHeaderText.ToLower() == EnemyType.Name.ToLower())
                    {
                        EnemyType.PaletteIndex = element.ElementIndex;
                        break;
                    }
                }
            }
        }

        //Add vehicles to a palette
        public static void AddVehiclesToPalette(TagField tagField, List<VehicleObjectPaths> vehicleList)
        {
            //Get the elements of the field
            var tagFieldBlock = (Bungie.Tags.TagFieldBlock)tagField;
            foreach (var VehicleType in vehicleList)
            {
                bool found = false;
                foreach (var element in tagFieldBlock.Elements)
                {
                    if (element.ElementHeaderText.ToLower().Contains(VehicleType.Name.ToLower()))
                    {
                        //Remove the element
                        //tagFieldBlock.RemoveElement(element.ElementIndex);

                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    //Get the count of the elements
                    var count = tagFieldBlock.Elements.Count;
                    tagFieldBlock.AddElement();

                    //Get the last element
                    var tag = ((Bungie.Tags.TagFieldBlockElement)tagFieldBlock.Elements[count]);
                    var tgb = (Bungie.Tags.TagFieldReference)tag.Fields[0];

                    tgb.Path = Bungie.Tags.TagPath.FromPathAndType(VehicleType.Path, "vehi");
                    //Add the not found element
                }
            }

            //Loop through and set the palette index
            foreach (var element in tagFieldBlock.Elements)
            {
                foreach (var VehicleType in vehicleList)
                {
                    if (element.ElementHeaderText.ToLower() == VehicleType.Name.ToLower())
                    {
                        VehicleType.PaletteIndex = element.ElementIndex;
                        break;
                    }
                }
            }

        }

        //Add weapons to a palette
        public static void AddWeaponsToPalette(TagField tagField, List<WeaponDetails> weaponList)
        {
            //Get the elements of the field
            var tagFieldBlock = (Bungie.Tags.TagFieldBlock)tagField;
            foreach (var WeaponType in weaponList)
            {
                bool found = false;
                foreach (var element in tagFieldBlock.Elements)
                {
                    if (element.ElementHeaderText.ToLower().Contains(WeaponType.Name.ToLower()))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    //Get the count of the elements
                    var count = tagFieldBlock.Elements.Count;
                    tagFieldBlock.AddElement();

                    //Get the last element
                    var tag = ((Bungie.Tags.TagFieldBlockElement)tagFieldBlock.Elements[count]);
                    var tgb = (Bungie.Tags.TagFieldReference)tag.Fields[0];

                    tgb.Path = Bungie.Tags.TagPath.FromPathAndType(WeaponType.Path, "weap");
                    //Add the not found element
                }
            }

            //Loop through and set the palette index
            foreach (var element in tagFieldBlock.Elements)
            {
                foreach (var WeaponType in weaponList)
                {
                    if (element.ElementHeaderText.ToLower() == WeaponType.Name.ToLower())
                    {
                        WeaponType.PaletteIndex = element.ElementIndex;
                        break;
                    }
                }
            }
        }


        //Add equipment to a palette
        public static void EquipmentToPalette(TagField tagField, List<EquipmentDetails> equipmentList)
        {
            //Get the elements of the field
            var tagFieldBlock = (Bungie.Tags.TagFieldBlock)tagField;
            foreach (var EquipmentType in equipmentList)
            {
                bool found = false;
                foreach (var element in tagFieldBlock.Elements)
                {
                    if (element.ElementHeaderText.ToLower().Contains(EquipmentType.Name.ToLower()))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    //Get the count of the elements
                    var count = tagFieldBlock.Elements.Count;
                    tagFieldBlock.AddElement();

                    //Get the last element
                    var tag = ((Bungie.Tags.TagFieldBlockElement)tagFieldBlock.Elements[count]);
                    var tgb = (Bungie.Tags.TagFieldReference)tag.Fields[0];

                    tgb.Path = Bungie.Tags.TagPath.FromPathAndType(EquipmentType.Path, "eqip");
                    //Add the not found element
                }
            }
            //Loop through and set the palette index
            foreach (var element in tagFieldBlock.Elements)
            {
                foreach (var EquipmentType in equipmentList)
                {
                    if (element.ElementHeaderText.ToLower() == EquipmentType.Name.ToLower())
                    {
                        EquipmentType.PaletteIndex = element.ElementIndex;
                        break;
                    }
                }
            }
        }


        //Gets palette indexes of variants of a type of object (scenery, crates) into a variant list
        public static void GetVariantListIndexes(TagField tagField, List<VariantList> variantList)
        {
            //Get the elements of the field
            var tagFieldBlock = (Bungie.Tags.TagFieldBlock)tagField;
            //Loop through and set the palette index
            foreach (var element in tagFieldBlock.Elements)
            {
                foreach (var ObjectType in variantList)
                {
                    if (element.ElementHeaderText.ToLower() == ObjectType.Name.ToLower())
                    {
                        ObjectType.PaletteIndex = element.ElementIndex;
                        break;
                    }
                }
            }
        }

       

        //Check if a squad element has a template
        public static string HasTemplate(TagElement tagElement)
        {
            var find = ((Bungie.Tags.TagFieldBlockElement)tagElement).Fields.Where(x => x.DisplayName.ToString().ToLower().Contains("squad template index")).FirstOrDefault();
            if (find != null)
            {
                TagFieldElementInteger tfr = (TagFieldElementInteger)find;
                if (tfr.Data == -1)
                {
                }
                else
                {
                    //Get number of cells in the template and replicate them in this cell
                    //tfr.Data = -1;
                }

            }
            var findb = ((Bungie.Tags.TagFieldBlockElement)tagElement).Fields.Where(x => x.DisplayName.ToString() == "template").FirstOrDefault();
            if (findb != null)
            {
                TagFieldElementStringIDWithMenu tfr = (TagFieldElementStringIDWithMenu)findb;
                if (tfr.Data == "")
                {
                }
                else
                {
                    //Get number of cells in the template and replicate them in this cell
                    //tfr.Data = "";
                    return tfr.Data;
                }

            }
            return "";
        }

        //Get Enemy count from a squad template
        public static int GetEnemyCountFromTemplate(string template)
        {
            int enemyCount = 0;

            if (!string.IsNullOrEmpty(template))
            {
                //Find the template in the list of squad templates with exact name with prefix like "ai\squad_templates\{template}"
                var templatewithprefix = @"ai\squad_templates\" + template;
                var found = FilePathsForReach.SquadTemplates.Where(x => x.ToLower() == templatewithprefix.ToLower()).FirstOrDefault();
                if (found != null)
                {
                    //Open the template file
                    var tag_path = Bungie.Tags.TagPath.FromPathAndType(found, "sqtm*");
                    using (var tagFileTemplate = new Bungie.Tags.TagFile(tag_path))
                    {
                        //Get the "normal diff count" of the template
                        foreach (var element in tagFileTemplate.Elements)
                        {
                            //Loop through the elements of Fields[1] which is "Block:Cell Templates"
                            foreach (var field in ((Bungie.Tags.TagFieldBlock)element.Fields[1]).Elements)
                            {
                                //Loop through the elements of Fields[1] which is "Block:Cell Templates"
                                foreach (var ele_field in field.Fields)
                                {
                                    //If it is "Field:Name"
                                    if (ele_field.DisplayName.ToString().Contains("normal diff count"))
                                    {
                                        //Get ele_field.Data as a shortinteger
                                        var tfr = (TagFieldElementInteger)ele_field;
                                        enemyCount += (int)tfr.Data;

                                    }
                                }
                            }
                        }
                    }



                }
                else
                {
                    Console.WriteLine("Template not found: " + template);
                    Random random = new Random();
                    enemyCount = random.Next(1, 4);
                    Console.WriteLine("Random enemy count: " + enemyCount);

                }


            }
            return enemyCount;
        }

        //Remove template from squad
        public static void RemoveTemplate(TagElement tagElement)
        {
            var find = ((Bungie.Tags.TagFieldBlockElement)tagElement).Fields.Where(x => x.DisplayName.ToString().ToLower().Contains("squad template index")).FirstOrDefault();
            if (find != null)
            {
                TagFieldElementInteger tfr = (TagFieldElementInteger)find;
                if (tfr.Data == -1)
                {
                }
                else
                {
                    //Get number of cells in the template and replicate them in this cell
                    tfr.Data = -1;
                }

            }
            var findb = ((Bungie.Tags.TagFieldBlockElement)tagElement).Fields.Where(x => x.DisplayName.ToString() == "template").FirstOrDefault();
            if (findb != null)
            {
                TagFieldElementStringIDWithMenu tfr = (TagFieldElementStringIDWithMenu)findb;
                if (tfr.Data == "")
                {
                }
                else
                {
                    //Get number of cells in the template and replicate them in this cell
                    tfr.Data = "";
                }

            }
        }


        //check if a template has a vehicle
        public static bool TemplateHasVehicle(string template)
        {
            //Open the template file and check if it has a vehicle
            if (!string.IsNullOrEmpty(template))
            {
                //Find the template in the list of squad templates with exact name with prefix like "ai\squad_templates\{template}"
                var templatewithprefix = @"ai\squad_templates\" + template;
                var found = FilePathsForReach.SquadTemplates.Where(x => x.ToLower() == templatewithprefix.ToLower()).FirstOrDefault();
                if (found != null)
                {
                    //Open the template file
                    var tag_path = Bungie.Tags.TagPath.FromPathAndType(found, "sqtm*");
                    using (var tagFileTemplate = new Bungie.Tags.TagFile(tag_path))
                    {
                        //Get the "normal diff count" of the template
                        foreach (var element in tagFileTemplate.Elements)
                        {
                            //Loop through the elements of Fields[1] which is "Block:Cell Templates"
                            foreach (var field in ((Bungie.Tags.TagFieldBlock)element.Fields[1]).Elements)
                            {
                                //Loop through the elements of Fields[1] which is "Block:Cell Templates"
                                foreach (var ele_field in field.Fields)
                                {
                                    //If it is "Field:Name"
                                    if (ele_field.DisplayName.ToString().Contains("vehicle type"))
                                    {
                                        //Check if the vehichle type path is not null
                                        var tfr = (TagFieldReference)ele_field;
                                        if (tfr.Path != null)
                                        {
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
                else
                {
                    Console.WriteLine("Template not found: " + template);
                }

            }
            return false;
        }

        //Set the type of an object (ie weapon or scenery) in the world to a given index
        public static void SetType(TagElement objectElement, Random rand, int PaletteIndex)
        {
            //Get type field from elements[0]
            var typeField = ((Bungie.Tags.TagElement)objectElement).Fields.Where(x => x.DisplayName == "type").FirstOrDefault();
            if (typeField != null)
            {
                //Set the type to the random shortblockindex
                ((TagFieldBlockIndex)typeField).Value = (short)PaletteIndex;
            }
        }


         //Set the weapon type of a weapon placed in the world (not weapon crates)
        public static void SetWeapon(TagElement weapon, Random rand, int PaletteIndex)
        {
            SetType(weapon, rand, PaletteIndex);
            //clear weapon ammo so it goes to default values
            TagFieldStruct weaponData;
            weaponData = (TagFieldStruct)((Bungie.Tags.TagElement)weapon).Fields.Where(x => x.DisplayName == "weapon data").FirstOrDefault();
            if (weaponData != null)
            {
                var roundsLeft = ((Bungie.Tags.TagElement)weaponData.Elements.First()).Fields.Where(x => x.DisplayName == "rounds left").FirstOrDefault();
                if (roundsLeft != null)
                {
                    ((TagFieldElementInteger)roundsLeft).Data = 0;
                }
                else
                {
                    Console.WriteLine("rounds left not found");
                }
                var roundsLoaded = ((Bungie.Tags.TagElement)weaponData.Elements.First()).Fields.Where(x => x.DisplayName == "rounds loaded").FirstOrDefault();
                if (roundsLoaded != null)
                {
                    ((TagFieldElementInteger)roundsLoaded).Data = 0;
                }
            }
        }

        
        //Set the equipment type of equipment in the world (not equipment crates)
        public static void SetEquipment(TagElement equipment, Random rand, int PaletteIndex)
        {
            SetType(equipment, rand, PaletteIndex);
        }

        //Set the variant of a tag element
        public static void SetVariant(TagElement element, Random rand, string variantString)
        {
            TagFieldStruct permutationData;
            permutationData = (TagFieldStruct)((Bungie.Tags.TagElement)element).Fields.Where(x => x.DisplayName == "permutation data").FirstOrDefault();
            var variantName = ((Bungie.Tags.TagElement)permutationData.Elements.First()).Fields.Where(x => x.DisplayName == "variant name").FirstOrDefault();
            if (variantName != null)
            {
                //Set the equipment type to the random equipment shortblockindex
                ((TagFieldElementStringID)variantName).Data = variantString;
            }
            else
            {
                Console.WriteLine("variant name field not found");
            }
        }

        //Get the variant of a tag element
        public static string GetVariant(TagElement element)
        {
            var variantName = ((Bungie.Tags.TagElement)element).Fields.Where(x => x.DisplayName == "variant name").FirstOrDefault();
            if (variantName != null)
            {
                
                //Set the equipment type to the random equipment shortblockindex
                return ((TagFieldElementString)variantName).Data;

            }
            return null;
        }


        //Get the equipment type palette index of the equipment
        public static int GetEquipmentIndex(TagElement equipment)
        {
            return GetElementTypeIndex(equipment);
        }

        public static int GetWeaponIndex(TagElement weapon)
        {
            return GetElementTypeIndex(weapon);
        }

        //Get the type palette index of an element (such as scenery or crate)
        public static int GetElementTypeIndex(TagElement element)
        {
            //Get type field from elements[0]
            var typeField = ((Bungie.Tags.TagElement)element).Fields.Where(x => x.DisplayName == "type").FirstOrDefault();
            if (typeField != null)
            {
                //Get the equipment type
                return ((TagFieldBlockIndex)typeField).Value;
            }
            return -1;
        }

        //Gets the given name string index of a tag element (such as scenery, weapon, or a crate)
        public static int GetElementNameIndex(TagElement element)
        {
            //Get equipment type field from elements[0]
            var nameField = ((Bungie.Tags.TagElement)element).Fields.Where(x => x.DisplayName == "name").FirstOrDefault();
            if (nameField != null)
            {
                //Get the equipment type
                return ((TagFieldBlockIndex)nameField).Value;
            }
            return -1;
        }

        //Set the initial weapon of a cell with palette index
        public static void SetWeaponOfCell(TagElement cell, Random rand, int PaletteIndex)
        {
            //Choose a random weapon for the cell
            var weapon = cell.Fields.Where(x => x.DisplayName == "initial weapon").FirstOrDefault();
            if (weapon != null)
            {
                if (((TagFieldBlock)weapon).Elements.Count < 1)
                {
                    //Add an element
                    ((TagFieldBlock)weapon).AddElement();
                }
                //Get weapon type field from elements[0]
                var weaponType = ((Bungie.Tags.TagFieldBlock)weapon).Elements[0].Fields.Where(x => x.DisplayName == "weapon type").FirstOrDefault();
                if (weaponType != null)
                {
                    //Set the weapon type to the random weapon shortblockindex
                    ((TagFieldBlockIndex)weaponType).Value = (short)PaletteIndex;

                }


            }
        }

        //Add weapon to a cell with palette index
        public static void AddWeaponToCell(TagElement cell, Random rand, int PaletteIndex)
        {
            //Choose a random weapon for the cell
            var weapon = cell.Fields.Where(x => x.DisplayName == "initial weapon").FirstOrDefault();
            if (weapon != null)
            {
                //Check max elements
                if (((TagFieldBlock)weapon).Elements.Count >= 2)
                {
                    return;
                }

                //Get Element count
                var count = ((TagFieldBlock)weapon).Elements.Count;

                //Add an element
                ((TagFieldBlock)weapon).AddElement();

                //Get weapon type field from elements[0]
                var weaponType = ((Bungie.Tags.TagFieldBlock)weapon).Elements[count].Fields.Where(x => x.DisplayName == "weapon type").FirstOrDefault();
                if (weaponType != null)
                {
                    //Set the weapon type to the random weapon shortblockindex
                    ((TagFieldBlockIndex)weaponType).Value = (short)PaletteIndex;

                }
            }
        }

        //Remove all weapons from cell with RemoveAllElements()
        public static void RemoveAllWeaponsOfCell(TagElement cell)
        {
            //Choose a random weapon for the cell
            var weapon = cell.Fields.Where(x => x.DisplayName == "initial weapon").FirstOrDefault();
            if (weapon != null)
            {
                ((TagFieldBlock)weapon).RemoveAllElements();
            }
        }

        //Set normal diff count of a cell
        public static void SetNormalDiffCountOfCell(TagElement cell, int count)
        {
            //Find normal diff count
            var normalDiffCount = cell.Fields.Where(x => x.DisplayName == "normal diff count").FirstOrDefault();
            if (normalDiffCount != null)
            {
                //Set the normal diff count to the enemy count
                ((TagFieldElementInteger)normalDiffCount).Data = count;
            }

        }

        //Get normal diff count of a cell
        public static int GetNormalDiffCountOfCell(TagElement cell)
        {
            //Find normal diff count
            var normalDiffCount = cell.Fields.Where(x => x.DisplayName == "normal diff count").FirstOrDefault();
            if (normalDiffCount != null)
            {
                //Set the normal diff count to the enemy count
                return (int)((TagFieldElementInteger)normalDiffCount).Data;
            }
            return 0;

        }


        //Remove all characters from cell with RemoveAllElements()
        public static void RemoveAllCharactersOfCell(TagElement cell)
        {
            //Choose a random weapon for the cell
            var characters = cell.Fields.Where(x => x.DisplayName == "character type").FirstOrDefault();
            if (characters != null)
            {
                ((TagFieldBlock)characters).RemoveAllElements();
            }
        }


        //Get "designer zones" from tagfile
        public static TagField GetDesignerZones(TagFile tagFile)
        {
            foreach (var field in tagFile.Fields)
            {
                if (field.DisplayName == "designer zones")
                {
                    return field;
                }
            }
            return null;
        }

        //copies all files/folders from one directory to another
        public static void CopyAll(string source, string target)
        {
            foreach (var directory in Directory.GetDirectories(source))
            {
                var newDirectory = Path.Combine(target, Path.GetFileName(directory));
                Directory.CreateDirectory(newDirectory);
                //Recursively clone the directory
                CopyAll(directory, newDirectory);
            }

            foreach (var file in Directory.GetFiles(source))
            {
                File.Copy(file, Path.Combine(target, Path.GetFileName(file)), true);
            }
        }

        //Takes an object category (ie scenery, crates) and randomizes certain types into certain variants 
        public static void RandomizeVariants(TagPath tagPath, string objectCategory, List<VariantList> runTimeVariantsList, Random rand)
        {
            using (var tagFile = new Bungie.Tags.TagFile(tagPath))
            {
                var newPalette = tagFile.Fields.Where(x => x.DisplayName.Contains(objectCategory + " palette")).FirstOrDefault();
                if (newPalette != null)
                {
                    GetVariantListIndexes(newPalette, runTimeVariantsList);
                }
                else
                {
                    Console.WriteLine("Palette not found");
                }

                //Randomize variants
                TagField objectField = GetTagField(tagFile, objectCategory + "s");
                if (objectField == null)
                {
                    Console.WriteLine("objectField is null");
                    return;
                }
                foreach (var objectFieldBlock in ((Bungie.Tags.TagFieldBlock)objectField).Elements)
                {
                    //only randomize desired variants (no randomizing jetpack cases to avoid softlock)
                    //todo: fix to only check the m50 (exodus) jetpacks
                    if (runTimeVariantsList.Any(x => x.PaletteIndex == GetElementTypeIndex(objectFieldBlock)) && GetVariant(objectFieldBlock) != "jetpack_2")
                    {
                        var sceneryVariantList = runTimeVariantsList.First(x => x.PaletteIndex == GetElementTypeIndex(objectFieldBlock));
                        var randomVariant = sceneryVariantList.Variants[rand.Next(0, runTimeVariantsList.Count)];
                        SetVariant(objectFieldBlock, rand, randomVariant);
                    }
                }
                tagFile.Save();
            }
        }


        //randomizes weapons in the world
        public static void RandomizeWeapons(TagPath weaponfile, List<WeaponDetails> runtimeWeapons, Random rand)
        {
            using (var secondaryPalette = new Bungie.Tags.TagFile(weaponfile))
            {
                //Randomize weapons (not ones held by ai)
                TagField weapons = GetWeapons(secondaryPalette);
                if (weapons == null)
                {
                    Console.WriteLine("Weapons is null");
                    return;
                }
                foreach (var weapon in ((Bungie.Tags.TagFieldBlock)weapons).Elements)
                {
                    if (runtimeWeapons.Any(x => x.PaletteIndex == GetWeaponIndex(weapon)))
                    {
                        var randomWeapon = GetWeightedWeapon(runtimeWeapons, rand);//runtimeWeapons[rand.Next(0, runtimeWeapons.Count)];
                        SetWeapon(weapon, rand, randomWeapon.PaletteIndex);
                    }
                }
                secondaryPalette.Save();
            }
        }

        //Randomizes Equipment in the world
        public static void RandomizeEquipment(TagPath equipmentfile, List<EquipmentDetails> runtimeEquipment, Random rand)
        {
            using (var secondaryPalette = new Bungie.Tags.TagFile(equipmentfile))
            {
                var newEquipmentPalette = secondaryPalette.Fields.Where(x => x.DisplayName.Contains("equipment palette")).FirstOrDefault();
                TagField equipments = GetEquipment(secondaryPalette);
                if (equipments == null)
                {
                    Console.WriteLine("Equipments is null");
                    return;
                }
                foreach (var equipment in ((Bungie.Tags.TagFieldBlock)equipments).Elements)
                {
                    //only randomize armor abilities to other armor abilities
                    if (runtimeEquipment.Any(x => x.PaletteIndex == GetEquipmentIndex(equipment)))
                    {
                        var randomEquipment = runtimeEquipment[rand.Next(0, runtimeEquipment.Count)];
                        SetEquipment(equipment, rand, randomEquipment.PaletteIndex);
                    }
                }
                secondaryPalette.Save();
            }
        }

        //Adds all the needed weapons to the weapon palette
        public static void AddWeaponsToTag(TagPath weaponfile, List<WeaponDetails> runtimeWeapons)
        {
            using (var secondaryPalette = new Bungie.Tags.TagFile(weaponfile))
            {
                var newWeaponPalette = secondaryPalette.Fields.Where(x => x.DisplayName.Contains("weapon palette")).FirstOrDefault();
                if (newWeaponPalette != null)
                {
                    AddWeaponsToPalette(newWeaponPalette, runtimeWeapons);
                }
                secondaryPalette.Save();

            }
        }

        //Adds all the needed vehicles to the vehicle palette
        public static void AddVehiclesToTag(TagPath vehiclefile, List<VehicleObjectPaths> runtimeVehicleObjectPaths)
        {
            using (var secondaryPalette = new Bungie.Tags.TagFile(vehiclefile))
            {
                var newVehiclePalette = secondaryPalette.Fields.Where(x => x.DisplayName.Contains("vehicle palette")).FirstOrDefault();
                if (newVehiclePalette != null)
                {
                    AddVehiclesToPalette(newVehiclePalette, runtimeVehicleObjectPaths);
                }

                secondaryPalette.Save();

            }
        }

        //Adds all the needed equipment to the equipment palette
        public static void AddEquipmentToTag(TagPath equipmentfile, List<EquipmentDetails> runtimeEquipment)
        {
            using (var secondaryPalette = new Bungie.Tags.TagFile(equipmentfile))
            {
                var newEquipmentPalette = secondaryPalette.Fields.Where(x => x.DisplayName.Contains("equipment palette")).FirstOrDefault();
                if (newEquipmentPalette != null)
                {
                    EquipmentToPalette(newEquipmentPalette, runtimeEquipment);
                }
                else
                {
                    Console.WriteLine("Equipment Palette not found");
                }
                secondaryPalette.Save();
            }
        }

        public static void RandomizeProfiles(TagFile scenario, Random rand, List<WeaponDetails> runtimeWeapons, List<EquipmentDetails> runtimeEquipment)
        {
            var profiles = scenario.Fields.Where(x => x.DisplayName.Contains("player starting profile")).FirstOrDefault();
            if (profiles != null)
            {
                foreach (var profile  in ((Bungie.Tags.TagFieldBlock)profiles).Elements)
                {
                    var primaryType = GetWeightedWeapon(runtimeWeapons, rand);
                    var secondaryType = GetWeightedWeapon(runtimeWeapons, rand);
                    var equipmentType = runtimeEquipment[rand.Next(0, runtimeEquipment.Count)];
                    foreach (var field in profile.Fields)
                    {
                        
                        if (field.FieldName == "primary weapon")
                        {
                            ((TagFieldReference)field).Path = Bungie.Tags.TagPath.FromPathAndType(primaryType.Path, "weap");
                        }
                        else if (field.FieldName == "secondary weapon")
                        {
                            ((TagFieldReference)field).Path = Bungie.Tags.TagPath.FromPathAndType(secondaryType.Path, "weap");
                        }
                        else if (field.FieldName == "primaryrounds loaded")
                        {
                            ((TagFieldElementInteger)field).Data = primaryType.AmmoMag;
                        }
                        else if (field.FieldName == "secondaryrounds loaded")
                        {
                            ((TagFieldElementInteger)field).Data = secondaryType.AmmoMag;
                        }
                        else if (field.FieldName == "primaryrounds total")
                        {
                            ((TagFieldElementInteger)field).Data = primaryType.AmmoTotal;
                        }
                        else if (field.FieldName == "secondaryrounds total")
                        {
                            ((TagFieldElementInteger)field).Data = secondaryType.AmmoTotal;
                        }
                        var grenadeValues = new List<string> {"starting fragmentation grenade count", "starting plasma grenade count"};
                        if (grenadeValues.Contains(field.FieldName, StringComparer.OrdinalIgnoreCase))
                        {
                            ((TagFieldElementInteger)field).Data = rand.Next(0, 3);
                        }
                        else if (field.FieldName == "starting equipment")
                        {
                            ((TagFieldReference)field).Path = Bungie.Tags.TagPath.FromPathAndType(equipmentType.Path, "eqip");
                        }
                    }
                }
            }
        }

        public static TagField GetSceneObjects(TagFile tagFile)
        {
            foreach (var field in tagFile.Fields)
            {
                if (field.DisplayName == "objects")
                {
                    return field;
                }
            }
            return null;
        }

        public static TagField GetSceneShots(TagFile tagFile)
        {
            foreach (var field in tagFile.Fields)
            {
                if (field.DisplayName == "shots")
                {
                    return field;
                }
            }
            return null;
        }

        public static TagField GetDialogueFromShot(TagElement shotField)
        {
            foreach (var field in shotField.Fields)
            {
                if (field.DisplayName == "dialogue")
                {
                    return field;
                }
            }
            Console.WriteLine("dialogue not found");
            return null;
        }

        public static void RandomizeSceneObjects(TagField objectsField, Random rand)
        {
            
            foreach (var objectElement in ((TagFieldBlock)objectsField).Elements)
            {
                string objectTypePath = null;
                foreach (var field in objectElement.Fields)
                {
                    if (field.FieldName == "object type")
                    {

                        if (((TagFieldReference)field).Path != null)
                        {
                            objectTypePath = ((TagFieldReference)field).Path.RelativePath;

                            //randomize heads
                            //swapping heads seems to not have them display? leaving out for now
                            /*
                            if (HeadPaths.Any(x => objectTypePath.Contains(x)))
                            {
                                Console.WriteLine("randomizing head");
                                string randomHeadPath = HeadPaths[rand.Next(0, HeadPaths.Count)];
                                TagPath randomHead = Bungie.Tags.TagPath.FromPathAndType(randomHeadPath, "biped");
                                ((TagFieldReference)field).Path = randomHead;
                                Console.WriteLine("head: "+ ((TagFieldReference)field).Path.RelativePath);
                            }
                            */
                        }
                    }
                }
                
                if (objectTypePath != null && BipedVariants.Any(x => objectTypePath.Contains(x.Path)))
                {
                    foreach (var field in objectElement.Fields)
                    {
                        if (field.FieldName == "variant name")
                        {
                            var bipedVariantList = BipedVariants.First(x => x.Path == objectTypePath);
                            var randomVariant = bipedVariantList.Variants[rand.Next(0, bipedVariantList.Variants.Count)];
                            ((TagFieldElementStringID)field).Data = randomVariant;
                        }
                    }
                }

            }
        }

        public static void RandomizeDialogue(TagField shotsField, Random rand)
        {
            foreach (var shotElement in ((TagFieldBlock)shotsField).Elements)
            {
                var dialogueField = GetDialogueFromShot(shotElement);
                foreach (var dialogueElement in ((TagFieldBlock)dialogueField).Elements)
                {
                    Dialogue randomDialogue = DialogueList[rand.Next(0, DialogueList.Count)];
                    foreach (var field in dialogueElement.Fields)
                    {
                        if (field.DisplayName == "dialogue" || field.DisplayName == "female dialogue")
                        {
                            TagPath randomDialoguePath = Bungie.Tags.TagPath.FromPathAndType(randomDialogue.Path, "snd!");
                            ((TagFieldReference)field).Path = randomDialoguePath;
                        }
                        if (field.DisplayName == "subtitle" || field.DisplayName == "female subtitle")
                        {
                            ((TagFieldElementStringID)field).Data = randomDialogue.Subtitle;
                        }
                    }
                }
            }
        }

        public static void RandomizeCutscenes(Random rand)
        {
            foreach (var scene in ScenePaths)
            {
                var scenePath = Bungie.Tags.TagPath.FromPathAndType(scene, "cisc*");
                Console.WriteLine("randomizing cutscene: " + scenePath.RelativePath);
                using (var tagFile = new Bungie.Tags.TagFile(scenePath))
                {
                    var sceneObjects = GetSceneObjects(tagFile);
                    RandomizeSceneObjects(sceneObjects, rand);
                    var shotObjects = GetSceneShots(tagFile);
                    RandomizeDialogue(shotObjects, rand);
                    tagFile.Save();
                }
            }
        }

        public static EnemyObjectPaths GetWeightedCharacter(List<EnemyObjectPaths> enemyObjects, Random rand)
        {
            var max = enemyObjects.Sum(y => y.Weight);
            var randInt = rand.Next(max);
            var res = enemyObjects.FirstOrDefault(x => randInt >= (max -= x.Weight));
            return res;
        }

        public static VehicleObjectPaths GetWeightedVehicle(List<VehicleObjectPaths> vehicleObjects, Random rand)
        {
            var max = vehicleObjects.Sum(y => y.Weight);
            var randInt = rand.Next(max);
            var res = vehicleObjects.FirstOrDefault(x => randInt >= (max -= x.Weight));
            return res;
        }

        public static WeaponDetails GetWeightedWeapon(List<WeaponDetails> weaponObjects, Random rand)
        {
            var max = weaponObjects.Sum(y => y.Weight);
            var randInt = rand.Next(max);
            var res = weaponObjects.FirstOrDefault(x => randInt >= (max -= x.Weight));
            return res;
        }


    }
}
