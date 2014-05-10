using Jv.Games.DX.Components;
using Jv.Games.DX.Test.Materials;
using Jv.Games.DX.Test.Mesh;
using SharpDX.Direct3D9;
using System.Collections.Generic;

namespace Jv.Games.DX.Test.Objects
{
    class Sprite : GameObject
    {
        static Dictionary<string, Texture> Textures = new Dictionary<string, Texture>();

        public string Name;

        public Sprite(Device device, string name, float size)
        {
            string fileName = "sprite_" + name;

            Name = name;

            Texture texture;
            if (!Textures.TryGetValue(fileName, out texture))
            {
                var a = device.Capabilities.TextureCaps.HasFlag(TextureCaps.NonPow2Conditional);
                Textures.Add(fileName, texture = Texture.FromFile(device, "Assets/Textures/" + fileName + ".png", 0, 0, 0, Usage.None, Format.Unknown, Pool.Managed, Filter.Default, Filter.Default, 0));
            }

            Add(new MeshRenderer
            {
                Mesh = new Billboard(device, size, size),
                Material = new TextureMaterial(texture, containsTransparency: true)
            });
        }
    }
}
