using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jv.Games.DX.Components
{
    public abstract class Component
    {
        public GameObject Object;

        public virtual void Init() { }
    }
}
