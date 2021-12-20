using Godot;
using System;

// Intermediate class for Projectiles
public class Projectile : KinematicBody2D 
{
    [Export(PropertyHint.Range, "0,5000,10,or_greater")]
    public float Speed;
    public Vector2 Velocity;

    [Export(PropertyHint.Range, "0,10,1,or_greater")]
    private int maxNumBounces = 2;
    private int numBounces = 0;

    [Export(PropertyHint.Range, "0.0,90.0,0.5")]
    private float maxBounceAngle = 30f;

    private float maxProjectileLifeTime = 1f;
    private float projectileLifeTime = 0f;


    // set the starting 
    public void Start(Vector2 startPosition, float rotationAngle) 
    {
        GlobalPosition = startPosition + new Vector2(25f, 0f).Rotated(rotationAngle) ;
        GlobalRotation  = rotationAngle;
        Velocity = new Vector2(Speed, 0f).Rotated(rotationAngle);
    }


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    // process movement of projectile
    public override void _PhysicsProcess(float delta)
    {

        // add to time alive to see if hit ground yet
        projectileLifeTime += delta;
        if(projectileLifeTime > maxProjectileLifeTime) {
            Destroy();
        }


        // move the projectile the appropriate amount
        var collision = MoveAndCollide(Velocity * delta);

        // there was a collision
        if(collision != null) 
        {
            // able to bounce again
            if(numBounces < maxNumBounces) 
            {
                // OLD: ( Mathf.Abs(90-(Mathf.Rad2Deg( collision.Normal.AngleTo(Velocity) ))) ) < maxBounceAngle

                float collisionAngle = Mathf.PosMod(Velocity.AngleTo(collision.Normal), Mathf.Pi/2);

                GD.Print(collisionAngle);
                GD.Print(Mathf.Deg2Rad(maxBounceAngle));

                // check if valid bounce angle
                if( collisionAngle  <  Mathf.Deg2Rad(maxBounceAngle) ) 
                {
                    Velocity = Velocity.Bounce(collision.Normal);
                    GlobalRotation = Velocity.Angle();

                    ++numBounces;
                }
                else // invalid bounce angle
                {
                    Destroy();
                }

            }
            else // no more bounce
            {
                Destroy();
            }
        }

    }

    // Destroy the projectile
    private void Destroy() 
    {
        QueueFree();
    }

}
