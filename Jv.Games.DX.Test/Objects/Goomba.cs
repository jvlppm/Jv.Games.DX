using Jv.Games.DX.Components;
using Jv.Games.DX.Test.Behaviors;
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
    class Goomba : GameObject
    {
        static Texture Texture;

        static Vector2[] FrontUV = new[] { new Vector2(0.469969111739848f, 0.417409184372858f), new Vector2(0.673404161244102f, 0.416185254087927f), new Vector2(0.673404161244102f, 0.641388426515226f), new Vector2(0.469969111739848f, 0.642612356800157f) };
        static Vector2[] BackUV = new[] { new Vector2(0.064067751062314f, 0.417409184372858f), new Vector2(0.26847153889754f, 0.416185254087927f), new Vector2(0.26847153889754f, 0.643836287085088f), new Vector2(0.064067751062314f, 0.641388426515226f) };
        static Vector2[] LeftUV = new[] { new Vector2(0.267502800566567f, 0.417409184372858f), new Vector2(0.471906588401794f, 0.416185254087927f), new Vector2(0.471906588401794f, 0.643836287085088f), new Vector2(0.267502800566567f, 0.642612356800157f) };
        static Vector2[] RightUV = new[] { new Vector2(0.267502800566567f, 0.417409184372858f), new Vector2(0.471906588401794f, 0.416185254087927f), new Vector2(0.471906588401794f, 0.643836287085088f), new Vector2(0.267502800566567f, 0.642612356800157f) };
        static Vector2[] TopUV = new[] { new Vector2(0.469000373408876f, 0.638940565945364f), new Vector2(0.670497946251184f, 0.638940565945364f), new Vector2(0.673404161244102f, 0.895965925780868f), new Vector2(0.469000373408876f, 0.895965925780868f) };
        static Vector2[] BottomUV = new[] { new Vector2(0.468031635077903f, 0.162831685107216f), new Vector2(0.674372899575074f, 0.161607754822285f), new Vector2(0.672435422913129f, 0.41985704494272f), new Vector2(0.469000373408876f, 0.421080975227651f) };

        public Goomba(Device device)
        {
            Texture = Texture ?? Texture.FromFile(device, "Assets/Textures/goomba.png");

            Tag = "npc";
            Add(new Gravity());
            Add(new RandomWalk { CurrentDirection = new Vector3(-1, 0, 0) });
            Add(new RigidBody { MaxSpeed = new Vector3(0.4f, float.PositiveInfinity, 0), Friction = new Vector3(3) });
            Add(new AxisAlignedBoxCollider { RadiusWidth = 0.45f, RadiusDepth = 0.45f, RadiusHeight = 0.45f });
            Add(new Stomp { Tags = new[] { "player" } });
            Add(new DieOnStomp { Tags = new[] { "player" } });
            Add(new GameObject { new LookForward() })
                .Add(new GameObject {
                    new SwingWhileMoving(new Vector3(0, 0, 1)) { Speed = 15, MaxSwing = 0.1f} })
                .Add(new MeshRenderer
                    {
                        Mesh = new TexturedCube(device, 1, 1, 1, FrontUV, BackUV, LeftUV, RightUV, TopUV, BottomUV),
                        Material = new TextureMaterial(Texture, false)
                    });

            Transform *= Matrix.Scaling(0.8f);
        }
    }
}
