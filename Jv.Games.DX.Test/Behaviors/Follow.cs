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

        public Vector3 Offset;

        public Follow(GameObject target)
        {
            _target = target;
        }

        public override void Init()
        {
            _rigidBody = Object.SearchComponent<RigidBody>();
            base.Init();
        }

        public void Update(TimeSpan deltaTime)
        {
            var d = _target.GlobalTransform.TranslationVector - Object.GlobalTransform.TranslationVector + Offset;
            _rigidBody.Push(d * Mask, false, false);
        }
    }
}
