using Godot;
using System;

public class Prop : Sprite
{
    [Export(PropertyHint.Range, "0.1,1,0.05")]
    private float normalAlpha = 1.0f;

    [Export(PropertyHint.Range, "0.1,1,0.05")]
    private float nearbyAlpha = 0.1f;


    // On start: set proper base color
    public void _Ready(float delta)
    {
        Modulate = new Color(1f, 1f, 1f, normalAlpha);
    }


    public void _on_body_enter(Node body)
    {
        // if it is player entity
        if(body.IsInGroup("Player"))
        {
            Modulate = new Color(1f, 1f, 1f, nearbyAlpha);
        }
    }

    public void _on_body_exit(Node body) 
    {
        // if it is player entity
        if(body.IsInGroup("Player")) 
        {
            Modulate = new Color(1f, 1f, 1f, normalAlpha);
        }
    }


}
