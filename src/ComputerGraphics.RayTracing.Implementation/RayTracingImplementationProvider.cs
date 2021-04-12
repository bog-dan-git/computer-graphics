using ComputerGraphics.Ioc.Framework;
using ComputerGraphics.RayTracing.Core.Interfaces;
using ComputerGraphics.RayTracing.Core.Services;
using ComputerGraphics.RayTracing.Implementation.Builders;
using ComputerGraphics.RayTracing.Implementation.Entities;
using ComputerGraphics.RayTracing.Implementation.Services;

namespace ComputerGraphics.RayTracing.Implementation
{
    public class RayTracingImplementationProvider : Provider
    {
        protected override void LoadDependencies()
        {
            Bind<ICameraProvider>().ToSingleton<StaticCameraProvider>();
            Bind<IScreenProvider>().ToSingleton<DefaultScreenProvider>();
            Bind<IRayProvider>().ToSingleton<CameraRayProvider>();
            Bind<ICamera>().To<StaticCamera>();
            Bind<IScene>().To<Scene>();
            Bind<IRayTracer>().To<MultiThreadedRayTracer>();
            Bind<ISceneFactory>().To<SceneFactory>();
            Bind<ITransformationMatrixBuilder>().To<TransposedTransformationMatrixBuilder>();
            Bind<ILightingStrategy>().To<FlatShading>();
            Bind<ILightProvider>().To<LightProvider>();
        }
    }
}