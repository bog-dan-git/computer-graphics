using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ComputerGraphics.Converters.Sdk.Model;

namespace ComputerGraphics.Gif
{
    public class GifLzvDecompressor
    {
        private BitArray _compressedData;
        private int _codeLength;
        private Dictionary<int, List<int>> _dictionary;
        private int _clearCell;
        private int _endCell;
        private int _currentBit;

        private List<int> _colorIndexes;
        
        public List<int> Decompress(List<byte> compressedData, int minCodeLength, Dictionary<int, RgbColor> baseDictionary)
        {
            _compressedData = new BitArray(compressedData.ToArray());
            _currentBit = 0;
            InitDictionary(baseDictionary, minCodeLength);
            _colorIndexes = new List<int>();

            
            var code = GetNextCode();
            if (code != _clearCell)
            {
                throw new Exception();
            }
            
            code = GetNextCode();
            _colorIndexes.Add(code);
            var lastCode = code;
            code = GetNextCode();
            
            var resultColorList = new List<int>();

            while (code != _endCell)
            {
                if (code != _clearCell)
                {
                    int k;
                    if (_dictionary.ContainsKey(code))
                    {
                        _colorIndexes.Add(code);
                        k = _dictionary[code][0];
                        var lastCellContent = _dictionary[lastCode].ToList();
                        lastCellContent.Add(k);
                        _dictionary[_dictionary.Count] = lastCellContent;
                    }
                    else
                    {
                        var lastCellContent = _dictionary[lastCode].ToList();
                        k = lastCellContent[0];
                        lastCellContent.Add(k);
                        _colorIndexes.Add(_dictionary.Count);
                        _dictionary[_dictionary.Count] = lastCellContent;
                    }

                    lastCode = code;

                    if (IsPowerOfTwo(_dictionary.Count) && _codeLength < 12)
                    {
                        _codeLength++;
                    }
                }
                else
                {
                    resultColorList.AddRange(FormColorList());
                    InitDictionary(baseDictionary, minCodeLength);
                    code = GetNextCode();

                    _colorIndexes = new List<int> {code};
                    lastCode = code;
                }

                code = GetNextCode();
            }
                
            resultColorList.AddRange(FormColorList());
            return resultColorList;
        }

        private List<int> FormColorList()
        {
            var result = new List<int>();
            foreach (var colorIndex in _colorIndexes)
            {
                result.AddRange(_dictionary[colorIndex]);
            }

            return result;
        }

        private int GetNextCode()
        {
            var code = BitSubArray(_compressedData, _currentBit, _codeLength);
            _currentBit += _codeLength;

            return GetIntFromBitArray(code);
        }

        private void InitDictionary(Dictionary<int, RgbColor> baseDictionary, int minCodeLength)
        {
            _codeLength = minCodeLength;
            _dictionary = new Dictionary<int, List<int>>();
            foreach (var key in baseDictionary.Keys)
            {
                _dictionary[key] = new List<int> {key};
            }

            _clearCell = (int) Math.Pow(2, _codeLength - 1);
            _endCell = _clearCell + 1;
            
            while (_dictionary.Count < _clearCell)
            {
                _dictionary[_dictionary.Count] = new List<int> {-3};
            }

            _dictionary[_clearCell] = new List<int> {-1};
            _dictionary[_endCell] = new List<int> {-2};
        }

        private static bool IsPowerOfTwo(int a)
        {
            return a > 0 && (a & (a - 1)) == 0;
        }
        
        private void OutputFinalResults()
        {
            Console.WriteLine("------------------------------------");
            Console.WriteLine("Result: ");
            foreach (var color in _colorIndexes)
            {
                var colorList = "{ ";
                foreach (var c in _dictionary[color])
                {
                    colorList = colorList + c + ", ";
                }

                colorList += "}";
                
                Console.WriteLine(color + ": " + colorList);
            }
        }
        
        private static BitArray BitSubArray(BitArray bitArray, int start, int size)
        {
            var sub = new bool[size];

            for (var i = start; i < start + size; i++)
            {
                sub[i - start] = bitArray[i];
            }

            return new BitArray(sub);
        }        
        
        private static int GetIntFromBitArray(BitArray bitArray)
        {
            
            if (bitArray.Length > 32)
                throw new ArgumentException("Argument length shall be at most 32 bits.");

            int[] array = new int[1];
            bitArray.CopyTo(array, 0);
            return array[0];

        }

        private void OutputDictionary()
        {
            Console.WriteLine("------------------------------------");
            Console.WriteLine("Dictionary: ");
            foreach (var (key, value) in _dictionary)
            {
                Console.WriteLine(key + ": {" +  string.Join(',', value.ToArray()) + "}");
            }
        }
    }
}