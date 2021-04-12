using System;
using System.Collections.Generic;
using System.Numerics;
using ComputerGraphics.ObjLoader.Models;

namespace ComputerGraphics.ObjLoader
{
    public class KDTreeNode
    {
        //  ////////////////////
        public static int trianglesChecked = 0;
        public static int maxDepthInTree = 0;
        //  //////////////////

        private const int SectionsNumber = 33;
        private const float Ct = 1f;
        private const float Ci = 1.3f;
        private const int MaxDepth = 35;
        private const int SplitsNumber = SectionsNumber - 1;

        public int Depth { get; set; }
        public Triangle[] Triangles { get; set; }
        public KDTreeNode LeftNode { get; set; }
        public KDTreeNode RightNode { get; set; }
        public Vector3 BoxMax { get; }
        public Vector3 BoxMin { get; }
        public BoxSplit Split { get; set; }
        public int SplitAxis { get; set; }
        
        private bool _splitted;

        private int[] _xHigh = new int[SectionsNumber];    
        private int[] _xLow = new int[SectionsNumber];        
        private int[] _yHigh = new int[SectionsNumber];
        private int[] _yLow = new int[SectionsNumber];            
        private int[] _zHigh = new int[SectionsNumber];    
        private int[] _zLow = new int[SectionsNumber];
        
        public KDTreeNode(Triangle[] triangles,  Vector3 boxMin, Vector3 boxMax, int depth)
        {
            Triangles = triangles;
            BoxMax = boxMax;
            BoxMin = boxMin;
            Depth = depth;
            if (Depth > maxDepthInTree)
            {
                maxDepthInTree++;
            }
        }
        
        public void SplitNode()
        {
            if (_splitted) return;
            _splitted = true;

            if (Triangles == null || Triangles.Length == 0) return;

            CalculateElementsAmountInPossibleSubsections();
            
            var lowestSah = float.MaxValue;
            var boxSplits = CalculateSplits();
            var parentSquare = CalculateSquare(BoxMin, BoxMax);

            var bestBoxSplitIndex = 0;
            var axis = 0;
            int nLeft = 0, nRight = 0;
            for (var i = 0; i < SplitsNumber; i++)
            {
                var xRightN = Triangles.Length - _xHigh[i];
                var xLeftN = Triangles.Length - _xLow[i + 1];
                var yRightN = Triangles.Length - _yHigh[i];
                var yLeftN = Triangles.Length - _yLow[i + 1];
                var zRightN = Triangles.Length - _zHigh[i];
                var zLeftN = Triangles.Length - _zLow[i + 1];

                var xLeftSquare = CalculateSquare(BoxMin, boxSplits[i].LBoxMax);
                var xRightSquare = CalculateSquare(boxSplits[i].RBoxMin, BoxMax);                
                var yLeftSquare = CalculateSquare(BoxMin, boxSplits[i + SplitsNumber].LBoxMax);
                var yRightSquare = CalculateSquare(boxSplits[i + SplitsNumber].RBoxMin, BoxMax);
                var zLeftSquare = CalculateSquare(BoxMin, boxSplits[i + SplitsNumber * 2].LBoxMax);
                var zRightSquare = CalculateSquare(boxSplits[i + SplitsNumber * 2].RBoxMin, BoxMax);

                var tmpSah = CalculateSah(xLeftSquare, xLeftN, xRightSquare, xRightN, parentSquare);
                if (tmpSah < lowestSah)
                {
                    bestBoxSplitIndex = i;
                    lowestSah = tmpSah;
                    axis = 0;
                    nLeft = xLeftN;
                    nRight = xRightN;
                }

                tmpSah = CalculateSah(yLeftSquare, yLeftN, yRightSquare, yRightN, parentSquare);
                if (tmpSah < lowestSah)
                {
                    bestBoxSplitIndex = i + SplitsNumber;
                    lowestSah = tmpSah;
                    axis = 1;
                    nLeft = yLeftN;
                    nRight = yRightN;
                }

                tmpSah = CalculateSah(zLeftSquare, zLeftN, zRightSquare, zRightN, parentSquare);
                if (tmpSah < lowestSah)
                {
                    bestBoxSplitIndex = i + SplitsNumber * 2;
                    lowestSah = tmpSah;
                    axis = 2;
                    nLeft = zLeftN;
                    nRight = zRightN;
                }
                
            }

            if (Ci * lowestSah > Triangles.Length  || Depth > MaxDepth)       // dangerous point
            {
                return;
            }

            Split = boxSplits[bestBoxSplitIndex];
            SplitAxis = axis;
            
            var splitTriangles = SplitTriangles(Split, axis, nLeft, nRight);
            
            LeftNode = new KDTreeNode(splitTriangles[0], BoxMin, Split.LBoxMax, Depth + 1);
            LeftNode.SplitNode();
            
            RightNode = new KDTreeNode(splitTriangles[1], Split.RBoxMin, BoxMax, Depth + 1);
            RightNode.SplitNode();

            Triangles = null;
        }

