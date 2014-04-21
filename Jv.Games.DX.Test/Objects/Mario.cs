using Jv.Games.DX.Components;
using Jv.Games.DX.Test.Behaviors;
using Jv.Games.DX.Test.Materials;
using Jv.Games.DX.Test.Mesh;
using SharpDX;
using SharpDX.Direct3D9;

namespace Jv.Games.DX.Test.Objects
{
    class Mario : GameObject
    {
        static Texture Texture;

        static Vector2[] BodyFrontUV = new[] { new Vector2(0.172566444131249f, 0.377019484970136f), new Vector2(0.172566444131249f, 0.54592186429061f), new Vector2(0.0388805544570255f, 0.54592186429061f), new Vector2(0.0388805544570255f, 0.375795554685205f) };
        static Vector2[] BodyBackUV = new[] { new Vector2(0.171597705800277f, 0.0453343777538431f), new Vector2(0.170628967469304f, 0.211788896504455f), new Vector2(0.0388805544570255f, 0.211788896504455f), new Vector2(0.0388805544570255f, 0.046558308038774f) };
        static Vector2[] BodyLeftUV = new[] { new Vector2(0.169660229138331f, 0.211788896504455f), new Vector2(0.170628967469304f, 0.378243415255067f), new Vector2(0.0398492927879981f, 0.377019484970136f), new Vector2(0.0398492927879981f, 0.210564966219524f) };
        static Vector2[] BodyRightUV = new[] { new Vector2(0.171597705800277f, 0.54592186429061f), new Vector2(0.170628967469304f, 0.711152452756291f), new Vector2(0.0398492927879981f, 0.711152452756291f), new Vector2(0.0398492927879981f, 0.544697934005679f) };
        static Vector2[] BodyTopUV = new[] { new Vector2(0.0485679377667518f, 0.332957994712621f), new Vector2(0.0485679377667518f, 0.359884460981102f), new Vector2(0.0175683111756275f, 0.359884460981102f), new Vector2(0.0156308345136822f, 0.320718691863311f) };
        static Vector2[] BodyBottomUV = new[] { new Vector2(0.302377380481582f, 0.378243415255067f), new Vector2(0.302377380481582f, 0.544697934005679f), new Vector2(0.170628967469304f, 0.54592186429061f), new Vector2(0.172566444131249f, 0.379467345539998f) };

        static Vector2[] ArmFrontUV = new[] { new Vector2(0.355657988685077f, 0.139577009693528f), new Vector2(0.390532568600092f, 0.184862430235974f), new Vector2(0.32368962376298f, 0.270537550181142f), new Vector2(0.28687756718602f, 0.226476059923627f) };
        static Vector2[] ArmBackUV = new[] { new Vector2(0.355657988685077f, 0.139577009693528f), new Vector2(0.390532568600092f, 0.184862430235974f), new Vector2(0.32368962376298f, 0.270537550181142f), new Vector2(0.28687756718602f, 0.226476059923627f) };
        static Vector2[] ArmLeftUV = new[] { new Vector2(0.355657988685077f, 0.139577009693528f), new Vector2(0.390532568600092f, 0.184862430235974f), new Vector2(0.32368962376298f, 0.270537550181142f), new Vector2(0.28687756718602f, 0.226476059923627f) };
        static Vector2[] ArmRightUV = new[] { new Vector2(0.355657988685077f, 0.139577009693528f), new Vector2(0.390532568600092f, 0.184862430235974f), new Vector2(0.32368962376298f, 0.270537550181142f), new Vector2(0.28687756718602f, 0.226476059923627f) };
        static Vector2[] ArmTopUV = new[] { new Vector2(0.252002987271005f, 0.17996670909625f), new Vector2(0.288815043847965f, 0.225252129638696f), new Vector2(0.252971725601978f, 0.270537550181142f), new Vector2(0.218097145686963f, 0.226476059923627f) };
        static Vector2[] ArmBottomUV = new[] { new Vector2(0.391501306931065f, 0.0967394497209438f), new Vector2(0.425407148515107f, 0.140800939978459f), new Vector2(0.390532568600092f, 0.184862430235974f), new Vector2(0.35662672701605f, 0.140800939978459f) };

