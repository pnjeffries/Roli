using Nucleus.Game;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roli
{
    /// <summary>
    /// A game element which is static and does not move
    /// </summary>
    public class StaticElement : GameElement
    {
        public StaticElement()
        {
        }

        public StaticElement(string name) : base(name)
        {
        }

        public StaticElement(string name, params IElementDataComponent[] data) : base(name, data)
        {
        }
    }
}
