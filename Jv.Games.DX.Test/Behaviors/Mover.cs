using Jv.Games.DX.Components;
using SharpDX;
using System;

namespace Jv.Games.DX.Test.Behaviors
{
    class Mover : Components.Component, IUpdateable
    {
        bool _apply = true;
        public RigidBody RigidBody;

        public bool Continuous = true;
        public bool Acceleration = false;
        public Vector3 Direction;

        public override void Init()
        {
            RigidBody = RigidBody ?? Object.SearchComponent<RigidBody>();
            base.Init();
        }

        public void Update(TimeSpan deltaTime)
        {
            if (_apply || Continuous)
            {
                RigidBody.Push(Direction, !Continuous, Acceleration);
                _apply = false;
            }
        }
    }
}
