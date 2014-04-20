using Jv.Games.DX.Components;
using SharpDX;
using System;

namespace Jv.Games.DX.Test.Behaviors
{
    class Mover : Components.Component, IUpdateable
    {
        bool _apply = true;
        RigidBody _rigidBody;

        public bool Continuous = true;
        public bool Acceleration = false;
        public Vector3 Direction;

        public override void Init()
        {
            _rigidBody = Object.SearchComponent<RigidBody>();
            base.Init();
        }

        public void Update(TimeSpan deltaTime)
        {
            if (_apply || Continuous)
            {
                _rigidBody.Push(Direction, !Continuous, Acceleration);
                _apply = false;
            }
        }
    }
}
