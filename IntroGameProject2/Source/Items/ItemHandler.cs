using Godot;
using System;

public class ItemHandler : Node
{
    // the currently-equipped item (or null if none)
    public Item CurrentItem {get; private set;}

    private Entity entity; // the associated entity for this item handler


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        CurrentItem = null;

        entity = GetParent<Entity>();
    }


    // Predicate to check if there is an active item
    public bool hasActiveItem() { return (CurrentItem != null); }


    // Primary Use of item, if available
    // returns true if use was successful, else false
    public bool Use_1() 
    {
        if( hasActiveItem() )  return CurrentItem.Use_1();

        return false; // no active item
    }

    // Stop Primary Use of item, if available
    public void Use_1_Stop() 
    {
        if( hasActiveItem() )  CurrentItem.Use_1_Stop();
    }


    // Secondary Use of item, if available
    // returns true if use successful, else false
    public bool Use_2() 
    {
        if( hasActiveItem() )  return CurrentItem.Use_2();

        return false; // no active item
    }

    // Stop Secondary Use of item, if available
    public void Use_2_Stop() 
    {
        if( hasActiveItem() )  CurrentItem.Use_2_Stop();
    }

    

    // Attempts to equip the item with the given name
    // returns true if item equipped, else false
    public bool EquipItem(string itemName) 
    {
        Item item = GetItemWithName(itemName);

        if( item != null ) 
        {
            return EquipItem( item ); // tru to equip
        }
        
        return false; // item name not found
    }

    // Attempts to equip the given item
    // returns true if item equipped, else false
    public bool EquipItem(Item newItem) 
    {
        // check if has active item, and if can be unequipped
        if( hasActiveItem() ) 
        {
            if( !CurrentItem.UnEquip() )  return false; // can't unequip
        }

        // if this point reached, can try equip new item
        if( !newItem.Equip() )  return false; // can't equip

        // if this point reached, can equip new item (so, do it)
        CurrentItem = newItem;
        return true;
    }

    // Equip item of given index
    // returns true if item successfully equipped, else false
    public bool EquipItem(int itemIndex) 
    {
        if( (itemIndex >= 0)  &&
            (itemIndex < GetChildCount()) )
        {
            return EquipItem( (Item)GetChild(itemIndex) );
        }

        return false; // invalid index
    }


    // UnEquip the current item, if possible
    // return true if item unequipped, else false
    public bool UnEquipItem() 
    {
        if( hasActiveItem() ) 
        {
            if( !CurrentItem.UnEquip() )  return false; // can't unequip
        }

        // if this point reached, can unequip current item (so, do it)
        CurrentItem = null;
        return true;
    }

    // FORCE UnEquip the current item (always succeeds).
    // returns true if item would have permitted non-forced unequip, else false
    public bool Force_UnEquipItem() 
    {
        bool wouldHaveAllowed = true; // assume true if no active item

        if( hasActiveItem() ) 
        {
            wouldHaveAllowed = CurrentItem.UnEquip();
        }

        // This point is ALWAYS reached in this particular function
        CurrentItem = null;

        return wouldHaveAllowed; 
    }


    // from available items, return the item with the given name, or null if not found
    private Item GetItemWithName(string itemName) 
    {
        // get array of all available items for this entity
        Godot.Collections.Array<Item> ItemsAvailable = new Godot.Collections.Array<Item>( GetChildren() );

        for(int i = 0; i < ItemsAvailable.Count; ++i) // go thru all items
        {
            if(ItemsAvailable[i].Name == itemName) 
            {
                return ItemsAvailable[i]; // found item with given name
            }
        }

        return null; // item not found
    }


}
