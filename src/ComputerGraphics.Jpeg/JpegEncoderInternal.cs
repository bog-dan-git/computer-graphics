using System;
using System.Collections.Generic;
using System.Linq;
using ComputerGraphics.Converters.Sdk.Model;
using ComputerGraphics.Jpeg.Models;

namespace ComputerGraphics.Jpeg
{
    internal class JpegEncoderInternal
    {
        #region settings

        private const int Quality = 100;
        private const int AmountOfComponents = 3;

        #endregion

        #region constants

        private readonly byte[] _header = new byte[]
        {
            0xFF, 0xD8,
            0xFF, 0xE0,
            0, 16,
            (byte) 'J', (byte) 'F', (byte) 'I', (byte) 'F', 0,
            1, 1,
            0,
            0, 1, 0, 1,
            0, 0
        };

        // All of these were taken from here https://create.stephan-brumme.com/toojpeg/
        private readonly byte[] _defaultQuantLuminance =
        {
            16, 11, 10, 16, 24, 40, 51, 61,
            12, 12, 14, 19, 26, 58, 60, 55,
            14, 13, 16, 24, 40, 57, 69, 56,
            14, 17, 22, 29, 51, 87, 80, 62,
            18, 22, 37, 56, 68, 109, 103, 77,
            24, 35, 55, 64, 81, 104, 113, 92,
            49, 64, 78, 87, 103, 121, 120, 101,
            72, 92, 95, 98, 112, 100, 103, 99
        };

        private readonly byte[] _defaultQuantChrominance =
        {
            17, 18, 24, 47, 99, 99, 99, 99,
            18, 21, 26, 66, 99, 99, 99, 99,
            24, 26, 56, 99, 99, 99, 99, 99,
            47, 66, 99, 99, 99, 99, 99, 99,
            99, 99, 99, 99, 99, 99, 99, 99,
            99, 99, 99, 99, 99, 99, 99, 99,
            99, 99, 99, 99, 99, 99, 99, 99,
            99, 99, 99, 99, 99, 99, 99, 99
        };

        private readonly byte[] _zigZagInverse =
        {
            0, 1, 8, 16, 9, 2, 3, 10,
            17, 24, 32, 25, 18, 11, 4, 5,
            12, 19, 26, 33, 40, 48, 41, 34,
            27, 20, 13, 6, 7, 14, 21, 28,
            35, 42, 49, 56, 57, 50, 43, 36,
            29, 22, 15, 23, 30, 37, 44, 51,
            58, 59, 52, 45, 38, 31, 39, 46,
            53, 60, 61, 54, 47, 55, 62, 63
        };

        private readonly byte[] _dcLuminanceCodesPerBitSize = {0, 1, 5, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0};

        private readonly byte[] _dcLuminanceValues = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11};

        private readonly byte[] _acLuminanceCodesPerBitSize = {0, 2, 1, 3, 3, 2, 4, 3, 5, 5, 4, 4, 0, 0, 1, 125};

        private readonly byte[] _acLuminanceValues =
        {
            0x01, 0x02, 0x03, 0x00, 0x04, 0x11, 0x05, 0x12, 0x21, 0x31, 0x41, 0x06, 0x13, 0x51, 0x61, 0x07, 0x22, 0x71,
            0x14, 0x32, 0x81, 0x91, 0xA1, 0x08,
            0x23, 0x42, 0xB1, 0xC1, 0x15, 0x52, 0xD1, 0xF0, 0x24, 0x33, 0x62, 0x72, 0x82, 0x09, 0x0A, 0x16, 0x17, 0x18,
            0x19, 0x1A, 0x25, 0x26, 0x27, 0x28,
            0x29, 0x2A, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x53,
            0x54, 0x55, 0x56, 0x57, 0x58, 0x59,
            0x5A, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69, 0x6A, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78, 0x79, 0x7A, 0x83,
            0x84, 0x85, 0x86, 0x87, 0x88, 0x89,
            0x8A, 0x92, 0x93, 0x94, 0x95, 0x96, 0x97, 0x98, 0x99, 0x9A, 0xA2, 0xA3, 0xA4, 0xA5, 0xA6, 0xA7, 0xA8, 0xA9,
            0xAA, 0xB2, 0xB3, 0xB4, 0xB5, 0xB6,
            0xB7, 0xB8, 0xB9, 0xBA, 0xC2, 0xC3, 0xC4, 0xC5, 0xC6, 0xC7, 0xC8, 0xC9, 0xCA, 0xD2, 0xD3, 0xD4, 0xD5, 0xD6,
            0xD7, 0xD8, 0xD9, 0xDA, 0xE1, 0xE2,
            0xE3, 0xE4, 0xE5, 0xE6, 0xE7, 0xE8, 0xE9, 0xEA, 0xF1, 0xF2, 0xF3, 0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA
        };

