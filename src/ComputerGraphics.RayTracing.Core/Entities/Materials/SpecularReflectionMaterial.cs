namespace ComputerGraphics.RayTracing.Core.Entities.Materials
{
    public class SpecularReflectionMaterial : LambertReflectionMaterial
    {
        public float Eta { get; set; }
    }
}