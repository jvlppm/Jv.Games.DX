using Jv.Games.DX.Components;
using Jv.Games.DX.Test.Materials;
using Jv.Games.DX.Test.Mesh;
using SharpDX;
using SharpDX.Direct3D9;
using System;

namespace Jv.Games.DX.Test.Objects
{
    class BrickBlock : GameObject, IUpdateable
    {
        static Texture Texture;

        static Vector2[] TopUV = new[] { new Vector2(0.548436916548632f, 0.678106335063155f), new Vector2(0.638529581329087f, 0.680554195633017f), new Vector2(0.634654628005196f, 0.777244688142563f), new Vector2(0.556186823196413f, 0.780916478997356f) };
        static Vector2[] BottomUV = new[] { new Vector2(0.524218458274316f, 0.303583667874278f), new Vector2(0.561999253182249f, 0.302359737589347f), new Vector2(0.562967991513221f, 0.347645158131793f), new Vector2(0.522280981612371f, 0.347645158131793f) };
        static Vector2[] UV = new[] { new Vector2(0.500968738330973f, 0.368451972975619f), new Vector2(0.707310002828144f, 0.368451972975619f), new Vector2(0.706341264497171f, 0.631596984235778f), new Vector2(0.5f, 0.631596984235778f) };

        float? _toMove;

        public BrickBlock(Device device)
        {
            Texture = Texture ?? Texture.FromFile(device, "Assets/Textures/block_brick.png");

            Add(new MeshRenderer
            {
                Mesh = new TexturedCube(device, 1, 1, 1, UV, UV, UV, UV, TopUV, BottomUV),
                Material = new TextureMaterial(Texture, false)
            });

            Add(new AxisAlignedBoxCollider());

            Add(new Trigger(c => {
                var body = c.Object.SearchComponent<RigidBody>();
                if (body.Momentum.Y < 0)
                    return;

                if (_toMove == null)
                    _toMove = 0.3f;
            }, 1, 0.5f, 1, new Vector3(0, -0.5f, 0)));
        }

        public void Update(System.TimeSpan deltaTime)
        {
            if (_toMove == null)
                return;

            Translate(new Vector3(0, (float)deltaTime.TotalSeconds * 2, 0));
            _toMove -= (float)deltaTime.TotalSeconds * 2;

            if (_toMove < 0) {
                _toMove = null;
                Dispose();
            }
        }
    }
}
