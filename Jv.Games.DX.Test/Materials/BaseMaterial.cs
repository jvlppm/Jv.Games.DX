using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jv.Games.DX.Test.Materials
{
    abstract class BaseMaterial : Material
    {
        protected BaseMaterial(string shader, string technique)
            : base(shader, technique)
        {

        }

        public Matrix WVP { set { Set("WVP", value); } }
    }
}
