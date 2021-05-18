using System.IO;
using System.Threading.Tasks;
using ComputerGraphics.RayTracing.Core.Entities;
using ComputerGraphics.RayTracing.Core.Interfaces;
using ComputerGraphics.SceneLoader.Mapping;

namespace ComputerGraphics.SceneLoader
{
    internal class SceneLoader : ISceneLoader
    {
        private readonly IMapper<Scene, SceneFormat.Scene> _sceneMapper;

        public SceneLoader(IMapper<Scene, SceneFormat.Scene> sceneMapper) => _sceneMapper = sceneMapper;

        public async Task<Scene> LoadFromJsonAsync(string filename)
        {
            var fileContent = await File.ReadAllTextAsync(filename);
            var scene = SceneFormat.Scene.Parser.ParseJson(filename);
            var mappedScene = _sceneMapper.Map(scene);
            return mappedScene;
        }

        public async Task<Scene> LoadFromProtoAsync(string filename)
        {
            var bytes = await File.ReadAllBytesAsync(filename);
            var scene = SceneFormat.Scene.Parser.ParseFrom(bytes);
            var mappedScene = _sceneMapper.Map(scene);
            return mappedScene;
        }
    }
}