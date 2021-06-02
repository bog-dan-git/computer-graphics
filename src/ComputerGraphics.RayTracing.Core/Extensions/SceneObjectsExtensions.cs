using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using ComputerGraphics.RayTracing.Core.Entities.SceneObjects;

namespace ComputerGraphics.RayTracing.Core.Extensions
{
    public static class SceneObjectsExtensions
    {
        public static float PdfValue(this IEnumerable<SceneObject> sceneObjects, in Vector3 o, in Vector3 v)
        {
            var weight = 1f / sceneObjects.Count();
            var sum = 0f;
            foreach (var obj in sceneObjects)
            {
                sum += weight * obj.PdfValue(o, v);
            }

            return sum;
        }

        public static Vector3 Random(this IEnumerable<SceneObject> sceneObjects, in Vector3 o)
        {
            var rnd = new Random();
            return sceneObjects.ElementAt(rnd.Next(0, sceneObjects.Count())).Random(o);
        }
    }
}