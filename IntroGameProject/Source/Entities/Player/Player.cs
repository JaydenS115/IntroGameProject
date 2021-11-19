using Godot;
using System;

// Player Entity
public class Player : Entity
{

	// Input Handler - Move commands
    private HexDirection handleInput_Move()
	{
		HexDirection direction = HexDirection.None;

		if(Input.IsActionJustPressed("move_upleft")) {
			direction = HexDirection.UpLeft;
		}
		else if(Input.IsActionJustPressed("move_up")) {
			direction = HexDirection.Up;
		}
		else if(Input.IsActionJustPressed("move_upright")) {
			direction = HexDirection.UpRight;
		}
		else if(Input.IsActionJustPressed("move_downleft")) {
			direction = HexDirection.DownLeft;
		}
		else if(Input.IsActionJustPressed("move_down")) {
			direction = HexDirection.Down;
		}
		else if(Input.IsActionJustPressed("move_downright")) {
			direction = HexDirection.DownRight;
		}

		return direction;
	}


	// Storage variable for the move direction last attempted
	private HexDirection dir = HexDirection.None;


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{

		// wait action - always interruptable
		if( Input.IsActionJustPressed("action_wait") ) 
		{
			if( actionHandler.SetAction("Wait") )
				Tick.TickFactor = Tick.SpeedQuarter;

		}


        // handle movement based upon input
		// check if direction selected is possible to be moved in
		if(  ( dir = handleInput_Move() )  !=  HexDirection.None  ) 
		{
			// if user has input a direction,
			// if the action can be interrupted, do so.
			if( actionHandler.ClearAction() ) 
			{
				// if the direction is actually movable
				if( isDirectionMovable( dir ) ) 
				{
					TargetTilePos = TilePos.TilePositionOf( dir ); // set target pos

					actionHandler.SetAction("Move"); // start move action - mostly uninterruptable

					Tick.TickFactor = Tick.SpeedNormal; // set to normal tick rate
				}
			}

		}
		

		// if the player no longer has an active action, tick pauses
		if (  ! actionHandler.hasActiveAction()  ) 
		{
			Tick.Pause();
		}


		// if just pressed tick pause toggle key
		if( Input.IsActionJustPressed("pause_tick") ) 
		{
			if( Tick.isPaused ) // if paused
			{
				Tick.Unpause(); // unpause

				// if unpause with no current action
				if(  ! actionHandler.hasActiveAction() ) 
				{
					// set action to waiting
					actionHandler.SetAction("Wait");

					Tick.TickFactor = Tick.SpeedQuarter;
				}
			}

			else // not currently paused
			{
				Tick.Pause();
			}

		}


	}


}
