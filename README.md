# LandscapeGenerator
A full Unity project with a simple implementation of procedural terrain generation with rocks and trees.

![Island example](https://github.com/SintLucasAllStars/LandscapeGenerator/raw/master/screenshot.png)

In the [releases](https://github.com/SintLucasAllStars/LandscapeGenerator/releases) there is a Unity Package you can import in your own project with all the content of this project in its own namespace so it doesnt conflict with your scripts from class.

## Usage
There is a Landscape scene in the package. If you load that scene you will see an island. When in play mode the island is generated.
If you click the left mouse button the island gets re-generated (useful to tweak the terrain setting). If you click the right mouse button
a new seed will be set and a new island will be created.

There is a main camera that looks at the island and a FPSController that automatically spawns in the center of the island on play and when a 
new island is generated.

## Basic architecture
### ProceduralManager
The center point of the architecture, a Singleton that controls all the procedural generation

### ProceduralWorld
Manages the procedure and all the world data. This is basically a data representation of our world.

#### HeightPass
This defines a height pass (or octave) on the terrain generation, a heightmap can be generated with as many height passes as desired. Some passes can make mountains, others just small bumps, others a vignette effect to make your terrain an island etc...  
When the heightmap is generated on the ProceduralWorld all heightpasses will be added to generate the final result.

### Landscape
This is a base class to allow subclasses to render our world in different ways. In this project we only have one sub-class
(TerrainScape) that renders the world through the Unity Terrain tool.

#### Terrainscape
This is where we apply all our world data to the Terrain in our scene.

### PropGroup
This is a base class that represents a group of props for our world. They could be anything, like Rocks, Trees, Items, Houses etc...
There are two subclasses on this project:

#### PrefabLayer
This handles instantiating prefabs into the terrain. It is used on this project for the Rocks.

#### TreeLayer
Handles spawning trees on the terrain using TreeInstances from the Untity Terrain Tool.

