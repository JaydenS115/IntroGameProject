using Godot;
using System;

public class Player : Entity
{
    [Export(PropertyHint.Range, "200,600,5,or_greater")]
    private float moveAcceleration = 500f; // amount accelerating by player

    [Export(PropertyHint.Range, "50,300,5,or_greater")]
    private float maxMoveSpeed = 200f; // max move velocity

    private Vector2 velocity = Vector2.Zero; // current velocity of player
    private Vector2 acceleration = Vector2.Zero; // current acceleration of the player

    
    [Export(PropertyHint.Range, "2,20,0.1,or_greater")]
    private float maxTurnSpeed = 7.5f; // max rotation speed

    //private Sprite spriteTorso; // sprite of torso-up
    private Sprite spriteLegs; // sprite of leg portion

    private ItemHandler itemHandler; // item handling child

    private HUD headsUpDisplay;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //spriteTorso = GetNode<Sprite>("Sprite_Torso");
        spriteLegs = GetNode<Sprite>("Sprite_Legs");

        headsUpDisplay = GetNode<HUD>("../../HUD");

        itemHandler = GetNode<ItemHandler>("ItemHandler");

        itemHandler.EquipItem(0);
    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        // Handle look at mouse position
        Vector2 targetPos = GetGlobalMousePosition() - GlobalPosition;
        GlobalRotation = Mathf.LerpAngle(GlobalRotation, targetPos.Angle(), maxTurnSpeed*delta);

        // turn leg sprite in movement direction
            //spriteLegs.GlobalRotation = Mathf.LerpAngle(spriteLegs.GlobalRotation, velocity.Angle(), maxTurnSpeed*delta);
        spriteLegs.GlobalRotation = velocity.Angle();


        // input handling for item use

        // primary use of item
        if(Input.IsActionJustPressed("use_1")) // press button
        {
            itemHandler.Use_1();
        }
        else if(Input.IsActionJustReleased("use_1")) // release button
        {
            itemHandler.Use_1_Stop();
        }

        // secondary use of item
        if(Input.IsActionJustPressed("use_2")) // press button
        {
            itemHandler.Use_2();
        }
        else if(Input.IsActionJustReleased("use_2")) // release button
        {
            itemHandler.Use_2_Stop();
        }
        // END: input handling for item use


        // input handling for item swap
        if(Input.IsActionJustPressed("item_swap")) 
        {
            if(itemHandler.GetChild(0) == itemHandler.CurrentItem) {
                itemHandler.EquipItem(1);
            }
            else {
                itemHandler.EquipItem(0);
            }
        }


        headsUpDisplay.itemTexture.Texture = itemHandler.CurrentItem.itemSprite;

    }


    // Process physics calculations (movement, etc.)
    public override void _PhysicsProcess(float delta)
    {

        // Movement calculation
        acceleration = Vector2.Zero; // assume no movement until proven otherwise
        
        // input handling for movement
        if(Input.IsActionPressed("move_up")) 
        {
            acceleration.y -= moveAcceleration;
        }
        if(Input.IsActionPressed("move_down"))
        {
            acceleration.y += moveAcceleration;
        }
        if(Input.IsActionPressed("move_left")) 
        {
            acceleration.x -= moveAcceleration;
        }
        if(Input.IsActionPressed("move_right"))
        {
            acceleration.x += moveAcceleration;
        }
        // END: input handling for movement

        // add input acceleration to velocity
        velocity += acceleration * delta;
        
        // limit upper bound of velocity
        velocity = velocity.Clamped(maxMoveSpeed);

        // move according to current velocity
        velocity = MoveAndSlide(velocity);


        // slow velocity over time if not moving in that direction
        if(acceleration.x == 0f) {
            velocity.x *= 0.9f;
        }
        if(acceleration.y == 0f) {
            velocity.y *= 0.9f;
        }

        // END: Movement calculation

    }

}
