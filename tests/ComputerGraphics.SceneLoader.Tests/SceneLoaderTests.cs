using System.Linq;
using ComputerGraphics.Ioc.Framework;
using SceneFormat;
using Xunit;
using ComputerGraphics.SceneLoader.Mapping;
using Disk = ComputerGraphics.RayTracing.Entities.Entities.Disk;
using LambertReflectionMaterial = ComputerGraphics.RayTracing.Core.Entities.Materials.LambertReflectionMaterial;
using PerspectiveCamera = ComputerGraphics.RayTracing.Core.Entities.Cameras.PerspectiveCamera;

namespace ComputerGraphics.SceneLoader.Tests
{
    public class SceneLoaderTests
    {
        private readonly CustomServiceCollection _serviceCollection =
            new Container(new[] {typeof(ComputerGraphics.SceneLoader.DependencyInjection).Assembly})
                .Build();

        [Fact]
        public void SimpleJsonFile_ShouldLoad_Successfully()
        {
            var json =
                "{ \"version\": 1, \"sceneObjects\": [ { \"transform\": { \"position\": { \"x\": 1, \"y\": 1, \"z\": 1 } }, \"material\": { \"lambertReflection\": { } }, \"disk\": { } } ], \"cameras\": [ { \"transform\": { \"position\": { \"x\": 1.01, \"y\": 2.76, \"z\": 3 } }, \"perspective\": { \"fov\": 60 } } ] }";
            var mapper = _serviceCollection.GetService<IMapper<RayTracing.Core.Entities.Scene, Scene>>();
            var parsedScene = Scene.Parser.ParseJson(json);
            var mappedScene = mapper.Map(parsedScene);
            Assert.Single(mappedScene.SceneObjects);
            Assert.IsType<Disk>(mappedScene.SceneObjects.First());
            Assert.IsType<LambertReflectionMaterial>(mappedScene.SceneObjects.First().Material);
            Assert.Single(mappedScene.Cameras);
            var camera = mappedScene.Cameras.First();
            Assert.IsType<PerspectiveCamera>(camera);
            Assert.Equal(60f, ((PerspectiveCamera) camera).HFov);
        }
    }
}