        static Vector2[] LegFrontUV = new[] { new Vector2(0.635623366336169f, 0.981641045726035f), new Vector2(0.585248973125592f, 0.981641045726035f), new Vector2(0.586217711456565f, 0.919220601194556f), new Vector2(0.635623366336169f, 0.921668461764418f) };
        static Vector2[] LegBackUV = new[] { new Vector2(0.635623366336169f, 0.981641045726035f), new Vector2(0.585248973125592f, 0.981641045726035f), new Vector2(0.586217711456565f, 0.919220601194556f), new Vector2(0.635623366336169f, 0.921668461764418f) };
        static Vector2[] LegLeftUV = new[] { new Vector2(0.635623366336169f, 0.981641045726035f), new Vector2(0.585248973125592f, 0.981641045726035f), new Vector2(0.586217711456565f, 0.919220601194556f), new Vector2(0.635623366336169f, 0.921668461764418f) };
        static Vector2[] LegRightUV = new[] { new Vector2(0.635623366336169f, 0.981641045726035f), new Vector2(0.585248973125592f, 0.981641045726035f), new Vector2(0.586217711456565f, 0.919220601194556f), new Vector2(0.635623366336169f, 0.921668461764418f) };
        static Vector2[] LegTopUV = new[] { new Vector2(0.635623366336169f, 0.981641045726035f), new Vector2(0.585248973125592f, 0.981641045726035f), new Vector2(0.586217711456565f, 0.919220601194556f), new Vector2(0.635623366336169f, 0.921668461764418f) };
        static Vector2[] LegBottomUV = new[] { new Vector2(0.453500560113313f, 0.964506021737002f), new Vector2(0.462219205092067f, 0.965729952021933f), new Vector2(0.466094158415958f, 0.971849603446588f), new Vector2(0.452531821782341f, 0.970625673161657f) };

        static Vector2[] CapUV = new[] { new Vector2(0.850683275812094f, 0.749094291589151f), new Vector2(0.697622619518418f, 0.555713306570058f), new Vector2(0.733465937764405f, 0.510427886027612f), new Vector2(0.89330776237489f, 0.696465289337119f) };

        AxisAlignedBoxCollider _collider;
        RigidBody _rigidBody;
        GameObject _body;
        GameObject _sizeContainer;
        bool _small;

        public Mario(Device device)
        {
            Tag = "player";

            Texture = Texture ?? Texture.FromFile(device, "Assets/Textures/new-mario.png");

            LoadBehaviors();
            CreateBody(device, Texture);
            IsSmall = false;
        }

        public bool IsSmall
        {
            get { return _small; }
            set
            {
                _small = value;
                var scale = value ? 0.5f : 0.8f;
                _sizeContainer.Transform = Matrix.Scaling(scale);

                _collider.RadiusWidth = 0.6f * scale;
                _collider.RadiusHeight = scale;
                _collider.RadiusDepth = 0.6f * scale;

                while (!_rigidBody.ValidPosition())
                    Translate(0, 0.05 * _rigidBody.Momentum.Y > 0 ? -1 : 1, 0);
            }
        }

        void LoadBehaviors()
        {
            _rigidBody = new RigidBody { MaxSpeed = new Vector3(2, 10, 2), Friction = new Vector3(8f, 0, 8f) };
            Add(_rigidBody);
            Add(new Gravity());
            Add(new Controller { MinJumpForce = 2, MoveForce = 20 });
            Add(new LookForward());
        }

