using Godot;
using System;


// Tick Handler - attached to base level node
public class Tick : Node
{
    // various constant tick factors stored by name
    public const float SpeedMin           = 0.1f;
    public const float SpeedQuarter       = 0.25f;
    public const float SpeedHalf          = 0.5f;
    public const float SpeedThreeQuarters = 0.75f;
    public const float SpeedNormal        = 1.0f;
    public const float SpeedThreeHalves   = 1.5f;
    public const float SpeedMax           = 2.0f;

    // current scene's tree
    private static SceneTree scene; 

    public static float TimeScale {get; private set;}


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        scene = GetTree();

        Engine.TimeScale = TimeScale = SpeedNormal;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        
        // input handling for time pausing
        if(Input.IsActionJustPressed("time_pause_toggle")) 
        {
            Tick.TogglePause();
        }


        // smoothly transition between time scales
        if((Engine.TimeScale != TimeScale)) 
        {
            Engine.TimeScale = Mathf.MoveToward(Engine.TimeScale, TimeScale, delta);
        }

        // limit upper and lower bound of time speed
        if( (Engine.TimeScale < SpeedMin) ||
            (Engine.TimeScale > SpeedMax) ) 
        {
            Engine.TimeScale = Mathf.Clamp(Engine.TimeScale, SpeedMin, SpeedMax);
        }

    }


    // set time scale of engine to a new desired value
    // returns the previously-set value for the time scale
    public static float SetSpeed(float newFactor) {
        newFactor = Mathf.Clamp(newFactor, SpeedMin, SpeedMax);

        float prevFactor = TimeScale;

        TimeScale = newFactor; // set new time factor

        return prevFactor; // simply return what time scale was previously
    }

    // add a factor to the tick speed
    public static void AddFactor(float factor) 
    {
        TimeScale *= factor;
    }

    // remove (divide) a factor from the tick speed
    public static void RemoveFactor(float factor) 
    {
        TimeScale /= factor;
    }


    // simple predicate to check if scene is currently paused
    public static bool isPaused() 
    {
        return (scene.Paused);
    }

    // pause scene until resumed
    // returns true if the invocation paused the tick time
    public static bool Pause() 
    {
        if( scene.Paused ) return false; // already paused, do nothing

        scene.Paused = true; // set to paused state

        return true; // just paused successfully
    }


    // resume tick time
    // returns true if the invocation unpaused the tick time
    public static bool Unpause() 
    {
        // if already unpaused, do nothing
        if(  ! scene.Paused  ) return false;

        scene.Paused = false; // set to unpaused state

        return true; // tick now resumed successfully
    }


    // toggle pause state
    public static void TogglePause()
    {
        // if paused, unpause
        if( scene.Paused ) 
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
