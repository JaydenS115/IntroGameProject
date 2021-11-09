using Godot;
using System;

public class Move : Action
{

    // Total time required to move
    [Export]
    private float TotalTimeToMove;
    private float timeTaken;


    private Vector2 startingGlobalPos;
    private Vector2 destinationGlobalPos;


    // At start, initialize starting position
    public override void Start()
    {
        startingGlobalPos = entity.GlobalPosition;
        destinationGlobalPos = Entity.GlobalPosOfTile(entity.TargetTilePos);

        timeTaken = 0f;
    }

    // Tick changes progress toward destination position
    public override bool Tick(float tickTime)
    {
        // get percent completion of action
        timeTaken += tickTime;
        float proportionComplete = Mathf.Clamp((timeTaken / TotalTimeToMove), 0f, 1f);

        entity.GlobalPosition = startingGlobalPos.LinearInterpolate(destinationGlobalPos, proportionComplete);

        // if move is complete, return so
        if(proportionComplete >= 1f) return true;
        return false;
    }


    // Stop - no explicit use needed for movement
    public override void Stop() {}

}
