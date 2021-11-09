using Godot;
using System;

// Base class for all Entities
public class Entity : Area2D
{
	[Export]  // start position of the entity on the Hex Tile Grid
	public Vector2 StartingTilePosition;

	// the position of the entity in the TileMap
	public Vector2 TilePos;
	public Vector2 TargetTilePos;
	

	// tileMap and RayCast2D nodes, for movement and location
	private static TileMap tileMap;
	private RayCast2D collisionRay;

	// The Entity's currently-conducting action
	protected ActionHandler actionHandler;


	// initialize members
	public override void _Ready() 
	{
		tileMap = GetNode<TileMap>("../../Tiles/Gridlines");
		collisionRay = GetNode<RayCast2D>("RayCast2D");
		actionHandler = GetNode<ActionHandler>("ActionHandler");

		// set start location in grid
		TilePos = StartingTilePosition.Round();
		GlobalPosition = GetGlobalPos(); 
	}


	// return global pixel position in scene
	public Vector2 GetGlobalPos() 
	{
		Vector2 localPos = tileMap.MapToWorld(TilePos);
		localPos.x += (tileMap.CellSize.x/2);
		localPos.y += (tileMap.CellSize.y/2);
		return tileMap.ToGlobal(localPos);
	}
	
	// return global pixel position of a given Tile's coordinates
	public static Vector2 GlobalPosOfTile(Vector2 tilePosition) 
	{
		Vector2 localPos = tileMap.MapToWorld(tilePosition);
		localPos.x += (tileMap.CellSize.x/2);
		localPos.y += (tileMap.CellSize.y/2);
		return tileMap.ToGlobal(localPos);
	}

	// check if a direction is currently able to be moved in from 
	// the current position of the Entity
	public bool isDirectionMovable(HexDirection direction) 
	{
		if(direction == HexDirection.None) return false;

		// get global position of desired target tile to move to
		Vector2 targetPosition = GlobalPosOfTile(TilePositionOf(direction));
		
		// cast collisionRay and check for collision (with walls or any areas)
		return !(isCollision(targetPosition));
		// no collision means direction is movable
	}


	// return true if there is/are any collision(s) between the 
	// Entity and the given global position, otherwise false
	private bool isCollision(Vector2 globalPos) 
	{
		// cast ray from entity to position
		collisionRay.CastTo = globalPos - GlobalPosition;
		
		collisionRay.ForceRaycastUpdate();

		// if collision - return true;
		return collisionRay.IsColliding();
	}

	// return the tile position of a hex in the given direction,
	// relative to the current TilePos of the Entity
	public Vector2 TilePositionOf(HexDirection direction) {
		
		Vector2 targetTile = TilePos;

		switch (direction) {
			
			case HexDirection.UpLeft:
				if(TilePos.x % 2 == 0) --targetTile.y;
				--targetTile.x;
				break;

			case HexDirection.Up:
				--targetTile.y;
				break;

			case HexDirection.UpRight:
				if(TilePos.x % 2 == 0) --targetTile.y;
				++targetTile.x;
				break;

			case HexDirection.DownLeft:
				if(TilePos.x % 2 == 1) ++targetTile.y;
				--targetTile.x;
				break;

			case HexDirection.Down:
				++targetTile.y;
				break;

			case HexDirection.DownRight:
				if(TilePos.x % 2 == 1) ++targetTile.y;
				++targetTile.x;
				break;

			default:	// HexDirection.None
				break; 	// just returns current TilePos
		}

		return targetTile;
	}

}
