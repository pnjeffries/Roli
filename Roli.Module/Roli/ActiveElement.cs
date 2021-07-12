using Nucleus.Game;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roli
{
    /// <summary>
    /// A game element which takes an active role
    /// </summary>
    public class ActiveElement : GameElement
    {
        public ActiveElement()
        {
        }

        public ActiveElement(string name) : base(name)
        {
        }

        public ActiveElement(string name, params IElementDataComponent[] data) : base(name, data)
        {
        }
    }
}
