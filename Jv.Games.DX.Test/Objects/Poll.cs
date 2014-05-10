using Jv.Games.DX.Components;
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
    class Poll : GameObject, IUpdateable
    {
        enum State
        {
            Unused,
            InitialDelay,
            Sliding,
            FinalDelay,
            Used
        }

        static Texture Texture;
        static Texture TextureTop;

        static Vector2[] DefaultUV = new[] { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1) };

        GameObject _flag;
        Vector3 _topFlagLocation;
        GameObject _sliding;
        RigidBody _slidingBody;
        float _height;
        TimeSpan _delayCount;
        State _state;

        public TimeSpan InitialDelay = TimeSpan.FromSeconds(0.5);
        public TimeSpan FinalDelay = TimeSpan.FromSeconds(1);

        public float Speed = 2;

        public Poll(Device device, float height)
        {
            _height = height;
            Texture = Texture ?? Texture.FromFile(device, "Assets/Textures/poll.png");
            TextureTop = TextureTop ?? Texture.FromFile(device, "Assets/Textures/poll_top.png");

            _topFlagLocation = new Vector3(-0.5f, 0.25f + (height / 2 - 1.5f), 0);

            _flag = Add(new Sprite(device, "flag", 1));
            _flag.Translate(_topFlagLocation);

            Add(new GameObject
            {
                new MeshRenderer
                {
                    Mesh = new TexturedCube(device, 0.5f, 0.5f, 0.5f, DefaultUV, DefaultUV, DefaultUV, DefaultUV, DefaultUV, DefaultUV),
                    Material = new TextureMaterial(TextureTop, false)
                }
            }).Translate(0, 0.25f + (height / 2 - 0.5f), 0);

            Add(new GameObject
            {
                new MeshRenderer
                {
                    Mesh = new TexturedCube(device, 0.2f, height - 0.5f, 0.2f, DefaultUV, DefaultUV, DefaultUV, DefaultUV, DefaultUV, DefaultUV),
                    Material = new TextureMaterial(Texture, false)
                }
            }).Translate(0, -0.25f, 0);

            Add(new Trigger(SlideDown, 0.5f, height, 0.5f, Vector3.Zero));
        }

        void SlideDown(Collider collider)
        {
            if (_state != State.Unused)
                return;

            collider.Object.Translate(new Vector3(GlobalTransform.TranslationVector.X - collider.Object.GlobalTransform.TranslationVector.X - 0.4f, 0, 0));

            _sliding = collider.Object;
            _slidingBody = collider.Object.SearchComponent<RigidBody>();
            TrackFlag();
            _sliding.Enabled = false;
            _flag.Transform = _flag.Transform * Matrix.RotationY((float)Math.PI);

            _delayCount = TimeSpan.Zero;
            _state = State.InitialDelay;
        }

        void TrackFlag()
        {
            if (_sliding == null)
                throw new InvalidOperationException();
            var transform = _flag.Transform;
            var position = transform.TranslationVector;
            position.Y = _sliding.GlobalTransform.TranslationVector.Y - GlobalTransform.TranslationVector.Y;
            transform.TranslationVector = position;
            _flag.Transform = transform;
        }

        public void Update(TimeSpan deltaTime)
        {
            if (_sliding == null)
                return;

            switch (_state)
            {
                case State.InitialDelay:
                    _delayCount += deltaTime;
                    if (_delayCount > InitialDelay)
                        _state = State.Sliding;
                    break;
                case State.Sliding:
                    var toMove = new Vector3(0, -Speed * (float)deltaTime.TotalSeconds, 0);

                    if ((_slidingBody == null && _sliding.GlobalTransform.TranslationVector.Y <= GlobalTransform.TranslationVector.Y - _height / 2 + 1f)
                        || (_slidingBody != null && !_slidingBody.ValidPosition(toMove)))
                    {
                        _delayCount = TimeSpan.Zero;
                        _state = State.FinalDelay;
                        return;
                    }

                    _sliding.Translate(toMove);
                    TrackFlag();
                    break;
                case State.FinalDelay:
                    _delayCount += deltaTime;
                    if (_delayCount > FinalDelay)
                    {
                        _sliding.SendMessage("OnPollSlideComplete", true);

                        _sliding.Enabled = true;
                        _sliding = null;
                        _state = State.Used;
                    }
                    break;
            }
        }
    }
}
