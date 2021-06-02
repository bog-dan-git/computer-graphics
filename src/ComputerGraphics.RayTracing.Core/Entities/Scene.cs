using System.Collections.Generic;
using System.Numerics;
using ComputerGraphics.RayTracing.Core.Entities.Cameras;
using ComputerGraphics.RayTracing.Core.Entities.Lights;
using ComputerGraphics.RayTracing.Core.Entities.Materials;
using ComputerGraphics.RayTracing.Core.Entities.SceneObjects;

namespace ComputerGraphics.RayTracing.Core.Entities
{
    public class Scene
    {
        public IEnumerable<SceneObject> SceneObjects { get; set; }
        
        public IEnumerable<Camera> Cameras { get; set; }
        
        public IEnumerable<Light> Lights { get; set; }
        public IEnumerable<Material> Materials { get; set; }
        
        public RenderOptions RenderOptions { get; set; }
        
        public Vector3 BackgroundColor { get; set; }

        public Scene(IEnumerable<SceneObject> hittables) => SceneObjects = hittables;

        public Scene()
        {
        }
    }
}