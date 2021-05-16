using ComputerGraphics.Ioc.Framework;
using ComputerGraphics.RayTracing.Core.Entities;
using ComputerGraphics.RayTracing.Core.Entities.Cameras;
using ComputerGraphics.RayTracing.Core.Entities.Lights;
using ComputerGraphics.RayTracing.Core.Entities.Materials;
using ComputerGraphics.RayTracing.Core.Entities.SceneObjects;
using ComputerGraphics.RayTracing.Core.Interfaces;
using ComputerGraphics.SceneLoader.Mapping;

namespace ComputerGraphics.SceneLoader
{
    internal class DependencyInjection : Provider
    {
        protected override void LoadDependencies()
        {
            Bind<IMapper<Camera, SceneFormat.Camera>>()
                .To<CameraMapper>();
            Bind<IMapper<Light, SceneFormat.Light>>()
                .To<LightMapper>();
            Bind<IMapper<Transform, SceneFormat.Transform>>()
                .To<TransformMapper>();
            Bind<IMapper<Scene, SceneFormat.Scene>>()
                .To<SceneMapper>();
            Bind<IMapper<SceneObject, SceneFormat.SceneObject>>()
                .To<SceneObjectMapper>();
            Bind<IMapper<Material, SceneFormat.Material>>()
                .To<MaterialMapper>();
            Bind<ISceneLoader>()
                .To<SceneLoader>();
        }
    }
}