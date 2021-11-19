using Godot;
using System;

// Base class for all Entities
public class Entity : Area2D
{
	[Export]  // start position of the entity on the Hex Tile Grid
	private Vector2 StartingTilePosition;

	// the position of the entity in the TileMap
	public TilePosition TilePos {get; private set;}
	public TilePosition TargetTilePos;
	

	// tileMap and RayCast2D nodes, for movement and location
	private static TileMap tileMap;
	private RayCast2D collisionRay;
	private CollisionShape2D collisionShape;

	// The Entity's action handler node
	protected ActionHandler actionHandler;


	// initialize members
	public override void _Ready() 
	{
		tileMap = GetNode<TileMap>("../../Tiles/Gridlines");
		collisionRay = GetNode<RayCast2D>("RayCast2D");
		actionHandler = GetNode<ActionHandler>("ActionHandler");

		// set start location in grid
		TilePos = TargetTilePos = (TilePosition)StartingTilePosition;
		GlobalPosition = GetTileGlobalPos(); 
	}

	// return global pixel position in scene
	public Vector2 GetTileGlobalPos() 
	{
		Vector2 localPos = tileMap.MapToWorld((Vector2)TilePos);
		localPos.x += (tileMap.CellSize.x/2);
		localPos.y += (tileMap.CellSize.y/2);
		return tileMap.ToGlobal(localPos);
	}
	

	// return global pixel position of a given Tile's coordinates
	public static Vector2 GlobalPosOfTile(TilePosition tilePosition) 
	{
		Vector2 localPos = tileMap.MapToWorld((Vector2)tilePosition);
		localPos.x += (tileMap.CellSize.x/2);
		localPos.y += (tileMap.CellSize.y/2);
		return tileMap.ToGlobal(localPos);
	}

	// return TilePosition of global pixel position
	public static TilePosition TilePosOfGlobal(Vector2 globalPos) 
	{
		return ( (TilePosition)tileMap.WorldToMap(globalPos) );
	}

	public TilePosition setTilePos(TilePosition newTilePos) 
	{
		return (TilePos = newTilePos);
	}


	// check if a direction is currently able to be moved in from 
	// the current position of the Entity
	public bool isDirectionMovable(HexDirection direction) 
	{
		if(direction == HexDirection.None) return false;

		// get global position of desired target tile to move to
		Vector2 targetPosition = GlobalPosOfTile(TilePos.TilePositionOf(direction));
		
		// cast collisionRay and check for collision (with walls or any areas)
		return !(isCollision(targetPosition));
		// no collision means direction is movable
	}


	// return true if there is/are any collision(s) between the 
	// Entity and the given global position, otherwise false
	private bool isCollision(Vector2 globalPos) 
	{
		// cast ray from entity to position
		collisionRay.GlobalRotation = 0f;
		collisionRay.CastTo = globalPos - GlobalPosition;
		
		collisionRay.ForceRaycastUpdate();

		// if collision - return true;
		return collisionRay.IsColliding();
	}

}
