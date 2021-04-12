using System;
using System.Collections.Generic;

namespace ComputerGraphics.PluginLoader.Utils
{
    public class IgnoreCaseDictionary<T> : Dictionary<string, T>
    {
        public IgnoreCaseDictionary() : base(StringComparer.InvariantCultureIgnoreCase)
        {
            
        }
    }
}