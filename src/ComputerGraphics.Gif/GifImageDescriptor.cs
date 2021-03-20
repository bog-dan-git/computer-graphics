namespace ComputerGraphics.Gif
{
    public struct GifImageDescriptor
    {
        public int LeftCornerX { get; set; }
        public int LeftCornerY { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public bool LocalColorTableIsPresent { get; set; }
        public bool Interlaced { get; set; }
        public bool LocalColorTableIsSorted { get; set; }
        public int ReservedBits { get; set; }
        public int ColorResolution { get; set; }
    }
}