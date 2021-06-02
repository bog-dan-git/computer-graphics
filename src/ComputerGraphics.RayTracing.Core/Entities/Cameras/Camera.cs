﻿using System.Numerics;

namespace ComputerGraphics.RayTracing.Core.Entities.Cameras
{
    public abstract class Camera
    {
        public int Id { get; set; }
        public Transform Transform { get; set; }
        public Vector3 Origin => Vector3.Zero;
        public Vector3 Direction => new(0, 0, 1);
        
        public abstract Ray GetRay(float x, float y);
        
    }
}