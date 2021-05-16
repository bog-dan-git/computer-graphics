using System.Threading.Tasks;
using ComputerGraphics.RayTracing.Core.Entities;

namespace ComputerGraphics.RayTracing.Core.Interfaces
{
    public interface ISceneLoader
    {
        public Task<Scene> LoadFromJsonAsync(string fileName);

        public Task<Scene> LoadFromProtoAsync(string fileName);
    }
}