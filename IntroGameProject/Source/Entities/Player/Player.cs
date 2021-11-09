using Godot;
using System;

// Player Entity
public class Player : Entity
{
	// Input Handler - Move commands
    private HexDirection handleInput_Move()
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


	// temporary variable to store handling
	private HexDirection moveDirection;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{

        // handle movement based upon input
		moveDirection = handleInput_Move();
	    
		if(isDirectionMovable(moveDirection)) {

			// TEMPORARY MOVEMENT
			GlobalPosition = GlobalPosOfTile(TilePos = TilePositionOf(moveDirection));

		}
		

	}


}
