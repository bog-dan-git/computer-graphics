using ComputerGraphics.Ioc.Framework;
using ComputerGraphics.RayTracing.Core.Interfaces;
using ComputerGraphics.RayTracing.Implementation.Services;

namespace ComputerGraphics.RayTracing.Implementation
{
    public class RayTracingImplementationDependencyProvider : DependencyProvider
    {
        protected override void LoadDependencies()
        {
            Bind<IScreenProvider>().ToSingleton<DefaultScreenProvider>();
            Bind<IRayProvider>().ToSingleton<CameraRayProvider>();
            Bind<IRayTracer>().To<MultiThreadedRayTracer>();
            Bind<ILightingStrategy>().To<FlatShading>();
            Bind<ILightProvider>().To<LightProvider>();
        }
    }
}