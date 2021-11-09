using Godot;
using System;

// Level script
public class Environment : Node
{
    // factor by which delta is multiplied to get previous tick time
    private static float TickFactor;

    // tick time of previous delta
    private static float TickTime;

    // start at normal tick speed
    public override void _Ready()
    {
        TickFactor = 1f;
    }

    // Process each delta time
    public override void _Process(float delta)
    {
        TickTime = delta * TickFactor;
    }

    public static float GetTickTime() { return TickTime; }

    public static void SetTickFactor(float tickFactor) {
        TickFactor = tickFactor;
    }

}
