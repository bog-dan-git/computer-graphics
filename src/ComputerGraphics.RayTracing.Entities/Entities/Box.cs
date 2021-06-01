using System;
using System.Collections.Generic;
using System.Numerics;
using ComputerGraphics.Common;
using ComputerGraphics.RayTracing.Core.Entities;
using ComputerGraphics.RayTracing.Core.Entities.SceneObjects;

namespace ComputerGraphics.RayTracing.Entities.Entities
{
    public class Box : SceneObject
    {
        public Vector3 Size { get; set; }
        
        private const float Epsilon = 1e-6f;
        private Matrix4x4? _transformationMatrix;
        private List<Vector3> _boxPlanesCenters;
        private Vector3? _boxCenter;


        public override HitResult? Hit(Ray r)
        {
            Transform ??= new Transform();

            var tMin = 0.0f;
            var tMax = 100000.0f;

            var transformationMatrix = GetTransformationMatrix();
            var boxMin = Vector3.Zero;
            
            var obbPositionWorldSpace = transformationMatrix.Translation;
            var delta = obbPositionWorldSpace - r.Origin;

            #region Test intersection with the 2 planes perpendicular to the OBB's X axis

            var xAxis = new Vector3(transformationMatrix.M11, transformationMatrix.M12, transformationMatrix.M13);
            var ex = Vector3.Dot(xAxis, delta);
            var fx = Vector3.Dot(r.Direction, xAxis);


            if (MathF.Abs(fx) > 0.00001f)
            {
                var t1 = (ex + boxMin.X) / fx; 
                var t2 = (ex + Size.X) / fx; 

                if (t1 > t2)
                {
                    var w = t1;
                    t1 = t2;
                    t2 = w; 
                }

                if (t2 < tMax)
                {
                    tMax = t2;
                }
                if (t1 > tMin)
                {
                    tMin = t1;
                }
                
                if (tMax < tMin)
                {
                    return null;
                }
            } else { 
                if(-ex + boxMin.X > 0.0f || -ex + Size.X < 0.0f)
                    return null;
            }
            #endregion

            #region Test intersection with the 2 planes perpendicular to the OBB's Y axis

            var yAxis = new Vector3(transformationMatrix.M21, transformationMatrix.M22, transformationMatrix.M23);
            var ey = Vector3.Dot(yAxis, delta);
            var fy = Vector3.Dot(r.Direction, yAxis);


            if (MathF.Abs(fy) > 0.00001f)
            {
                var t1 = (ey + boxMin.Y) / fy; 
                var t2 = (ey + Size.Y) / fy; 
                
                if (t1 > t2)
                {
                    var w = t1;
                    t1 = t2;
                    t2 = w; 
                }

                if (t2 < tMax)
                {
                    tMax = t2;
                }
                if (t1 > tMin)
                {
                    tMin = t1;
                }
                if (tMax < tMin)
                {
                    return null;
                }
            } else { 
                if(-ey + boxMin.Y > 0.0f || -ey + Size.Y < 0.0f)
                    return null;
            }
            #endregion
            
            #region Test intersection with the 2 planes perpendicular to the OBB's Z axis

            var zAxis = new Vector3(transformationMatrix.M31, transformationMatrix.M32, transformationMatrix.M33);
            var ez = Vector3.Dot(zAxis, delta);
            var fz = Vector3.Dot(r.Direction, zAxis);


            if (MathF.Abs(fz) > 0.00001f)
            {
                var t1 = (ez + boxMin.Z) / fz; 
                var t2 = (ez + Size.Z) / fz; 

                if (t1 > t2)
                {
                    var w = t1;
                    t1 = t2;
                    t2 = w; 
                }

                if (t2 < tMax)
                {
                    tMax = t2;
                }
                if (t1 > tMin)
                {
                    tMin = t1;
                }
                
                if (tMax < tMin)
                {
                    return null;
                }
            } else { 
                if(-ez + boxMin.Z > 0.0f || -ez + Size.Z < 0.0f)
                    return null;
            }
            #endregion
            
            var intersectionDistance = tMin;            
            var point = r.Direction * intersectionDistance + r.Origin;
            var normal = GetNormal(point, r);
            return new HitResult(){T = intersectionDistance, P = point, Normal = normal, Material = Material};
        }

        private Vector3 GetNormal(Vector3 point, Ray ray)
        {
            var boxPlanesCenters = GetBoxPlanesCenters();
            
            _boxCenter ??= Vector3.Transform(new Vector3(Size.X / 2, Size.Y / 2, Size.Z / 2), GetTransformationMatrix());
            
            var normal = Vector3.Zero;
            var closestPlane = float.MaxValue;
            foreach (var boxPlaneCenter in boxPlanesCenters)
            {
                var dot = Vector3.Dot(boxPlaneCenter - _boxCenter.Value, point - boxPlaneCenter);
                var dotAbs = MathF.Abs(dot);
                var distToPlane = (boxPlaneCenter - ray.Origin).Length();
                
                if (dotAbs < Epsilon && closestPlane > distToPlane)
                {
                    closestPlane = distToPlane;
                    normal = Vector3.Normalize(boxPlaneCenter - _boxCenter.Value);
                }
            }

            return normal;
        }

        private IEnumerable<Vector3> GetBoxPlanesCenters()
        {
            if (_boxPlanesCenters == null)
            {
                _boxPlanesCenters = new List<Vector3>
                {
                    Vector3.Transform(new Vector3(Size.X / 2, Size.Y / 2, 0f), GetTransformationMatrix()),
                    Vector3.Transform(new Vector3(Size.X / 2, Size.Y / 2, Size.Z), GetTransformationMatrix()),
                    Vector3.Transform(new Vector3(0, Size.Y / 2, Size.Z / 2), GetTransformationMatrix()),
                    Vector3.Transform(new Vector3(Size.X, Size.Y / 2, Size.Z / 2), GetTransformationMatrix()),
                    Vector3.Transform(new Vector3(Size.X / 2, 0, Size.Z / 2), GetTransformationMatrix()),
                    Vector3.Transform(new Vector3(Size.X / 2, Size.Y, Size.Z / 2), GetTransformationMatrix())
                };
            }
            
            return _boxPlanesCenters;
        }

        private Matrix4x4 GetTransformationMatrix()
        {
            _transformationMatrix ??= new TransposedTransformationMatrixBuilder()
                .Rotate(Transform.Rotation)
                .Move(Transform.Position)
                .Build();

            return _transformationMatrix.Value;
        }
    }
}