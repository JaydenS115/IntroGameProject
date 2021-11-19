using Godot;
using System;

public class Move : Action
{

    // Total time required to move
    [Export]
    public float MoveTime;
    private float totalTimeToMove;
    private float timeTaken;

    // information about starting and ending locations
    private Vector2 startingGlobalPos;
    private Vector2 destinationGlobalPos;
    private TilePosition destinationTilePos;


    // reference to TileCollision of the moving Entity
    private Area2D tileCollider;



    // At start, initialize starting and ending positions
    public override void Start()
    {
        IsInterruptable = true; // movement is only interruptable at the complete start

        // set internal variables
        startingGlobalPos = entity.GlobalPosition;
        destinationGlobalPos = Entity.GlobalPosOfTile(destinationTilePos = entity.TargetTilePos);

        // reserve the tile's space
        tileCollider = entity.GetNode<Area2D>("TileCollision");
        tileCollider.GlobalPosition = destinationGlobalPos;

        timeTaken = 0f; // reset move timer
        totalTimeToMove = MoveTime;
    }


    // Tick changes progress toward destination position
    public override bool TickProcess(float tickTime)
    {
        // get percent completion of action
        timeTaken += tickTime;
        float proportionComplete = Mathf.Clamp((timeTaken / totalTimeToMove), 0f, 1f);

        // if movement has begun
        if(proportionComplete > 0f) { 
        
            IsInterruptable = false; // movement started - no interrupt possible

            // progress to target tile according to percent complete
            entity.GlobalPosition = startingGlobalPos.LinearInterpolate(destinationGlobalPos, proportionComplete);

            // keep tile collider in-place at destination while moving
            tileCollider.GlobalPosition = destinationGlobalPos;

            // halfway there means Entity has entered the targeted tile
            if(proportionComplete >= 0.5f) {
                entity.setTilePos( destinationTilePos );
            }

            // if move is complete, return boolean accordingly
            if(proportionComplete >= 1f) {
                entity.GlobalPosition = entity.GetTileGlobalPos();

                return true;
            }

            else { // still moving towards target position
                entity.LookAt( destinationGlobalPos );
            }

        }

        return false; // action not yet complete if this is reached
    }


    // Stop Action
    public override void Stop() 
    {
        // at end of move, or if interrupted, align tile collider to entity
        tileCollider.GlobalPosition = entity.GlobalPosition;
    }

}
