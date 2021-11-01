using Godot;
using System;

public class Player : Entity
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    	private HexDirection handle_MoveInput()
	{
		if(Input.IsActionJustPressed("move_upleft")) {
			return HexDirection.UpLeft;
		}
		if(Input.IsActionJustPressed("move_up")) {
			return HexDirection.Up;
		}
		if(Input.IsActionJustPressed("move_upright")) {
			return HexDirection.UpRight;
		}
		if(Input.IsActionJustPressed("move_downleft")) {
			return HexDirection.DownLeft;
		}
		if(Input.IsActionJustPressed("move_down")) {
			return HexDirection.Down;
		}
		if(Input.IsActionJustPressed("move_downright")) {
			return HexDirection.DownRight;
		}

		return HexDirection.None;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
        // handle movement based upon input
	    MoveInDirection(handle_MoveInput());

        // look at cursor
        LookAt(GetViewport().GetMousePosition());
	}


}
