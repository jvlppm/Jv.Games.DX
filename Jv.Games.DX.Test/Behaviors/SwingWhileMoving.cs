using Jv.Games.DX.Components;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jv.Games.DX.Test.Behaviors
{
    class SwingWhileMoving : Components.Component, IUpdateable
    {
        public RigidBody RigidBody;
        public float Speed = 16;
        public bool Inverse = false;
        public readonly Vector3 Axis;
        public float MaxSwing = 0.4f;

        float _moveTime;
        bool _resetRotation = false;

        public SwingWhileMoving(Vector3 axis)
        {
            Axis = axis;
        }

        public override void Init()
        {
            RigidBody = RigidBody ?? Object.SearchComponent<RigidBody>();
            base.Init();
        }

        public void Update(TimeSpan deltaTime)
        {
            var momentum = RigidBody.Momentum.Length();

            if(momentum > 0.001)
            {
                _moveTime += (float)deltaTime.TotalSeconds * Speed;

                var sin = (float)Math.Sin(_moveTime) * MaxSwing;
                if (Inverse)
                    sin = -sin;

                var toSwing = sin * momentum;
                Object.Transform = Matrix.RotationAxis(Axis, toSwing) * Matrix.Translation(Object.Transform.TranslationVector);
                _resetRotation = true;
            }
            else if (_resetRotation)
            {
                Object.Transform = Object.Transform.ExtractRotation();
                _resetRotation = false;
            }
        }
    }
}
