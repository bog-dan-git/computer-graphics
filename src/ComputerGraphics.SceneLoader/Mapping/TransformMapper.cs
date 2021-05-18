using System.Numerics;
using ComputerGraphics.RayTracing.Core.Entities;

namespace ComputerGraphics.SceneLoader.Mapping
{
    internal class TransformMapper : IMapper<Transform, SceneFormat.Transform>
    {
        public Transform Map(SceneFormat.Transform transform)
        {
            var mapped = new Transform();
            if (transform.Position is { })
            {
                mapped.Position = Map(transform.Position);
            }

            if (transform.Rotation is { })
            {
                mapped.Rotation = Map(transform.Rotation);
            }

            if (transform.Scale is { })
            {
                mapped.Scale = Map(transform.Scale);
            }

            return mapped;
        }

        private Vector3 Map(SceneFormat.Vector3 vector3) => new Vector3((float) vector3.X, (float) vector3.Y, (float) vector3.Z);
    }
}