using Godot;
using System;

public class HUD : CanvasLayer
{
    private ColorRect timeEffect;
    private RichTextLabel timeLabel;

    public TextureRect itemTexture;

    private float elapsedTime;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        timeEffect = GetNode<ColorRect>("TimeEffect");
        timeLabel = GetNode<RichTextLabel>("TimeText");

        itemTexture = GetNode<TextureRect>("ItemTexture");

        elapsedTime = 0f;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        // visual effect to indicate time slowing
        timeEffect.Modulate = new Color(0.25f, 0.25f, 0.75f, Mathf.Clamp((1 - Engine.TimeScale), 0f, 1f));

        elapsedTime += delta;

        UpdateTimeText();
    }

    public void UpdateTimeText() 
    {
        timeLabel.Text = "Time: " + elapsedTime.ToString();
    }

}
