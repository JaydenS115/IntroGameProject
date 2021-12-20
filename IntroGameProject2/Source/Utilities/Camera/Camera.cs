using Godot;
using System;

public class Camera : Camera2D
{
    // refer to player node for the camera's use
    private static Player player;

    // last frame's mouse position
    private static Vector2 lastMousePosition;


    // camera zoom variables - adjustable in editor
    [Export(PropertyHint.Range, "0.1,1,0.05")]
    private Vector2 minZoomFactor = new Vector2(0.5f, 0.5f);

    [Export(PropertyHint.Range, "1,5,0.1")]
    private Vector2 maxZoomFactor = new Vector2(2.5f, 2.5f);

    [Export(PropertyHint.Range, "0.01,0.2,0.01")]
    private float zoomStepFactor = 0.1f;

    private Vector2 desiredZoom; // internally-stored level to zoom to smoothly


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Current = true; // set this camera to be the active one

        player = GetNode<Player>("../Entities/Player");
        SnapTo(player.GlobalPosition);

        lastMousePosition = GetGlobalMousePosition();

        desiredZoom = Zoom;
    }


    // instantly move to given global position without smoothing
    public void SnapTo(Vector2 globalPos) 
    {
        GlobalPosition = globalPos;
        Align();
    }


    // Handle Camera Controls
    public override void _Process(float delta)
    {

        // Zoom in - by the specified step amount
        if(Input.IsActionJustReleased("camera_zoom_in")) 
        {
            desiredZoom *= (1f - zoomStepFactor);
            //Zoom = new Vector2(Zoom.x * (1f-zoomStepFactor), Zoom.y * (1f-zoomStepFactor));

            if(desiredZoom.LengthSquared() < minZoomFactor.LengthSquared()) desiredZoom = minZoomFactor;
        }

        // Zoom out - by the specified step amount
        else if(Input.IsActionJustReleased("camera_zoom_out")) 
        {
            desiredZoom *= (1f + zoomStepFactor);
            //Zoom = new Vector2(Zoom.x*(1f+zoomStepFactor), Zoom.y*(1f+zoomStepFactor));

            if(desiredZoom.LengthSquared() > maxZoomFactor.LengthSquared()) desiredZoom = maxZoomFactor;
        }


        // Pan the Camera using click-and-drag
        if(Input.IsActionPressed("camera_pan")) 
        {
            // move camera to midpoint of mouse pos & player position
            GlobalPosition = ( (player.GlobalPosition + GetGlobalMousePosition()) / 2 );
        }
        // focus cam on player if focus button pressed or scene not paused
        else if( !GetTree().Paused  ||  Input.IsActionPressed("camera_focus_player")) 
        {
            // glide to player location
            GlobalPosition = player.GlobalPosition; 
        }
        else // not paused, move cam with movement controls
        {
            // TODO
        }


        // update current mouse pos
        lastMousePosition = GetGlobalMousePosition();

        // update camera zoom smoothly
        Zoom = Zoom.LinearInterpolate(desiredZoom, delta);

    }

}
