using System.Collections.Generic;

namespace ComputerGraphics.ObjLoader.Models
{
    public class Object3D
    {
        public List<Triangle> Faces { get; set; }
        public List<Triangle> Normals { get; set; }
        public List<Triangle> Textures { get; set; }
        
    }
}