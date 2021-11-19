using Godot;
using System;

// Tick Handler - Instantiate at base level node
public class Tick : Node
{
    // various constant tick factors stored by name
    public const float SpeedQuarter = 0.25f;
    public const float SpeedHalf = 0.5f;
    public const float SpeedNormal = 1f;
    public const float SpeedDouble = 2f;

    // factor by which delta is multiplied to get previous tick time
    public static float TickFactor {private get; set;}


    // tick time of previous delta
    public static float TickTime {get; private set;}


    // stored paused state of tick time
    public static bool isPaused {get; private set;}



    // start at normal tick speed
    public override void _EnterTree() {
        TickFactor = SpeedNormal;
    }


    // Process each delta time
    public override void _Process(float delta)
    {
        // set TickTime for this process iteration

        if( isPaused ) TickTime = 0f;

        else TickTime = delta * TickFactor;


        GD.Print(TickTime); // DEBUG PRINT: Tick Time this delta
    }


    // pause non-player entities by stopping tick time until resumed
    // returns true if the invocation paused the tick time
    public static bool Pause() 
    {
        if( isPaused ) return false; // already paused, do nothing

        isPaused = true; // set to paused state

        return true; // just paused successfully
    }


    // resume tick time
    // returns true if the invocation unpaused the tick time
    public static bool Unpause() 
    {
        // if already unpaused, do nothing
        if(  ! isPaused  ) return false;

        isPaused = false; // set to unpaused state

        return true; // tick now resumed successfully
    }


    // toggle pause state
    public static void TogglePause()
    {
        // if paused, unpause
        if( isPaused ) 
        {
            Unpause();
        }

        // if unpaused, pause
        else 
        {
            Pause();
        }

    }


}
