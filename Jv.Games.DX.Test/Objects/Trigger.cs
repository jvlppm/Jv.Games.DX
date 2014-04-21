using Jv.Games.DX.Components;
using SharpDX;
using System;

namespace Jv.Games.DX.Test.Objects
{
    class Trigger : GameObject
    {
        readonly Action<Collider> _onTrigger;

        public Trigger(Action<Collider> onTrigger, float width, float height, float depth, Vector3 position)
        {
            _onTrigger = onTrigger;
            Translate(position);
            Add(new AxisAlignedBoxCollider{ IsTrigger = true, RadiusWidth = width / 2, RadiusHeight = height / 2, RadiusDepth = depth / 2 });
        }

        public void OnTrigger(Collider collider)
        {
            if (_onTrigger != null)
                _onTrigger(collider);
        }
    }
}
