using System.Collections.Generic;
using System.Linq;
using ComputerGraphics.RayTracing.Core.Entities.Cameras;
using ComputerGraphics.RayTracing.Core.Entities.Lights;
using ComputerGraphics.RayTracing.Core.Entities.Materials;
using ComputerGraphics.RayTracing.Core.Entities.SceneObjects;
using ComputerGraphics.RayTracing.Core.Interfaces;

namespace ComputerGraphics.RayTracing.Core.Entities
{
    public class Scene
    {
        public IEnumerable<SceneObject> SceneObjects { get; set; }
        
        public IEnumerable<Camera> Cameras { get; set; }
        
        public IEnumerable<Light> Lights { get; set; }
        public IEnumerable<Material> Materials { get; set; }
        
        public RenderOptions RenderOptions { get; set; }

        public Scene(IEnumerable<SceneObject> hittables) => SceneObjects = hittables;

        public Scene()
        {
        }
    }
}