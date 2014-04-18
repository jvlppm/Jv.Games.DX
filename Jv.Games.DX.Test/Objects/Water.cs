using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jv.Games.DX.Test.Objects
{
    class Water : Model
    {
        public Water(Device device, float width, float depth, int rows, int cols)
            : base(new Mesh.TriangleGrid(device, width, depth, rows, cols), new Materials.WaveMaterial())
        {

        }
    }
}
