using Godot;
using System;


public class Entity : KinematicBody2D
{
	[Export]
	public Vector2 TilePos;

	/* Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}
	*/

	// return local pixel position relative to tilemap
	public Vector2 GetLocalPos() {

		TileMap tileMap = GetNode<TileMap>("../../TileMaps/Gridlines");
		Vector2 localPos = tileMap.MapToWorld(TilePos);
		localPos.x += tileMap.CellSize.x/2;
		localPos.y += tileMap.CellSize.y/2;
		return localPos;
	}

	// return global pixel position
	public Vector2 GetGlobalPos() {

		TileMap tileMap = GetNode<TileMap>("../../TileMaps/Gridlines");
		Vector2 localPos = tileMap.MapToWorld(TilePos);
		localPos.x += tileMap.CellSize.x/2;
		localPos.y += tileMap.CellSize.y/2;
		return tileMap.ToGlobal(localPos);
	}


	public void MoveInDirection(HexDirection direction) {
		
		switch (direction) {
			
			case HexDirection.UpLeft:
				if(TilePos.x % 2 == 0) --TilePos.y;
				--TilePos.x;
				break;

			case HexDirection.Up:
				--TilePos.y;
				break;

			case HexDirection.UpRight:
				if(TilePos.x % 2 == 0) --TilePos.y;
				++TilePos.x;
				break;

			case HexDirection.DownLeft:
				if(TilePos.x % 2 == 1) ++TilePos.y;
				--TilePos.x;
				break;

			case HexDirection.Down:
				++TilePos.y;
				break;

			case HexDirection.DownRight:
				if(TilePos.x % 2 == 1) ++TilePos.y;
				++TilePos.x;
				break;
		}

		GlobalPosition = GetGlobalPos();
	}


}
