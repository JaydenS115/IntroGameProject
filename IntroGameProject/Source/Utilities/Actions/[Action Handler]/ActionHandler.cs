using Godot;
using System;

public class ActionHandler : Node
{
    
    // The currently-active Action
    public Action CurrentAction;


    // list of available actions
    public Godot.Collections.Array<Action> ActionsAvailable; 


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //entity = GetParent<Entity>();
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
            if( CurrentAction.TickProcess(Tick.TickTime) ) {

                CurrentAction.Stop(); // Stop and Set to null
                CurrentAction = null;
            }
        }

    }


    // Set action to new type (if possible)
    // returns true if current action changed or set
    public bool SetAction(string actionName) 
    {
        Action action = GetActionWithName(actionName);

        if(action != null)
            return SetAction( action );
        else
            return ClearAction();
    }


    // Set the current action to the given action
    // returns true if action was changed
    public bool SetAction(Action newAction) 
    {
        // no active Action
        if( !hasActiveAction() ) {
            CurrentAction = newAction; // Set and start new Action
            CurrentAction.Start();

            return true;
        }
        
        // Active, but interruptable Action
        else if(CurrentAction.IsInterruptable) {
            CurrentAction.Stop(); // Stop current Action

            CurrentAction = newAction; // Set and start new Action
            CurrentAction.Start();

            return true;
        }
        
        // return false if action unchanged
        return false;
    }


    // Clear the current Action, if possible
    // return true if action is available or was stopped
    public bool ClearAction() 
    {
        if( hasActiveAction() ) 
        {
            if( CurrentAction.IsInterruptable ) 
            {
                CurrentAction.Stop();
                CurrentAction = null;

                return true; // action successfully stopped
            }

            else {return false;} // action unable to be stopped

        }

        else { return true; } // no action to stop
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