        private readonly byte[] _dcChrominanceCodesPerBitSize = {0, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0};
        private readonly byte[] _dcChrominanceValues = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11};
        private readonly byte[] _acChrominanceCodesPerBitSize = {0, 2, 1, 2, 4, 4, 3, 4, 7, 5, 4, 4, 0, 1, 2, 119};

        private readonly byte[] _acChrominanceValues =
        {
            0x00, 0x01, 0x02, 0x03, 0x11, 0x04, 0x05, 0x21, 0x31, 0x06, 0x12, 0x41, 0x51, 0x07, 0x61, 0x71, 0x13, 0x22,
            0x32, 0x81, 0x08, 0x14, 0x42, 0x91,
            0xA1, 0xB1, 0xC1, 0x09, 0x23, 0x33, 0x52, 0xF0, 0x15, 0x62, 0x72, 0xD1, 0x0A, 0x16, 0x24, 0x34, 0xE1, 0x25,
            0xF1, 0x17, 0x18, 0x19, 0x1A, 0x26,
            0x27, 0x28, 0x29, 0x2A, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A,
            0x53, 0x54, 0x55, 0x56, 0x57, 0x58,
            0x59, 0x5A, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69, 0x6A, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78, 0x79, 0x7A,
            0x82, 0x83, 0x84, 0x85, 0x86, 0x87,
            0x88, 0x89, 0x8A, 0x92, 0x93, 0x94, 0x95, 0x96, 0x97, 0x98, 0x99, 0x9A, 0xA2, 0xA3, 0xA4, 0xA5, 0xA6, 0xA7,
            0xA8, 0xA9, 0xAA, 0xB2, 0xB3, 0xB4,
            0xB5, 0xB6, 0xB7, 0xB8, 0xB9, 0xBA, 0xC2, 0xC3, 0xC4, 0xC5, 0xC6, 0xC7, 0xC8, 0xC9, 0xCA, 0xD2, 0xD3, 0xD4,
            0xD5, 0xD6, 0xD7, 0xD8, 0xD9, 0xDA,
            0xE2, 0xE3, 0xE4, 0xE5, 0xE6, 0xE7, 0xE8, 0xE9, 0xEA, 0xF2, 0xF3, 0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA
        };


        private static readonly float[] AanScaleFactors =
            {1, 1.387039845f, 1.306562965f, 1.175875602f, 1, 0.785694958f, 0.541196100f, 0.275899379f};

        #endregion

        private readonly IDiscreteCosineTransformer _discreteCosineTransformer;

        public JpegEncoderInternal(IDiscreteCosineTransformer discreteCosineTransformer)
        {
            _discreteCosineTransformer = discreteCosineTransformer;
        }

        private void WriteBits(Action<byte> writeCallback, ref BitBuffer buffer, BitCode data)
        {
            buffer.AmountOfBits += data.AmountOfBits;
            buffer.Bits <<= data.AmountOfBits;
            buffer.Bits |= data.Code;

            while (buffer.AmountOfBits >= 8)
            {
                buffer.AmountOfBits -= 8;
                byte oneByte = (byte) ((buffer.Bits >> buffer.AmountOfBits));
                writeCallback(oneByte);
                if (oneByte == 0xFF)
                {
                    writeCallback(0);
                }
            }
        }

        private BitCode ConvertCode(short value)
        {
            short absolute = Math.Abs(value);
            var mask = 0;
            byte numBits = 0;
            while (absolute > mask)
            {
                numBits++;
                mask = 2 * mask + 1;
            }

            return new BitCode()
            {
                AmountOfBits = numBits,
                Code = value >= 0 ? (ushort) value : (ushort) (value + mask)
            };
        }

        private short EncodeBlock(Action<byte> output, ref BitBuffer buffer, float[,] block, float[] scaled,
            short lastDc, BitCode[] huffmanDc, BitCode[] huffmanTable)
        {
            var block64 = block.Cast<float>().ToArray().AsSpan();
            for (var offset = 0; offset < 8; offset++)
            {
                _discreteCosineTransformer.Transform(block64.Slice(offset * 8), 1);
            }

            for (var offset = 0; offset < 8; offset++)
            {
                _discreteCosineTransformer.Transform(block64.Slice(offset * 1), 8);
            }

            for (int i = 0; i < 64; i++)
            {
                block64[i] *= scaled[i];
            }

            var dc = (short) (Math.Round(block64[0]));
            if (dc == lastDc)
            {
                WriteBits(output, ref buffer, huffmanDc[0x00]);
            }
            else
            {
                var bits = ConvertCode((short) (dc - lastDc));
                WriteBits(output, ref buffer, huffmanDc[bits.AmountOfBits]);
                WriteBits(output, ref buffer, bits);
            }

            var blockArray = block64.ToArray();
            var quanitzed = blockArray
                .Select((_, index) => blockArray[_zigZagInverse[index]])
                .Select(_ => (short) Math.Round(_)).ToList();
            var posNonZero = quanitzed.FindIndex(_ => _ > 0);


            for (int i = 1; i <= posNonZero; i++)
            {
                var offset = 0;
                while (quanitzed[i] == 0)
                {
                    i++;
                    offset += 16;
                    if (offset > 15 << 4)
                    {
                        offset = 0;
                        WriteBits(output, ref buffer, huffmanTable[0xF0]);
                    }
                }

                var bits = ConvertCode(quanitzed[i]);
                offset += bits.AmountOfBits;
                WriteBits(output, ref buffer, huffmanTable[offset]);
                WriteBits(output, ref buffer, bits);
            }

            if (posNonZero < 8 * 8 - 1)
            {
                WriteBits(output, ref buffer, huffmanTable[0x00]);
            }

            return dc;
        }

        public void WriteJpeg(Action<byte> output, RgbColor[,] rgbs)
        {
            int width = rgbs.GetLength(0);
            int height = rgbs.GetLength(1);
            var bitWriter = new BitWriter(output);
            bitWriter.Write(_header);


            var quantLuminance = GetClampedZigZagValuesWithQuality(_defaultQuantLuminance);
            var quantChrominance = GetClampedZigZagValuesWithQuality(_defaultQuantChrominance);

            WriteQuantizedLumaAndChroma(bitWriter, quantLuminance, quantChrominance);

            WriteSize(bitWriter, height, width);

            for (byte id = 1; id <= 3; id++)
            {
                bitWriter.Write(id);
                bitWriter.Write(0x11);
                bitWriter.Write((byte) (id == 1 ? 0 : 1));
            }

            WriteLuminanceAndChrominance(bitWriter);

            var huffmanLuminanceDc = GenerateHuffmanTable(_dcLuminanceCodesPerBitSize, _dcLuminanceValues);
            var huffmanLuminanceAc = GenerateHuffmanTable(_acLuminanceCodesPerBitSize, _acLuminanceValues);


            var huffmanChrominanceDc = GenerateHuffmanTable(_dcChrominanceCodesPerBitSize, _dcChrominanceValues);
            var huffmanChrominanceAc = GenerateHuffmanTable(_acChrominanceCodesPerBitSize, _acChrominanceValues);

            bitWriter.AddMarker(0xDA, 2 + 1 + 2 * AmountOfComponents + 3);
            output(AmountOfComponents);
            for (byte id = 1; id <= AmountOfComponents; id++)
            {
                bitWriter.Write(id);

                bitWriter.Write((byte) (id == 1 ? 0x00 : 0x11));
            }

            bitWriter.Write(0);
            bitWriter.Write(63);
            bitWriter.Write(0);

            var scaledLuminance = CalculateScaled(quantLuminance);
            var scaledChrominance = CalculateScaled(quantChrominance);

            var buffer = new BitBuffer();

            var y = new float[8, 8];
            var cb = new float[8, 8];
            var cr = new float[8, 8];

            short lastYdc = 0;
            short lastCbDc = 0;
            short lastCrDc = 0;

            const int sampling = 1;

            var pixels = new List<byte>(width * height);
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    pixels.Add(rgbs[j, i].R);
                    pixels.Add(rgbs[j, i].G);
                    pixels.Add(rgbs[j, i].B);
                }
            }

            var mcuSize = 8 * sampling;
            for (var mcuY = 0; mcuY < height; mcuY += mcuSize)
            {
                for (var mcuX = 0; mcuX < width; mcuX += mcuSize)
                {
                    for (var blockY = 0; blockY < mcuSize; blockY += 8)
                    {
                        for (var blockX = 0; blockX < mcuSize; blockX += 8)
                        {
                            for (var deltaY = 0; deltaY < 8; deltaY++)
                            {
                                var column = Math.Min(mcuX + blockX, width - 1);
                                var row = Math.Min(mcuY + blockY + deltaY, height - 1);
                                for (var deltaX = 0; deltaX < 8; deltaX++)
                                {
                                    int pixelPos = row * width + column;
                                    var r = pixels[3 * pixelPos];
                                    var g = pixels[3 * pixelPos + 1];
                                    var b = pixels[3 * pixelPos + 2];
                                    y[deltaY, deltaX] = Rgb2Y(r, g, b) - 128;
                                    cb[deltaY, deltaX] = Rgb2Cb(r, g, b);
                                    cr[deltaY, deltaX] = Rgb2Cr(r, g, b);

                                    if (column < width - 1)
                                    {
                                        column++;
                                    }
                                }
                            }
                        }

                        lastYdc = EncodeBlock(output, ref buffer, y, scaledLuminance, lastYdc, huffmanLuminanceDc,
                            huffmanLuminanceAc);
                    }

                    lastCbDc = EncodeBlock(output, ref buffer, cb, scaledChrominance, lastCbDc, huffmanChrominanceDc,
                        huffmanChrominanceAc);
                    lastCrDc = EncodeBlock(output, ref buffer, cr, scaledChrominance, lastCrDc,
                        huffmanChrominanceDc, huffmanChrominanceAc);
                }
            }

            bitWriter.Flush();
            // EOI
            bitWriter.Write(0xFF);
            bitWriter.Write(0xD9);
        }

        private void WriteLuminanceAndChrominance(BitWriter bitWriter)
        {
            bitWriter.AddMarker(0xC4, (2 + 208 + 208));
            bitWriter.Write(0x00);
            bitWriter.Write(_dcLuminanceCodesPerBitSize);
            bitWriter.Write(_dcLuminanceValues);


            bitWriter.Write(0x10);
            bitWriter.Write(_acLuminanceCodesPerBitSize);
            bitWriter.Write(_acLuminanceValues);


            bitWriter.Write(0x01);
            bitWriter.Write(_dcChrominanceCodesPerBitSize);
            bitWriter.Write(_dcChrominanceValues);

            bitWriter.Write(0x11);
            bitWriter.Write(_acChrominanceCodesPerBitSize);
            bitWriter.Write(_acChrominanceValues);
        }

        private static void WriteSize(BitWriter bitWriter, int height, int width)
        {
            bitWriter.AddMarker(0xc0, 2 + 6 + 3 * AmountOfComponents);
            bitWriter.Write(0x08);
            bitWriter.Write((byte) (height >> 8));
            bitWriter.Write((byte) (height & 0xFF));
            bitWriter.Write((byte) (width >> 8));
            bitWriter.Write((byte) (width & 0xFF));

            bitWriter.Write(AmountOfComponents);
        }

        private static void WriteQuantizedLumaAndChroma(BitWriter bitWriter, byte[] quantLuminance,
            byte[] quantChrominance)
        {
            bitWriter.AddMarker(0xDB, 2 + 2 * (1 + 8 * 8));
            bitWriter.Write(0x00);
            bitWriter.Write(quantLuminance);

            bitWriter.Write(0x01);
            bitWriter.Write(quantChrominance);
        }

        private byte[] GetClampedZigZagValuesWithQuality(byte[] input)
        {
            var coefficient = Quality < 50 ? 5000 / Quality : 200 - Quality * 2;
            return Enumerable.Range(0, 64)
                .Select(_ => (input[_zigZagInverse[_]] * coefficient + 50) / 100)
                .Select(_ => (byte) Math.Clamp(_, 1, 255))
                .ToArray();
        }

        private float Rgb2Y(byte r, byte g, byte b) => 0.299f * r + 0.587f * g + 0.114f * b;

        private float Rgb2Cb(byte r, byte g, byte b) => -0.16874f * r - 0.33126f * g + 0.5f * b;

        private float Rgb2Cr(byte r, byte g, byte b) => 0.5f * r - 0.41869f * g - 0.08131f * b;


        private BitCode[] GenerateHuffmanTable(byte[] numCodes, byte[] values)
        {
            ushort huffmanCode = 0;
            int currentIndex = 0;
            var result = new BitCode[256];
            for (int numBits = 1; numBits <= 16; numBits++)
            {
                for (var i = 0; i < numCodes[numBits - 1]; i++)
                {
                    var current = values[currentIndex++];
                    result[current].Code = huffmanCode++;
                    result[current].AmountOfBits = (byte) numBits;
                }

                huffmanCode <<= 1;
            }

            return result;
        }

        private float[] CalculateScaled(byte[] initial)
        {
            var scaled = new float[64];
            for (int i = 0; i < 64; i++)
            {
                var row = _zigZagInverse[i] / 8;
                var col = _zigZagInverse[i] % 8;
                var factor = 1 / (AanScaleFactors[row] *
                                  AanScaleFactors[col] * 8);
                scaled[_zigZagInverse[i]] = factor / initial[i];
            }

            return scaled;
        }
    }
}