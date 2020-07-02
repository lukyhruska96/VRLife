using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Core.Character
{
    class CharacterLoadingException : Exception
    {
        public CharacterLoadingException(string message) : base(message)
        {
        }
    }
}
