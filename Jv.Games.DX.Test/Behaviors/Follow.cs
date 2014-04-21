using Jv.Games.DX.Components;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jv.Games.DX.Test.Behaviors
{
    class Follow : Components.Component, IUpdateable
    {
        GameObject _target;
        public Vector3 Mask = Vector3.One;
        RigidBody _rigidBody;
        RigidBody _targetBody;

        public Vector3 Offset;
        public float Speed = 20;

        public Follow(GameObject target)
        {
            _target = target;
        }

        public override void Init()
        {
            _rigidBody = Object.SearchComponent<RigidBody>();
            _targetBody = _target.SearchComponent<RigidBody>();
            base.Init();
        }

        public void Update(TimeSpan deltaTime)
        {
            var d = _target.GlobalTransform.TranslationVector - Object.GlobalTransform.TranslationVector + Offset;
            var dist = (d * Mask).Length();

            float moveSpeed = 0;
            if (dist <= 1)
                moveSpeed = 0.1f;
            else
                moveSpeed = Speed;

            var toMove = (d * Mask);
            toMove.Normalize();


            _rigidBody.Push(toMove * moveSpeed, false, false);
        }
    }
}
