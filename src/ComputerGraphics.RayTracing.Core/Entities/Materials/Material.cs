﻿using System.Numerics;

namespace ComputerGraphics.RayTracing.Core.Entities.Materials
{
    public abstract class Material
    {
        public string Id { get; set; }
        public abstract bool Scatter(Ray inRay, HitResult result, out Vector3 attenuation, out Ray scattered);

        public virtual bool Scatter(in Ray inRay, in HitResult hitResult, out ScatterResult scatter)
        {
            scatter = new ScatterResult();
            return false;
        }

        public virtual float ScatteringPdf(in Ray ray, in HitResult hitResult, in Ray scattered)
        {
            return 0;
        }

        public virtual Vector3 Emitted() => Vector3.Zero;
    }
}