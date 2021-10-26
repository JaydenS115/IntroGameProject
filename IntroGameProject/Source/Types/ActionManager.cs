using Godot;
using System;


public class ActionManager : Node
{
    
    private delegate void FunctionPointer (string s);

    struct Action {
        Entity entity;
        FunctionPointer function;

    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
