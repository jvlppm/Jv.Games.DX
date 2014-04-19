using Jv.Games.DX.Test.Materials;
using SharpDX;
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
        public Water(Device device, float width, float depth)
            : this(device, width, depth, (int)(depth / 2), (int)(width / 2))
        { }
        public Water(Device device, float width, float depth, int rows, int cols)
            : base(new Mesh.TriangleGrid(device, width, depth, rows, cols), new Materials.WaveMaterial())
        {
        }
    }
}
