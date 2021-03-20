namespace ComputerGraphics.Gif
{
    public struct GifHeader
    {
        public byte[] Signature { get; set; }
        public byte[] Version { get; set; }
        public int ScreenWidth { get; set; }
        public int ScreenHeight { get; set; }
        public bool GlobalColorTable { get; set; }
        public int ColorResolution { get; set; }
        public bool ColorsSorted { get; set; }
        public int ColorTableSize { get; set; }
        public byte BackgroundColorIndex { get; set; }
        public byte AspectRatio { get; set; }
    }
}