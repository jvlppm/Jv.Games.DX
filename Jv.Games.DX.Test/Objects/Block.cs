using Jv.Games.DX.Test.Materials;
using Jv.Games.DX.Test.Mesh;
using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jv.Games.DX.Test.Objects
{
    class Block : Model
    {
        static Vector2[] UV = new[] { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1) };

        public Block(Device device, float size, Texture texture)
            : base(new TexturedCube(device, size, size, size, UV, UV, UV, UV, UV, UV), new TextureMaterial(texture))
        {

        }
    }
}
