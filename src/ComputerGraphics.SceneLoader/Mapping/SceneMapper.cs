using System.Linq;
using ComputerGraphics.RayTracing.Core.Entities;
using ComputerGraphics.RayTracing.Core.Entities.Cameras;
using ComputerGraphics.RayTracing.Core.Entities.Lights;
using ComputerGraphics.RayTracing.Core.Entities.Materials;
using ComputerGraphics.RayTracing.Core.Entities.SceneObjects;

namespace ComputerGraphics.SceneLoader.Mapping
{
    internal class SceneMapper : IMapper<Scene, SceneFormat.Scene>
    {
        private readonly IMapper<Camera, SceneFormat.Camera> _cameraMapper;
        private readonly IMapper<Transform, SceneFormat.Transform> _transformMapper;
        private readonly IMapper<Light, SceneFormat.Light> _lightMapper;
        private readonly IMapper<SceneObject, SceneFormat.SceneObject> _sceneObjectMapper;
        private readonly IMapper<Material, SceneFormat.Material> _materialMapper;

        public SceneMapper(IMapper<Camera, SceneFormat.Camera> cameraMapper,
            IMapper<Transform, SceneFormat.Transform> transformMapper, IMapper<Light, SceneFormat.Light> lightMapper,
            IMapper<SceneObject, SceneFormat.SceneObject> sceneObjectMapper,
            IMapper<Material, SceneFormat.Material> materialMapper)
        {
            _cameraMapper = cameraMapper;
            _transformMapper = transformMapper;
            _lightMapper = lightMapper;
            _sceneObjectMapper = sceneObjectMapper;
            _materialMapper = materialMapper;
        }

        public Scene Map(SceneFormat.Scene scene)
        {
            var mappedScene = new Scene
            {
                Cameras = scene.Cameras.Select(_cameraMapper.Map).ToArray(),
                Lights = scene.Lights.Select(_lightMapper.Map).ToArray(),

                RenderOptions = scene.RenderOptions is
                {
                }
                    ? new RenderOptions()
                    {
                        Height = scene.RenderOptions.Height,
                        Width = scene.RenderOptions.Width,
                        CameraId = scene.RenderOptions.CameraId
                    }
                    : RenderOptions.Default,
            };
            mappedScene.SceneObjects = scene.SceneObjects.Select(input =>
            {
                var mapped = _sceneObjectMapper.Map(input);
                if (input.ObjectMaterialCase == SceneFormat.SceneObject.ObjectMaterialOneofCase.MaterialId)
                {
                    mapped.Material = mappedScene.Materials.First(material => material.Id == input.MaterialId);
                }

                return mapped;
            });
            return mappedScene;
        }
    }
}