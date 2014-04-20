using Jv.Games.DX.Components;
using Jv.Games.DX.Test.Materials;
using Jv.Games.DX.Test.Mesh;
using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jv.Games.DX.Test.Objects
{
    class Block : GameObject
    {
        public readonly Material Material;

        static Vector2[] DefaultUV = new[] { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1) };

        public Block(Device device, float size, string type, bool containsTransparency = false)
            : this(device, size, Texture.FromFile(device, Directory.GetFiles("Assets/Textures", "block_" + type + ".*").First()), containsTransparency, DefaultUV, DefaultUV, DefaultUV, DefaultUV, DefaultUV, DefaultUV)
        {
        }

        public Block(Device device, float size, Texture texture, bool containsTransparency = false)
            : this(device, size, texture, containsTransparency, DefaultUV, DefaultUV, DefaultUV, DefaultUV, DefaultUV, DefaultUV)
        {
        }

        public Block(Device device, float size, Texture texture, bool containsTransparency, Vector2[] frontUV, Vector2[] backUV, Vector2[] topUV, Vector2[] bottomUV, Vector2[] leftUV, Vector2[] rightUV)
        {
            Material = new TextureMaterial(texture, containsTransparency);
            this.Add(new MeshRenderer
            {
                Mesh = new TexturedCube(device, size, size, size, frontUV, backUV, topUV, bottomUV, leftUV, rightUV),
                Material = Material
            });
        }
    }
}
