using Godot;
using System;

public class Camera : Camera2D
{

    // refer to player and Viewport for the camera's use
    private static Area2D player;
    private static Viewport viewport;

    // last frame's mouse position
    private static Vector2 lastMousePosition;


    // camera zoom variables = adjustable in editor
    [Export]
    private Vector2 minZoomFactor = new Vector2(0.5f, 0.5f);
    [Export]
    private Vector2 maxZoomFactor = new Vector2(2f, 2f);
    [Export]
    private float zoomStepFactor = 0.05f;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Current = true; // set the camera to be the active one

        player = GetNode<Area2D>("../Entities/Player");
        SnapTo(player.GlobalPosition);

        viewport = GetViewport();
        lastMousePosition = getMousePos();
    }


    // instantly move to a global position without smoothing
    public void SnapTo(Vector2 globalPosition) 
    {
        GlobalPosition = globalPosition;
        Align();
    }


    // Handle Camera Controls
    public override void _Process(float delta)
    {

        // Zoom in - by the specified step amount
        if(Input.IsActionJustReleased("camera_zoom_in")) 
        {
            Zoom = new Vector2(Zoom.x * (1f-zoomStepFactor), Zoom.y * (1f-zoomStepFactor));

            if(Zoom.LengthSquared() < minZoomFactor.LengthSquared()) Zoom = minZoomFactor;
        }

        // Zoom out - by the specified step amount
        else if(Input.IsActionJustReleased("camera_zoom_out")) 
        {
            Zoom = new Vector2(Zoom.x*(1f+zoomStepFactor), Zoom.y*(1f+zoomStepFactor));

            if(Zoom.LengthSquared() > maxZoomFactor.LengthSquared()) Zoom = maxZoomFactor;
        }


        // Pan the Camera using click-and-drag
        if(Input.IsActionPressed("camera_pan")) 
        {
            SmoothingSpeed = 15f * Zoom.Length(); // Adjust speed based on zoom level

            Translate( lastMousePosition - getMousePos() ); // move camera
        }
        

        // focus cam on player
        if(Input.IsActionPressed("camera_focus_player"))
        {
            SmoothingSpeed = 5f;
            GlobalPosition = player.GlobalPosition;
        }


        // update current mouse pos
        lastMousePosition = getMousePos();

    }


    private static Vector2 getMousePos() {
        return viewport.GetMousePosition();
    }

}
