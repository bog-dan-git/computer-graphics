namespace ComputerGraphics.RayTracing.Core.Entities
{
    public class RenderOptions
    {
        public static RenderOptions Default = new RenderOptions()
        {
            CameraId = 0,
            Height = 1280,
            Width = 720
        };
        public int Height { get; set; }
        public int Width { get; set; }
        public int CameraId { get; set; }
    }
}