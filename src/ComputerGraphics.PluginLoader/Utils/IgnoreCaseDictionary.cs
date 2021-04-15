using System;
using System.Collections.Generic;

namespace ComputerGraphics.PluginLoader.Utils
{
    internal class IgnoreCaseDictionary<T> : Dictionary<string, T>
    {
        public IgnoreCaseDictionary() : base(StringComparer.InvariantCultureIgnoreCase)
        {
            
        }
    }
}