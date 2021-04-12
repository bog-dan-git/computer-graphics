using ComputerGraphics.ObjLoader.Models;

namespace ComputerGraphics.ObjLoader
{
    public interface IObjLoader
    {
        public Object3D Load(string filename);
    }
}