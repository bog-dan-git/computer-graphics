using System.Numerics;
using ComputerGraphics.ObjLoader.Models;

namespace ComputerGraphics.ObjLoader
{
    public class KDTree
    {
        public KDTreeNode HeadNode { get; set; }
        
        public KDTree(Triangle[] triangles)
        {
            var minX = float.MaxValue;
            var minY = float.MaxValue;
            var minZ = float.MaxValue;
                    
            var maxX = float.MinValue;
            var maxY = float.MinValue;
            var maxZ = float.MinValue;
                    
            foreach (var triangle in triangles)
            {
                if (minX > triangle.BoxMin.X) minX = triangle.BoxMin.X; 
                if (minY > triangle.BoxMin.Y) minY = triangle.BoxMin.Y;
                if (minZ > triangle.BoxMin.Z) minZ = triangle.BoxMin.Z;
                        
                if (maxX < triangle.BoxMax.X) maxX = triangle.BoxMax.X; 
                if (maxY < triangle.BoxMax.Y) maxY = triangle.BoxMax.Y;
                if (maxZ < triangle.BoxMax.Z) maxZ = triangle.BoxMax.Z;
            }
            
            
            var boxMax = new Vector3(maxX, maxY, maxZ);
            var boxMin = new Vector3(minX, minY, minZ);

            HeadNode = new KDTreeNode(triangles, boxMin, boxMax, 1);
            HeadNode.SplitNode();
        }

        public HitResult? Traverse(Ray ray)
        {
            return HeadNode.Traverse(ray);
        }
    }
}