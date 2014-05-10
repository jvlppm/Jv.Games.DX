using Jv.Games.DX.Components;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jv.Games.DX.Test.Behaviors
{
    class LookForward : Components.Component, IUpdateable
    {
        public RigidBody RigidBody;

        public override void Init()
        {
            RigidBody = RigidBody ?? Object.SearchComponent<RigidBody>();
            base.Init();
        }

        public void Update(TimeSpan deltaTime)
        {
            LookTo(new Vector3(RigidBody.Momentum.X, 0, RigidBody.Momentum.Z));
        }

        public void LookTo(Vector3 direction)
        {
            if (direction == Vector3.Zero)
                return;

            direction.Normalize();
            var angle = (float)Math.Atan2(-direction.X, direction.Z);
            Object.Transform = Matrix.RotationY(angle) * Matrix.Translation(Object.Transform.TranslationVector);
        }
    }
}