        public HitResult? Traverse(Ray ray)
        {
            var intersectionResult = IntersectBox(ray);
            if (!intersectionResult.Intersected)
            {
                return null;
            }

            if (intersectionResult.TMin <= 0)
            {
                intersectionResult.TMin = 0;
            }

            if (intersectionResult.TMin >= intersectionResult.TMax)  //potentially dangerous point
            {
                return null;
            }

            var leftOrRight = new bool[3];
            leftOrRight[0] = !(ray.Direction.X >= 0);
            leftOrRight[1] = !(ray.Direction.Y >= 0);
            leftOrRight[2] = !(ray.Direction.Z >= 0);

            if (IsLeaf())
            {
                return Triangles == null ? null : IntersectAllPrimitivesInLeaf(ray);
            }
            
            var invRayDir = Vector3.One / ray.Direction;

            var tSplit = (Split.SplitValue - AxisVal(ray.Origin, SplitAxis)) * AxisVal(invRayDir, SplitAxis); // potentially dangerous point

            var nearNode = leftOrRight[SplitAxis] == false ? LeftNode : RightNode;
            var farNode = leftOrRight[SplitAxis] == false ? RightNode : LeftNode;
            
            if (tSplit <= intersectionResult.TMin)
            {
                return farNode.Traverse(ray);
            }
            
            if (tSplit >= intersectionResult.TMax)
            {
                return nearNode.Traverse(ray);
            }

            var hit = nearNode.Traverse(ray);
            if (hit.HasValue)
            {
                return hit.Value;
            }

            hit = farNode.Traverse(ray);
            return hit;
        }

        private HitResult? IntersectAllPrimitivesInLeaf(Ray ray)
        {
            HitResult? nearestHit = null;
            if (Triangles == null) return null;
            
            foreach (var triangle in Triangles)
            {
                var hitResult = Hit(ray, triangle);
                if (!nearestHit.HasValue)
                {
                    nearestHit = hitResult;
                    continue;
                }
                
                if (hitResult.HasValue && hitResult.Value.T < nearestHit.Value.T)
                {
                    nearestHit = hitResult.Value;
                }
            }

            return nearestHit;
        }
        
        private static HitResult? Hit(Ray r, Triangle triangle)
        {
            trianglesChecked++;
            var e1 = triangle.B - triangle.A;
            var e2 = triangle.C - triangle.A;
            var pvec = Vector3.Cross(r.Direction, e2);
            var det = Vector3.Dot(e1, pvec);
            if (det < 1e-8 && det > -1e-8)
            {
                return null;
            }

            var invDet = 1 / det;
            var tvec = r.Origin - triangle.A;
            var u = Vector3.Dot(tvec, pvec) * invDet;
            if (u < 0 || u > 1)
            {
                return null;
            }

            var qvec = Vector3.Cross(tvec, e1);
            var v = Vector3.Dot(r.Direction, qvec) * invDet;
            if (v < 0 || u + v > 1)
            {
                return null;
            }

            var f = Vector3.Dot(e2, qvec) * invDet;
            return new HitResult()
            {
                P = r.PointAt(f),
                Normal = Vector3.Cross(e2, e1),
                T = f
            };
        }
        
        private static float AxisVal(Vector3 vector, int axis) =>
            axis switch
            {
                0 => vector.X,
                1 => vector.Y,
                2 => vector.Z,
                _ => throw new Exception()
            };

