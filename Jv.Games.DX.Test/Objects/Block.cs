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

        public Block(Device device, float width, float height, float depth, string type, bool containsTransparency = false)
            : this(device, width, height, depth, TextureInfo.FromFile(device, Directory.GetFiles("Assets/Textures", "block_" + type + ".*").First()), containsTransparency)
        {
        }

        public Block(Device device, float width, float height, float depth, TextureInfo texture, bool containsTransparency = false)
            : this(device, texture, width, height, depth, containsTransparency)
        {
        }

        public Block(Device device, TextureInfo texture, float width, float height, float depth, bool containsTransparency)
        {
            Material = new TextureMaterial(texture.Texture, containsTransparency);

            var xUV = CreateUV(texture, depth, height);
            var yUV = CreateUV(texture, width, depth);
            var zUV = CreateUV(texture, width, height);

            this.Add(new MeshRenderer
            {
                Mesh = new TexturedCube(device, width, height, depth, zUV, zUV, xUV, xUV, yUV, yUV),
                Material = Material
            });
            
            this.Add(new AxisAlignedBoxCollider { RadiusWidth = width / 2, RadiusHeight = height / 2, RadiusDepth = depth / 2});
        }

        static Vector2[] CreateUV(TextureInfo info, float width, float height) {
            if (!info.Tile)
                return DefaultUV;

            var tw = info.Width;
            var th = info.Height;

            var u = width / (tw / info.Density);
            var v = height / (th / info.Density);

            return DefaultUV.Select(i => i * new Vector2(u, v)).ToArray();
        }
    }
}
