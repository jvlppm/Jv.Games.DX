using Jv.Games.DX.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jv.Games.DX.Test.Behaviors
{
    class Stomp : Components.Component
    {
        public string[] Tags;

        public void OnCollide(Collider collider)
        {
            if (Tags != null && !Tags.Contains(collider.Object.Tag))
                return;

            var other = collider.Object.GlobalTransform.TranslationVector;

            var box = collider as AxisAlignedBoxCollider;
            if (box == null)
                throw new NotImplementedException();

            var pos = Object.GlobalTransform.TranslationVector;
            var col = Object.SearchComponent<AxisAlignedBoxCollider>();

            if (other.Y - box.RadiusHeight > pos.Y - col.RadiusHeight + 0.1f)
            {
                var body = collider.Object.SearchComponent<RigidBody>();
                if (body != null)
                {
                    body.Momentum.Y = 0;
                    body.Push(new SharpDX.Vector3(0, 2.5f, 0), true);
                }
                Object.SendMessage("OnStomp", true, collider);
            }
            else
            {
                collider.Object.SendMessage("OnHit", true);
            }
        }
    }
}