        private RayBoxIntersection IntersectBox(Ray ray)
        {
            var invRayDir = Vector3.One / ray.Direction;
            
            var lo = invRayDir.X *(BoxMin.X - ray.Origin.X);
            var hi = invRayDir.X * (BoxMax.X - ray.Origin.X);
            
            var tMin  = Math.Min(lo, hi);
            var tMax = Math.Max(lo, hi);

            var lo1 = invRayDir.Y *(BoxMin.Y - ray.Origin.Y);
            var hi1 = invRayDir.Y * (BoxMax.Y - ray.Origin.Y);

            tMin = Math.Max(tMin, Math.Min(lo1, hi1));
            tMax = Math.Min(tMax, Math.Max(lo1, hi1));
            
            var lo2 = invRayDir.Z *(BoxMin.Z - ray.Origin.Z);
            var hi2 = invRayDir.Z * (BoxMax.Z - ray.Origin.Z);

            tMin = Math.Max(tMin, Math.Min(lo2, hi2));
            tMax = Math.Min(tMax, Math.Max(lo2, hi2));

            var result = new RayBoxIntersection
            {
                Intersected = (tMin <= tMax) && (tMax > 0f),
                TMin = tMin,
                TMax = tMax
            };

            return result;
        }
        
        private Triangle[][] SplitTriangles(BoxSplit split, int axis, int leftN, int rightN)
        {
            var result = new List<Triangle>[2];
            result[0] = new List<Triangle>();
            result[1] = new List<Triangle>();

            var splitPoint = AxisVal(split.LBoxMax, axis);
            
            foreach (var triangle in Triangles)
            {
                float tBoxMin;
                float tBoxMax;

                switch (axis)
                {
                    case 0:
                        tBoxMin = triangle.BoxMin.X;
                        tBoxMax = triangle.BoxMax.X;
                        break;
                    case 1:
                        tBoxMin = triangle.BoxMin.Y;
                        tBoxMax = triangle.BoxMax.Y;
                        break;
                    case 2:
                        tBoxMin = triangle.BoxMin.Z;
                        tBoxMax = triangle.BoxMax.Z;
                        break;
                    default:
                        throw new Exception();
                }
                
                if (tBoxMin <= splitPoint)       // dangerous point
                {
                    result[0].Add(triangle);
                }

                if (tBoxMax >= splitPoint)       // dangerous point
                {
                    result[1].Add(triangle);
                }
            }
            
            var resultArray =  new Triangle[2][];
            resultArray[0] = (result[0].Count > 0) ? result[0].ToArray() : null;
            resultArray[1] = (result[1].Count > 0) ? result[1].ToArray() : null;
            
            return resultArray;
        }
        
        private BoxSplit[] CalculateSplits()
        {
            var xBoxLength = BoxMax.X - BoxMin.X;
            var yBoxLength = BoxMax.Y - BoxMin.Y;
            var zBoxLength = BoxMax.Z - BoxMin.Z;
            
            var xSectionLength = xBoxLength / SectionsNumber;
            var ySectionLength = yBoxLength / SectionsNumber;
            var zSectionLength = zBoxLength / SectionsNumber;

            const int splitsNumber = SectionsNumber - 1;
            var boxSplits = new BoxSplit[splitsNumber * 3];

            for (var i = 0; i < splitsNumber; i++)
            {
                var xSplit = (i + 1) * xSectionLength + BoxMin.X;
                var ySplit = (i + 1) * ySectionLength + BoxMin.Y;
                var zSplit = (i + 1) * zSectionLength + BoxMin.Z;

                var xBoxSplit = new BoxSplit
                {
                    RBoxMin = new Vector3(xSplit, BoxMin.Y, BoxMin.Z),
                    LBoxMax = new Vector3(xSplit, BoxMax.Y, BoxMax.Z),
                    SplitValue = xSplit
                };
                boxSplits[i] = xBoxSplit;

                var yBoxSplit = new BoxSplit
                {
                    RBoxMin = new Vector3(BoxMin.X, ySplit, BoxMin.Z),
                    LBoxMax = new Vector3(BoxMax.X, ySplit, BoxMax.Z),
                    SplitValue = ySplit
                };
                boxSplits[i + splitsNumber] = yBoxSplit;
                
                var zBoxSplit = new BoxSplit
                {
                    RBoxMin = new Vector3(BoxMin.X, BoxMin.Y, zSplit),
                    LBoxMax = new Vector3(BoxMax.X, BoxMax.Y, zSplit),
                    SplitValue = zSplit
                };
                boxSplits[i + splitsNumber * 2] = zBoxSplit;
            }
            
            return boxSplits;
        }
        
