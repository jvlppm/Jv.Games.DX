using Jv.Games.DX.Components;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jv.Games.DX.Test.Behaviors
{
    class RandomWalk : Components.Component, IUpdateable
    {
        RigidBody _body;
        public Vector3 CurrentDirection = new Vector3(1, 0, 0);

        public float Speed = 1;
        public bool CanFall;

        public override void Init()
        {
            _body = Object.SearchComponent<RigidBody>();
            base.Init();
        }

        public void Update(TimeSpan deltaTime)
        {
            if (!CanFall)
            {
                var testDirection = CurrentDirection * (float)deltaTime.TotalSeconds;

                var originalPosition = _body.Object.Transform;
                _body.Object.Translate(CurrentDirection * 0.5f + new Vector3(0, -0.5f, 0));
                if (_body.ValidPosition())
                    CurrentDirection *= -1;
                _body.Object.Transform = originalPosition;

                _body.Object.Translate(testDirection);
                if (!_body.ValidPosition())
                    CurrentDirection *= -1;
                _body.Object.Transform = originalPosition;
            }

            _body.Push(CurrentDirection);
        }
    }
}
