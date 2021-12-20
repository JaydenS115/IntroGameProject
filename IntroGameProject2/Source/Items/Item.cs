using Godot;
using System;

public class Item : Node2D
{
    // Texture of item
    [Export(PropertyHint.ResourceType, "Texture")]
    public Texture itemSprite = null;


    // Primary Use of item
    // returns false if unable to use, else true;
    public virtual bool Use_1() 
    {
        return false;
    }

    // Stop Primary Use of item
    public virtual void Use_1_Stop() {}


    // Secondary Use of item
    // returns false if unable to use, else true
    public virtual bool Use_2() 
    {
        return false;
    }

    // Stop Secondary Use of item
    public virtual void Use_2_Stop() {}


    // Attempt to Equip item
    // returns false if unable to equip, else true
    public virtual bool Equip() 
    {
        return true;
    }

    // Attempt to UnEquip item
    // returns false if unable to equip, else true
    public virtual bool UnEquip() 
    {
        return true;
    }
    
}
