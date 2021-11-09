using Godot;
using System;

public class ActionHandler : Node
{
    // The currently-active Action
    public Action CurrentAction;

    // list of available actions
    public Godot.Collections.Array<Action> ActionsAvailable; 


    // The Entity of this Action Handler
    private Entity entity; 


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        entity = GetParent<Entity>();
        ActionsAvailable = new Godot.Collections.Array<Action>( GetChildren() );

        CurrentAction = null;
    }


    // Predicate to see if there is an active Action
    public bool hasActiveAction() { return CurrentAction != null; }


    // Process the currently active Action
    public override void _Process(float delta)
    {
        // if there is an action to Tick
        if( hasActiveAction() ) {

            // Tick CurrentAction, and if completed, stop the Action
            if( CurrentAction.Tick(Environment.GetTickTime()) ) {

                CurrentAction.Stop(); // Stop and Set to null
                CurrentAction = null;
            }
        }

    }


    // Set action to new type (if possible)
    public void SetAction(string actionName) 
    {
        SetAction(GetActionWithName(actionName));
    }

    public void SetAction(Action newAction) 
    {
        // no active Action
        if( !hasActiveAction() ) {
            CurrentAction = newAction; // Set and start new Action
            CurrentAction.Start();
        }
        
        // Active, but interruptable Action
        else if(CurrentAction.IsInterruptable) {
            CurrentAction.Stop(); // Stop current Action

            CurrentAction = newAction; // Set and start new Action
            CurrentAction.Start();
        }
    }


    // Clear the current Action, if possible
    public void ClearAction() 
    {
        if( hasActiveAction() && CurrentAction.IsInterruptable ) {
            CurrentAction.Stop();
            CurrentAction = null;
        }
    }

    // from the available actions, return the action with the given name
    private Action GetActionWithName(string actionName) 
    {
        for(int i = 0; i < ActionsAvailable.Count; ++i) 
        {
            if(ActionsAvailable[i].Name == actionName) 
            {
                return ActionsAvailable[i];
            }
        }

        return null;
    }


}