        private void CalculateElementsAmountInPossibleSubsections()
        {
            var xSectionLength = (BoxMax.X - BoxMin.X) / SectionsNumber;
            var ySectionLength = (BoxMax.Y - BoxMin.Y) / SectionsNumber;
            var zSectionLength = (BoxMax.Z - BoxMin.Z) / SectionsNumber;
            
            foreach (var triangle in Triangles)
            {
                var xLowIndex = (int) Math.Floor((triangle.BoxMin.X - BoxMin.X) / xSectionLength);
                var xHighIndex = (int) Math.Floor((triangle.BoxMax.X - BoxMin.X) / xSectionLength);
               
                var yLowIndex = (int) Math.Floor((triangle.BoxMin.Y - BoxMin.Y) / ySectionLength);
                var yHighIndex = (int) Math.Floor((triangle.BoxMax.Y - BoxMin.Y) / ySectionLength);
                
                var zLowIndex = (int) Math.Floor((triangle.BoxMin.Z - BoxMin.Z) / zSectionLength);
                var zHighIndex = (int) Math.Floor((triangle.BoxMax.Z - BoxMin.Z) / zSectionLength);

                if (xHighIndex > SectionsNumber - 1) xHighIndex = SectionsNumber - 1;
                if (yHighIndex > SectionsNumber - 1) yHighIndex = SectionsNumber - 1;
                if (zHighIndex > SectionsNumber - 1) zHighIndex = SectionsNumber - 1;
                if (xLowIndex > SectionsNumber - 1) xLowIndex = SectionsNumber - 1;
                if (yLowIndex > SectionsNumber - 1) yLowIndex = SectionsNumber - 1;
                if (zLowIndex > SectionsNumber - 1) zLowIndex = SectionsNumber - 1;

                if (xHighIndex < 0) xHighIndex = 0;
                if (yHighIndex < 0) yHighIndex = 0;
                if (zHighIndex < 0) zHighIndex = 0;
                if (xLowIndex < 0) xLowIndex = 0;
                if (yLowIndex < 0) yLowIndex = 0;
                if (zLowIndex < 0) zLowIndex = 0;

                
                _xHigh[xHighIndex]++;
                _xLow[xLowIndex]++;                
                
                _yHigh[yHighIndex]++;
                _yLow[yLowIndex]++;                
                
                _zHigh[zHighIndex]++;
                _zLow[zLowIndex]++;
                
            }

            var xHighCounter = 0;
            var xLowCounter = 0;            
            
            var yHighCounter = 0;
            var yLowCounter = 0;            
            
            var zHighCounter = 0;
            var zLowCounter = 0;
            
            for (var i = 0; i < SectionsNumber; i++)
            {
                xHighCounter += _xHigh[i];
                _xHigh[i] = xHighCounter;
                xLowCounter += _xLow[SectionsNumber - i - 1];
                _xLow[SectionsNumber - i - 1] = xLowCounter;
                
                yHighCounter += _yHigh[i];
                _yHigh[i] = yHighCounter;
                yLowCounter += _yLow[SectionsNumber - i - 1];
                _yLow[SectionsNumber - i - 1] = yLowCounter;
                
                zHighCounter += _zHigh[i];
                _zHigh[i] = zHighCounter;
                zLowCounter += _zLow[SectionsNumber - i - 1];
                _zLow[SectionsNumber - i - 1] = zLowCounter;
            }
        }

        private static float CalculateSah(float sLeft, float nLeft, float sRight, float nRight, float sParent) => Ct + Ci * (sLeft * nLeft + sRight * nRight) / sParent;

        private static float CalculateSquare(Vector3 min, Vector3 max) => (max.X - min.X) * (max.Y - min.Y) * (max.Z - min.Z);

        private bool IsLeaf() => (LeftNode == null) || (RightNode == null);
    }
}