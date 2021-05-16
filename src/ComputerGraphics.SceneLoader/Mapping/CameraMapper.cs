using System;
using ComputerGraphics.RayTracing.Core.Entities;
using ComputerGraphics.RayTracing.Core.Entities.Cameras;

namespace ComputerGraphics.SceneLoader.Mapping
{
    internal class CameraMapper : IMapper<Camera, SceneFormat.Camera>
    {
        private readonly IMapper<Transform, SceneFormat.Transform> _transformMapper;

        public CameraMapper(IMapper<Transform, SceneFormat.Transform> transformMapper) =>
            _transformMapper = transformMapper;

        public Camera Map(SceneFormat.Camera camera)
        {
            Camera mappedCamera = camera.CameraCase switch
            {
                SceneFormat.Camera.CameraOneofCase.Orthographic => new OrthographicCamera(),
                SceneFormat.Camera.CameraOneofCase.Perspective => new PerspectiveCamera()
                {
                    Fov = (float) camera.Perspective.Fov
                },
                SceneFormat.Camera.CameraOneofCase.None => throw new NotSupportedException(),
                _ => throw new ArgumentOutOfRangeException(nameof(camera.CameraCase))
            };
            mappedCamera.Id = camera.Id;
            mappedCamera.Transform = _transformMapper.Map(camera.Transform);
            return mappedCamera;
        }
    }
}