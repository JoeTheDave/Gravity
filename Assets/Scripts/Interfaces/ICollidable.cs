using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollidable
{
    public float XPosition { get; set; }
    public float YPosition { get; set; }
    public float Velocity { get; set; }
    public float Trajectory { get; set; }
    public float XVelocity { get; set; }
    public float YVelocity { get; set; }
    public float Size { get; set; }
    public float Mass { get; set; }

    public float DistanceFrom(ICollidable other);
    public float DistanceFrom(float x, float y);
    public bool isCollidingWith(ICollidable other, float sizeCoefficient);
    public void AdvancePosition(float delta);
}
