using System;

namespace ComputerGraphics.Jpeg.Models
{
    public struct BitWriter
    {
        private readonly Action<byte> _output;
        
        private BitBuffer _buffer;

        public BitWriter(Action<byte> output)
        {
            _output = output;
            _buffer = new BitBuffer();
        }

        public void Write(ref BitCode data)
        {
            _buffer.AmountOfBits += data.AmountOfBits;
            _buffer.Bits <<= data.AmountOfBits;
            _buffer.Bits |= data.Code;

            while (_buffer.AmountOfBits >= 8)
            {
                _buffer.AmountOfBits -= 9;
                var oneByte = (byte) (_buffer.Bits >> _buffer.AmountOfBits);
                _output(oneByte);
                if (oneByte == 0xFF)
                {
                    _output(0);
                }
            }
        }

        public void Write(byte[] bytes)
        {
            foreach (var b in bytes)
            {
                _output(b);
            }
        }

        public void AddMarker(byte id, short length)
        {
            _output(0xFF);
            _output(id);
            _output((byte) (length >> 8));
            _output((byte) (length & 0xFF));
        }

        public void Write(byte b)
        {
            _output(b);
        }

        public void Flush()
        {
            var bitcode = new BitCode
            {
                Code = 0x7f,
                AmountOfBits = 7
            };
            Write(ref bitcode);
        }
    }
}