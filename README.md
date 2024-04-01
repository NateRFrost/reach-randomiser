[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
<a href="https://twitter.com/danosw_">
![X (formerly Twitter) Follow](https://img.shields.io/twitter/follow/danosw_)
</a>

# Reach-Randomiser

A dynamic C# console application leveraging managedblam to modify tags in Halo Reach's mod tools, introducing a new dimension of unpredictability by randomizing enemies, their weapons, weapons/equipment in the world, and starting weapons/equipment. Some parts of cutscenes are also randomized.

# Use of this tool requires basic knowledge of the Halo Reach Modding Tools (HREK)

You can use a pre-generated version of this mod using the steam workshop!
[Steam workshop link](https://steamcommunity.com/sharedfiles/filedetails/?id=3197941875)

## Installation and Usage
1. **Extract HREK**: Begin by extracting a fresh copy of `HREK.7z`.
2. **Run Reach-Randomiser**: Execute the Reach-Randomiser program. If necessary, adjust the default path of your HREK installation in the main function (default is C:\Program Files (x86)\Steam\steamapps\common\HREK).
3. **Package Scenario Files**: Package each scenario file using the provided tools in the HREK (use build-cache-file).
4. **Enjoy Randomized Maps**: The generated map files will now feature randomized enemies, enhancing the unpredictability and replayability of the game.

## Program Steps

- Load the scenario files e.g. m10, m20, m30
- Add all vehicles, characters and weapons to each scenarios palette.
  - For weapons and vehicles, this is also done on the scenarios resource files e.g. m10/resources/m10.scenario_vehicles_resource
- Loop through the designer zones which are used to load resources at specific points in the level to save memory on initial load.
  - Without this, some enemies, weapons and vehicles would not spawn until you got half way through the level.
- Get all the squads and loop through each of their cells.
  - If it has a template, it removes it.
  - Generates random weapons, characters and vehicles for the cells whilst retaining the original enemy count.
  - Your average Squad cell which is not a Guta/Hunter/Engineer or Vehicle will have a selection of 8 different enemies and up to 5 different weapons that they can spawn with.

## Prerequisites

- **.NET Framework 4.8**: Ensure you have .NET Framework 4.8 installed on your system.
- **Halo Reach Editing Kit**: This tool requires the Halo Reach Editing Kit (HREK) to be installed and properly set up.

## Contributing

Any contributions you make are **greatly appreciated**.

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## Acknowledgments

- A thanks to the HaloMods Discord community and the contributors at [c20.reclaimers.net](https://c20.reclaimers.net) for their brilliant documentation.
- A shoutout for [HaloRuns](https://haloruns.com) speedrun community for enabling my Halo addiction.
- This project would not be possible without it.

---
