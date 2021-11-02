using Godot;
using System;


public class Entity : KinematicBody2D
{
	[Export]
	public Vector2 TilePos;


	/* return local pixel position relative to tilemap
	public Vector2 GetLocalPos() {

		TileMap tileMap = GetNode<TileMap>("../../TileMaps/Gridlines");
		Vector2 localPos = tileMap.MapToWorld(TilePos);
		localPos.x += tileMap.CellSize.x/2;
		localPos.y += tileMap.CellSize.y/2;
		return localPos;
	}
	*/

	// return global pixel position in scene
	public Vector2 GetGlobalPos() {

		TileMap tileMap = GetNode<TileMap>("../../TileMaps/Gridlines");
		Vector2 localPos = tileMap.MapToWorld(TilePos);
		localPos.x += tileMap.CellSize.x/2;
		localPos.y += tileMap.CellSize.y/2;
		return tileMap.ToGlobal(localPos);
	}


	// Change TilePos depending on move direction and current position
	public void MoveInDirection(HexDirection direction) {
		
		Vector2 desiredPos = TilePos;

		switch (direction) {
			
			case HexDirection.UpLeft:
				if(TilePos.x % 2 == 0) --desiredPos.y;
				--desiredPos.x;
				break;

			case HexDirection.Up:
				--desiredPos.y;
				break;

			case HexDirection.UpRight:
				if(TilePos.x % 2 == 0) --desiredPos.y;
				++desiredPos.x;
				break;

			case HexDirection.DownLeft:
				if(TilePos.x % 2 == 1) ++desiredPos.y;
				--desiredPos.x;
				break;

			case HexDirection.Down:
				++desiredPos.y;
				break;

			case HexDirection.DownRight:
				if(TilePos.x % 2 == 1) ++desiredPos.y;
				++desiredPos.x;
				break;

			default:
				break;
		}


		GlobalPosition = GetGlobalPos();
	}


}
