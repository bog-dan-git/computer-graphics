using System;
using System.Numerics;
using ComputerGraphics.RayTracing.Core.Entities;
using ComputerGraphics.RayTracing.Core.Entities.Materials;
using ComputerGraphics.RayTracing.Core.Entities.SceneObjects;
using ComputerGraphics.RayTracing.Entities.Entities;
using Plane = ComputerGraphics.RayTracing.Entities.Entities.Plane;

namespace ComputerGraphics.SceneLoader.Mapping
{
    internal class SceneObjectMapper : IMapper<SceneObject, SceneFormat.SceneObject>
    {
        private readonly IMapper<Transform, SceneFormat.Transform> _transformMapper;
        private readonly IMapper<Material, SceneFormat.Material> _materialMapper;

        public SceneObjectMapper(IMapper<Transform, SceneFormat.Transform> transformMapper,
            IMapper<Material, SceneFormat.Material> materialMapper)
        {
            _transformMapper = transformMapper;
            _materialMapper = materialMapper;
        }

        public SceneObject Map(SceneFormat.SceneObject input)
        {
            SceneObject mapped = input.MeshCase switch
            {
                SceneFormat.SceneObject.MeshOneofCase.Cube => new Box()
                {
                    Size = new Vector3((float) input.Cube.Size.X, (float) input.Cube.Size.Y, (float) input.Cube.Size.Z)
                },
                SceneFormat.SceneObject.MeshOneofCase.Sphere => new Sphere()
                {
                    Radius = (float) input.Sphere.Radius
                },
                SceneFormat.SceneObject.MeshOneofCase.Plane => new Plane(),
                SceneFormat.SceneObject.MeshOneofCase.Disk => new Disk()
                {
                    Radius = (float) input.Disk.Radius
                },
                SceneFormat.SceneObject.MeshOneofCase.MeshedObject => new MeshObject()
                {
                    Reference = input.MeshedObject.Reference
                },
                SceneFormat.SceneObject.MeshOneofCase.None => throw new NotImplementedException(),
                _ => throw new ArgumentOutOfRangeException(nameof(input))
            };
            mapped.Id = input.Id;
            mapped.Transform = _transformMapper.Map(input.Transform);
            if (input.ObjectMaterialCase == SceneFormat.SceneObject.ObjectMaterialOneofCase.Material)
            {
                mapped.Material = _materialMapper.Map(input.Material);
            }
            return mapped;
        }
    }
}