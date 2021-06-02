using System;
using System.Numerics;
using ComputerGraphics.RayTracing.Core.Entities;
using ComputerGraphics.RayTracing.Core.Entities.Lights;

namespace ComputerGraphics.SceneLoader.Mapping
{
    internal class LightMapper : IMapper<Light, SceneFormat.Light>
    {
        private readonly IMapper<Transform, SceneFormat.Transform> _transformMapper;

        public LightMapper(IMapper<Transform, SceneFormat.Transform> transformMapper) => _transformMapper = transformMapper;

        public Light Map(SceneFormat.Light input)
        {
            Light mappedLight = input.LightCase switch
            {
                SceneFormat.Light.LightOneofCase.Directional => new DirectionalLight(),
                SceneFormat.Light.LightOneofCase.Environment => new EnvironmentalLight(),
                SceneFormat.Light.LightOneofCase.Point => new PointLight(),
                SceneFormat.Light.LightOneofCase.Sphere => new SphereLight() {Radius = (float) input.Sphere.Radius},
                SceneFormat.Light.LightOneofCase.None => throw new NotImplementedException(),
                _ => throw new ArgumentOutOfRangeException(nameof(SceneFormat.Light.LightOneofCase))
            };
            mappedLight.Color = input.Color is null
                ? Vector3.One
                : new Vector3(mappedLight.Color.X, mappedLight.Color.Y, mappedLight.Color.Z);
            return mappedLight;
        }
    }
}