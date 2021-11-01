using Godot;
using System;

// Directions of movement within Hex Grid
public enum HexDirection
{
	UpLeft, 	Up, 	UpRight,
	DownLeft, 	Down, 	DownRight,
	None
}

// Tile Position data handler class for entities and others
public class TilePos : Node
{

	// Location within Hex Grid
	public int X;
	public int Y;


	// Default Constructor
	TilePos() {
		X = 0;
		Y = 0;
	}

	// Constructor - Given start location
	TilePos(int x, int y, int z) {
		X = x;
		Y = y;
	}

	// internal handler to convert to new Vector2 (x, y)
	private Vector2 toVector() {
		return new Vector2(X, Y);
	}

	// return Vector2 of global position in scene
	public Vector2 LocalPosition()
	{
		return GetNode<TileMap>("../../TileMap").MapToWorld(toVector());
	}

	public Vector2 GlobalPosition()
	{
		TileMap tileMap = GetNode<TileMap>("../../TileMap");
		Vector2 localPos = tileMap.MapToWorld(toVector());
		return tileMap.ToGlobal(localPos);
	}

}
