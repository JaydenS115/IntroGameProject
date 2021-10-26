using Godot;
using System;

// Directions of movement within Hex Grid
enum HexDirection
{
	UpLeft, 	Up, 	UpRight,
	DownLeft, 	Down, 	DownRight
}

// Tile Position data handler class for entities and others
public class TilePos : Node
{

	// Location within Hex Grid
	public int X { get; private set; }
	public int Y { get; private set; }


	// Default Constructor
	TilePos() {
		X = 0;
		Y = 0;
		Z = 0;
	}

	// Constructor - Given start location
	TilePos(int x, int y, int z) {
		X = x;
		Y = y;
		Z = z;
	}

	// return Vector2 of global position in scene
	Vector2 GetGlobal()
	{
		// TODO: when dimension system is decided upon
	}

}
