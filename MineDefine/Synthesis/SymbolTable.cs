using System;
using System.Collections.Immutable;

namespace MineDefine.Synthesis
{
    public class SymbolTable
    {
        public SymbolTable Parent { get; private set; }

        private readonly IImmutableDictionary<string, IElement> elements;
        private SymbolTable(SymbolTable parent, IImmutableDictionary<string, IElement> dict)
        {
            Parent = parent;
            elements = dict;
        }

        public IElement this[string name]
        {
            get
            {
                if (elements.ContainsKey(name)) return elements[name];
                if (Parent != null) return Parent[name];
                throw new Exception("Unrecognized symbol: " + name);
            }
        }

        public SymbolTable AddSymbol(string name, IElement element)
        {
            var dict = elements.Add(name, element);
            return new SymbolTable(Parent, dict);
        }

        public SymbolTable Pop()
        {
            return Parent;
        }

        public SymbolTable Push()
        {
            return new SymbolTable(this, ImmutableDictionary<string,IElement>.Empty);
        }

        public static SymbolTable BaseSymbolTable
        {
            get { return new SymbolTable(null,ImmutableDictionary<string,IElement>.Empty); }
        }
    }
}