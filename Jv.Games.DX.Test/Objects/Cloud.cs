using Jv.Games.DX.Components;
using Jv.Games.DX.Test.Materials;
using Jv.Games.DX.Test.Mesh;
using SharpDX.Direct3D9;
using System.Collections.Generic;

namespace Jv.Games.DX.Test.Objects
{
    class Cloud : Sprite
    {
        public Cloud(Device device, int width)
            : base(device, "cloud_" + width, width == 1 ? 1 : 2)
        {
        }
    }
}
