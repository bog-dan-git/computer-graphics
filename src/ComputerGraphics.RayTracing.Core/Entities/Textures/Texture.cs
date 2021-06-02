using System.Numerics;

namespace ComputerGraphics.RayTracing.Core.Entities.Textures
{
    public abstract class Texture
    {
        public abstract Vector3 ValueAt(float u, float v, Vector3 p);
    }
}