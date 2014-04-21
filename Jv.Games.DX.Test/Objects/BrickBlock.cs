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

        float _currentMove;
        const float MaxMove = 0.3f;
        bool _destroy;
        int _moveDirection = 1;
        bool _moving;
        Matrix _originalPosition;

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
                if (body == null || body.Momentum.Y < 0)
                    return;

                if(_moving)
                    return;
                _moving = true;
                _originalPosition = Transform;

                _moveDirection = 1;

                var mario = c.Object as Mario;
                _destroy = mario != null && !mario.IsSmall;
            }, 1, 0.5f, 1, new Vector3(0, -0.5f, 0)));
        }

        public void Update(System.TimeSpan deltaTime)
        {
            if (!_moving)
                return;

            var delta = (float)deltaTime.TotalSeconds * 2 * _moveDirection;

            Translate(new Vector3(0, delta, 0));
            _currentMove += delta;

            if (_currentMove > MaxMove && _moveDirection > 0) {
                _moveDirection *= -1;
                if(_destroy)
                    Dispose();
            }
            if(_currentMove < 0 && _moveDirection < 0)
            {
                _moving = false;
                Transform = _originalPosition;
            }
        }
    }
}
