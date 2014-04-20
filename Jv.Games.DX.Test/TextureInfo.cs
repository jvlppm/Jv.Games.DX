using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jv.Games.DX.Test
{
    struct TextureInfo
    {
        public bool Tile;
        public int Width;
        public int Height;
        public float Density;
        public Texture Texture;

        public static TextureInfo FromFile(Device device, string file, bool tile = true)
        {
            var t = Texture.FromFile(device, file);
            var info = t.GetLevelDescription(0);

            return new TextureInfo
            {
                Tile = tile,
                Width = info.Width,
                Height = info.Height,
                Density = info.Width,
                Texture = t
            };
        }
    }
}
