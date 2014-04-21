using Jv.Games.DX.Components;
using SharpDX;
using System;
using System.Linq;

namespace Jv.Games.DX.Test.Behaviors
{
    class HitOnTrigger : Components.Component
    {
        public string[] Tags;

        public void OnTrigger(Collider collider)
        {
            if (Tags == null || (collider.Object != null && Tags.Contains(collider.Object.Tag)))
                collider.Object.SendMessage("OnHit", false);
        }
    }
}
