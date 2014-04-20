using Jv.Games.DX.Components;
using Jv.Games.DX.Test.Materials;
using Jv.Games.DX.Test.Mesh;
using SharpDX;
using SharpDX.Direct3D9;

namespace Jv.Games.DX.Test.Objects
{
    class ItemBlock : GameObject
    {
        static Vector2[] UV = new[] { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1) };

        public ItemBlock(Device device)
        {
            var renderer = new MeshRenderer
            {
                Mesh = new TexturedCube(device, 1, 1, 1, UV, UV, UV, UV, UV, UV),
                Material = new TextureMaterial(Texture.FromFile(device, "Assets/Textures/block_question.png"), false)
            };

            Add(renderer);
        }
    }
}
