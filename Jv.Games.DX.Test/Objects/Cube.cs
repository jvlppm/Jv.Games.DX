using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jv.Games.DX.Test.Objects
{
    class Cube : Model
    {
        public Cube(Device device, float width, float height, float depth)
            : base(new Mesh.Cube(device, width, height, depth), new Materials.SimpleMaterial())
        {

        }
    }
}
