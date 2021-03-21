using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ComputerGraphics.Converters.Sdk;
using ComputerGraphics.Converters.Sdk.Interfaces;
using ComputerGraphics.Converters.Sdk.Model;
using ComputerGraphics.Gif.Exceptions;

namespace ComputerGraphics.Gif
{
    [ImageDecoder("gif")]
    public class GifDecoder : IImageDecoder
    {
        private byte[] _bytesArray;
        private int _offset;
        private GifHeader _gifHeader;
        private GifImageDescriptor _imageDescriptor;
        private Dictionary<int, RgbColor> _globalColorTable;

        public RgbColor[,] Read(byte[] bytes)
        {
            _bytesArray = bytes;
            _offset = 0;
            _gifHeader = ReadHeader();

            if (_gifHeader.GlobalColorTable)
            {
                _globalColorTable = ReadColorTable(_gifHeader.ColorTableSize);
            }

            SkipToByte(0x2C);
            _imageDescriptor = ReadImageDescriptor();

            OutputFileDescription();

            if (_imageDescriptor.LocalColorTableIsPresent)
            {
                throw new AnimationUnsupportedException();
            }

            if (_imageDescriptor.Interlaced)
            {
                throw new InterlacedUnsupportedException();
            }

            var minCodeLength = ReadBytes(1)[0] + 1;
            var sectionLength = ReadBytes(1)[0];
            var compressedData = new List<byte>();

            while (sectionLength != 0)
            {
                compressedData.AddRange(ReadBytes(sectionLength));
                sectionLength = ReadBytes(1)[0];
            }

            var decompressor = new GifLzvDecompressor();
            var colorIndexList = decompressor.Decompress(compressedData, minCodeLength, _globalColorTable);

            var resultArray = FormPixelTable(colorIndexList);

            return TransposeAndTransformTo2D(resultArray);
        }

        private RgbColor[,] TransposeAndTransformTo2D(RgbColor[][] resultArray)
        {
            var result = new RgbColor[resultArray[0].Length, resultArray.Length];
            for (int i = 0; i < resultArray[0].Length; i++)
            {
                for (int j = 0; j < resultArray.Length; j++)
                {
                    result[i, j] = resultArray[j][i];
                }
            }

            return result;
        }

        private RgbColor[][] FormPixelTable(List<int> colorIndexList)
        {
            var pixelList = colorIndexList.Select(colorIndex => _globalColorTable[colorIndex]).ToList();
            var pixelTable = new RgbColor[_imageDescriptor.Height][];
            for (var i = 0; i < pixelTable.Length; i++)
            {
                pixelTable[i] = pixelList.GetRange(i * _imageDescriptor.Width, _imageDescriptor.Width).ToArray();
            }

            return pixelTable;
        }

        private byte[] ReadBytes(int size)
        {
            var result = _bytesArray.Skip(_offset).Take(size).ToArray();
            _offset += size;

            return result;
        }

        private GifImageDescriptor ReadImageDescriptor()
        {
            var descriptor = new GifImageDescriptor();

            var leftCornerXBytes = ReadBytes(2);
            var leftCornerYBytes = ReadBytes(2);
            descriptor.LeftCornerX = (leftCornerXBytes[1] << 8) | leftCornerXBytes[0];
            descriptor.LeftCornerY = (leftCornerYBytes[1] << 8) | leftCornerYBytes[0];

            var widthBytes = ReadBytes(2);
            var heightBytes = ReadBytes(2);
            descriptor.Width = (widthBytes[1] << 8) | widthBytes[0];
            descriptor.Height = (heightBytes[1] << 8) | heightBytes[0];

            var infoByte = ReadBytes(1)[0];
            descriptor.LocalColorTableIsPresent = (infoByte >> 7 == 1);
            descriptor.Interlaced = (infoByte & 0b01000000) >> 6 == 1;
            descriptor.LocalColorTableIsSorted = (infoByte & 0b00100000) >> 5 == 1;
            descriptor.ReservedBits = (infoByte & 0b00011000) >> 3;
            descriptor.ColorResolution = infoByte & 0b00000111;

            return descriptor;
        }

        private GifHeader ReadHeader()
        {
            var header = new GifHeader();

            header.Signature = ReadBytes(3);
            header.Version = ReadBytes(3);

            if (System.Text.Encoding.UTF8.GetString(header.Signature) != "GIF" ||
                (System.Text.Encoding.UTF8.GetString(header.Version) != "87a" &&
                 System.Text.Encoding.UTF8.GetString(header.Version) != "89a"))
            {
                throw new InvalidFormatException();
            }

            var widthBytes = ReadBytes(2);
            var heightBytes = ReadBytes(2);
            header.ScreenWidth = (widthBytes[1] << 8) | widthBytes[0];
            header.ScreenHeight = (heightBytes[1] << 8) | heightBytes[0];

            var infoByte = ReadBytes(1)[0];
            header.GlobalColorTable = (infoByte >> 7 == 1);
            header.ColorResolution = Convert.ToInt32(Math.Pow(2, ((infoByte & 0b01110000) >> 4) + 1));
            header.ColorsSorted = (infoByte & 0b00001000) >> 2 == 1;
            header.ColorTableSize = 3 * Convert.ToInt32(Math.Pow(2, (infoByte & 0b00000111) + 1));
            header.BackgroundColorIndex = ReadBytes(1)[0];
            header.AspectRatio = ReadBytes(1)[0];

            return header;
        }

        private Dictionary<int, RgbColor> ReadColorTable(int size)
        {
            var colorTable = new Dictionary<int, RgbColor>();
            var colorTableArray = ReadBytes(size);

            var colorsNumber = size / 3;

            for (var i = 0; i < colorsNumber; i++)
            {
                colorTable[i] = new RgbColor
                {
                    R = colorTableArray[i * 3], G = colorTableArray[i * 3 + 1], B = colorTableArray[i * 3 + 2]
                };
            }

            return colorTable;
        }

        private void SkipToByte(byte value)
        {
            while (_bytesArray[_offset] != value)
            {
                _offset++;
            }

            _offset++;
        }

        private void OutputFileDescription()
        {
            Console.WriteLine("Version: " + System.Text.Encoding.UTF8.GetString(_gifHeader.Signature) +
                              System.Text.Encoding.UTF8.GetString(_gifHeader.Version));
            Console.WriteLine("Screen width: " + _gifHeader.ScreenWidth);
            Console.WriteLine("Screen height: " + _gifHeader.ScreenHeight);
            Console.WriteLine("Global color table present: " + _gifHeader.GlobalColorTable);
            Console.WriteLine("Color table size: " + _gifHeader.ColorTableSize);
            Console.WriteLine("Colors are sorted: " + _gifHeader.ColorsSorted);
        }

        private void OutputColorTable()
        {
            Console.WriteLine("------------------------------------");
            Console.WriteLine("Color table: ");
            foreach (var (key, color) in _globalColorTable)
            {
                Console.WriteLine(key + ": " + color.R + " " + color.G + " " + color.B);
            }
        }

        private void ToPpm(RgbColor[][] pixelTable, string path)
        {
            var sw = new StreamWriter(path);
            sw.WriteLine("P3");
            sw.WriteLine("# out.ppm");
            sw.WriteLine(_imageDescriptor.Width + " " + _imageDescriptor.Height);
            sw.WriteLine("255");

            foreach (var row in pixelTable)
            {
                foreach (var pixel in row)
                {
                    sw.Write(pixel.R + " " + pixel.G + " " + pixel.B + "  ");
                }

                sw.WriteLine();
            }

            sw.Close();
        }
    }
}