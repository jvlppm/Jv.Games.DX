using Jv.Games.DX.Components;
using Jv.Games.DX.Test.Behaviors;
using Jv.Games.DX.Test.Materials;
using Jv.Games.DX.Test.Mesh;
using SharpDX;
using SharpDX.Direct3D9;

namespace Jv.Games.DX.Test.Objects
{
    class ItemBlock : GameObject, IUpdateable
    {
        static Texture DefaultTexture, EmptyTexture;

        float? _toMove;
        TextureMaterial _material;

        static Vector2[] UV = new[] { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1) };

        public GameObject Item;

        public ItemBlock(Device device)
        {
            DefaultTexture = DefaultTexture ?? Texture.FromFile(device, "Assets/Textures/block_question.png");
            EmptyTexture = EmptyTexture ?? Texture.FromFile(device, "Assets/Textures/block_empty.png");

            Add(new MeshRenderer
            {
                Mesh = new TexturedCube(device, 1, 1, 1, UV, UV, UV, UV, UV, UV),
                Material = (_material = new TextureMaterial(DefaultTexture, false))
            });

            Add(new AxisAlignedBoxCollider());
            Add(new HeadStomp());
        }

        public void OnHeadStomp(Collider collider)
        {
            _material.Texture = EmptyTexture;

            if(Item != null && _toMove != null)
            {
                Item.Visible = true;
                _toMove = 1;
                Item.Transform = Transform;
                Parent.Add(Item);
                Item.Init();
            }
        }

        public void Update(System.TimeSpan deltaTime)
        {
            if (Item == null || _toMove == null)
                return;

            Item.Translate(0, (float)deltaTime.TotalSeconds, 0);
            _toMove -= (float)deltaTime.TotalSeconds;

            if(_toMove < 0)
            {
                _toMove = null;
                Item.Enabled = true;
                Item = null;
            }
        }
    }
}