        void CreateBody(Device device, Texture texture)
        {
            _sizeContainer = Add(new GameObject());

            // Container vai ser rotacionado no plano xz para olhar na direção do movimento
            var container = _sizeContainer.Add(new GameObject {
                (_collider = new Components.AxisAlignedBoxCollider())
                //new Behaviors.LookForward()
            });
            container.Tag = Tag;

            // Body container poderá rotacionar no seu eixo X, sem que a direção seja impactada
            var bodyContainer = container.Add(new GameObject());
            //.add(Behaviors.RotateWhileJumping, { speed: 6 });
            bodyContainer.Translate(0, 0.1f, 0);

            _body = bodyContainer.Add(new GameObject());
            _body.Translate(0, -0.5f - bodyContainer.Transform.TranslationVector.Y, 0);

            var xAxis = new Vector3(1, 0, 0);

            AddHead(device, texture);
            AddCap(device, texture);
            AddChest(device, texture);
            AddArm(device, texture, new Vector3(-0.35f, 0.05f, 0f))
                .Add(new SwingWhileMoving(xAxis) { Inverse = true });
            AddArm(device, texture, new Vector3(0.35f, 0.05f, 0f))
                .Add(new SwingWhileMoving(xAxis));
            AddLeg(device, texture, new Vector3(-0.125f, -0.35f, 0f))
                .Add(new SwingWhileMoving(xAxis));
            AddLeg(device, texture, new Vector3(0.125f, -0.35f, 0f))
                .Add(new SwingWhileMoving(xAxis) { Inverse = true });
        }

        GameObject AddHead(Device device, Texture texture)
        {
            var head = _body.Add(new GameObject());
            head.Translate(0, 0.8f, 0);
            head.Add(new MeshRenderer
            {
                Mesh = new MarioHead(device, 1),
                Material = new TextureMaterial(texture, false)
            });
            return head;
        }

        GameObject AddCap(Device device, Texture texture)
        {
            var cap = _body.Add(new GameObject());
            cap.Translate(0, 1, -0.5f);
            cap.Add(new MeshRenderer
            {
                Mesh = new TexturedCube(device, 0.95f, 0.01f, 0.3f, Mario.CapUV, Mario.CapUV, Mario.CapUV, Mario.CapUV, Mario.CapUV, Mario.CapUV),
                Material = new TextureMaterial(texture, false)
            });
            return cap;
        }

        GameObject AddChest(Device device, Texture texture)
        {
            var part = _body.Add(new GameObject());
            part.Add(new MeshRenderer
            {
                Mesh = new TexturedCube(device, 0.5f, 0.5f, 0.5f, Mario.BodyFrontUV, Mario.BodyBackUV, Mario.BodyLeftUV, Mario.BodyRightUV, Mario.BodyTopUV, Mario.BodyBottomUV),
                Material = new TextureMaterial(texture, false)
            });
            return part;
        }

        GameObject AddArm(Device device, Texture texture, Vector3 location)
        {
            var marioArmMesh = new MeshRenderer
            {
                Mesh = new TexturedCube(device, 0.2f, 0.5f, 0.2f,
                    Mario.ArmFrontUV, Mario.ArmBackUV,
                    Mario.ArmLeftUV, Mario.ArmRightUV,
                    Mario.ArmTopUV, Mario.ArmBottomUV),
                Material = new TextureMaterial(texture, false)
            };

            var container = _body.Add(new GameObject());
            container.Translate(0, 0.2f, 0);
            var arm = container.Add(new GameObject());
            arm.Translate(location + new Vector3(0, -0.2f, 0));
            arm.Add(marioArmMesh);
            return container;
        }

        GameObject AddLeg(Device device, Texture texture, Vector3 location)
        {
            var marioLegMesh = new MeshRenderer
            {
                Mesh = new TexturedCube(device, 0.24f, 0.24f, 0.3f,
                    Mario.LegFrontUV, Mario.LegBackUV,
                    Mario.LegLeftUV, Mario.LegRightUV,
                    Mario.LegTopUV, Mario.LegBottomUV),
                Material = new TextureMaterial(texture, false)
            };

            var container = _body.Add(new GameObject());
            container.Translate(0, -0.05f, 0);
            var leg = container.Add(new GameObject());
            leg.Translate(location + new Vector3(0, 0.05f, 0));
            leg.Add(marioLegMesh);
            return container;
        }
    }
}
