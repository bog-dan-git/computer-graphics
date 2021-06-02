using System;
using System.Numerics;
using ComputerGraphics.RayTracing.Core.Entities.Materials;

namespace ComputerGraphics.SceneLoader.Mapping
{
    internal class MaterialMapper : IMapper<Material, SceneFormat.Material>
    {
        public Material Map(SceneFormat.Material input)
        {
            Material material = input.MaterialCase switch
            {
                SceneFormat.Material.MaterialOneofCase.LambertReflection => new LambertReflectionMaterial()
                {
                    Color = input.LambertReflection.Color is {} ? new Vector3((float) input.LambertReflection.Color.R,
                        (float) input.LambertReflection.Color.G, (float) input.LambertReflection.Color.B) : Vector3.One
                },
                SceneFormat.Material.MaterialOneofCase.SpecularReflection => new DielectricMaterial()
                {
                    RefractionIndex = (float) input.SpecularReflection.Eta
                },
                SceneFormat.Material.MaterialOneofCase.None => throw new NotSupportedException(),
                _ => throw new ArgumentOutOfRangeException()
            };
            return material;
        }
    }
}