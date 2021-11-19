using Godot;
using System;

public class Wait : Action
{

    public override void Start() 
    {
        IsInterruptable = true; // wait action is always interruptable
    }


    // just spend time waiting for action to replace this wait
    public override bool TickProcess(float tickTime) 
    {
        return false; // wait action never self-completes
    }

    public override void Stop() {}
    
}
