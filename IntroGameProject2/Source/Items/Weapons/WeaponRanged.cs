using Godot;
using System;


// Enumeration to store modes of ranged weapon fire
public enum FireMode : byte 
{
    None,
    SemiAuto,
    FullAuto,
    Charged
}


// Base class of all ranged weapons
public class WeaponRanged : Weapon
{
    [Export(PropertyHint.ResourceType, "Projectile")]
    private PackedScene projectile;
    
    // way in which the weapon is fired
    [Export]
    public FireMode fireMode {get; private set;}

    // time between shots (regardless of fire mode)
    [Export(PropertyHint.Range, "0,10,0.05,or_greater")]
    public float MinTimeBetweenShots = 0.5f;

    
    //[Export(PropertyHint.Range, "0.1,1.0,0.05")]
    private float timeScaleFire = Tick.SpeedThreeHalves; // time scale when firing
    
    //[Export(PropertyHint.Range, "0.1,1.0,0.05")]
    private float timeScaleAim = Tick.SpeedHalf; // time scale when aiming
    

    private float timeSinceLastShot;
    private bool canFire; // weapon is able to shoot

    
    // Variables used for charged weapons only
    private float chargeAmount;
    [Export(PropertyHint.Range, "0.1,1,0.05,or_greater")]
    private float chargeSpeed = 1.0f;

    private Entity entity = null; // the wielding entity
    private Line2D aimLine = null;



    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        timeSinceLastShot = 0f;
        chargeAmount      = 0f;


        //default starting values depend on weapon type
        switch(fireMode) 
        {
        case FireMode.SemiAuto :
            canFire = false;
            timeScaleFire = Tick.SpeedThreeQuarters;
            timeScaleAim  = Tick.SpeedHalf;
        break;
        
        case FireMode.FullAuto :
            canFire = false;
            timeScaleFire = Tick.SpeedNormal;
            timeScaleAim  = Tick.SpeedThreeQuarters;
        break;

        case FireMode.Charged :
            canFire = true;    // need to charge before canFire
            timeScaleFire = Tick.SpeedNormal;
            timeScaleAim  = Tick.SpeedThreeQuarters;
        break;
        }
    }


    // Called every frame
    public override void _Process(float delta)
    {
        timeSinceLastShot += delta;

        if(aimLine != null)
            UpdateAimLine();


        // processing depends on weapon type
        switch(fireMode) 
        {
        
        // Semi auto fire - one shot per trigger pull
        case FireMode.SemiAuto : 

            if( canFire  &&  (timeSinceLastShot >= MinTimeBetweenShots) )
            {
                canFire = false; // need to release trigger to fire again

                Shoot();
            }

        break;

        // Full Auto fire - multiple shots per trigger pull
        case FireMode.FullAuto :

            if( canFire  &&  (timeSinceLastShot >= MinTimeBetweenShots) )
            {
                Shoot();
            }

        break;

        // Charged fire - one burst per trigger RELEASE (only when charged)
        case FireMode.Charged :

            if( (chargeAmount > 0f)  &&  canFire  &&  (timeSinceLastShot >= MinTimeBetweenShots))
            {
                Shoot();
                chargeAmount = 0f;
            }

            if( !canFire ) // currently in charging phase
            {
                chargeAmount = Mathf.MoveToward(chargeAmount, 1.0f, chargeSpeed * delta);
            }

        break;
        }

    }


    // Primary Fire
    public override bool Use_1()
    {
        Tick.AddFactor(timeScaleFire);

        // action to take depends upon fire mode
        switch(fireMode) 
        {
        
        // Semi auto fire
        case FireMode.SemiAuto : 

            canFire = true;

        break;

        // Full Auto fire
        case FireMode.FullAuto :

            canFire = true;

        break;

        // Charged fire - one burst per trigger RELEASE when charged
        case FireMode.Charged :

            canFire = false;

        break;

        }
        
        return true;
    }


    // Release of primary fire
    public override void Use_1_Stop()
    {
        Tick.RemoveFactor(timeScaleFire);

        // action to take depends upon fire mode
        switch(fireMode) 
        {
        
        // Semi auto fire
        case FireMode.SemiAuto : 

            canFire = false;

        break;

        // Full Auto fire
        case FireMode.FullAuto :

            canFire = false;

        break;

        // Charged fire - one burst per trigger RELEASE when charged
        case FireMode.Charged :

            canFire = true;

        break;
        }

    }


    // Press Aim button
    public override bool Use_2()
    {
        Tick.AddFactor(timeScaleAim);

        aimLine.Visible = true;

        // action to take depends upon fire mode
        switch(fireMode) 
        {
        
        // Semi auto fire
        case FireMode.SemiAuto : 

        break;

        // Full Auto fire
        case FireMode.FullAuto :

        break;

        // Charged fire - one burst per trigger RELEASE when charged
        case FireMode.Charged :

        break;
        }

        return true;
    }

    // Release Aim button
    public override void Use_2_Stop()
    {
        Tick.RemoveFactor(timeScaleAim);

        aimLine.Visible = false;

        // action to take depends upon fire mode
        switch(fireMode) 
        {
        
        // Semi auto fire
        case FireMode.SemiAuto : 

        break;

        // Full Auto fire
        case FireMode.FullAuto :

        break;

        // Charged fire - one burst per trigger RELEASE when charged
        case FireMode.Charged :

        break;
        }

    }


    // grab whomever is wielding weapon
    public override bool Equip()
    {
        entity = GetNode<Entity>("../..");
        aimLine = entity.GetNode<Line2D>("AimLine");

        return true;
    }

    public override bool UnEquip()
    {
        entity = null;
        aimLine = null;

        return true;
    }


    // fire the associated projectile
    public virtual void Shoot() 
    {
        timeSinceLastShot = 0f; // reset shot timer

        /*
        GD.Print("DEBUG: WeaponRanged.Shoot(): no projectiles instantiated");
        return;
        */

        // instantiate projectile node
        Projectile newProjectile = (Projectile)projectile.Instance();

        // setup projectile start transform
        newProjectile.Start(entity.GlobalPosition, entity.GlobalRotation);

        // add to scene under Entities
        GetNode<Node2D>("../../..").AddChild(newProjectile);
    }


    private void UpdateAimLine() 
    {
        aimLine.GlobalRotation = 0f;
        aimLine.SetPointPosition(1, aimLine.GetLocalMousePosition());
    }
    
}
