using Godot;
using System;

// Base Action class - for all Actions to inherit from
public abstract class Action : Node
{
    // whether or not the action is able to be interrupted
    [Export]
    public bool IsInterruptable {get; protected set;}

    // The entity upon which the action is conducted
    protected Entity entity;

    // On ready: stop this node's processing 
    // (until re-enabled through action handler)
    public override void _Ready() 
    {
        entity = GetNode<Entity>("../..");

        SetProcess(false);
        SetPhysicsProcess(false);
    }


    // Process the Action by given tick time in action handler
    // returns true if action self-completed this TickTime interval
    public abstract bool TickProcess(float tickTime);

    // Invoked when action started
    public abstract void Start();

    // Invoked when action finished
    public abstract void Stop();

}
