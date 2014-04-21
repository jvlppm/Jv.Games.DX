using Jv.Games.DX.Components;
using SharpDX;
using System;
using System.Linq;

namespace Jv.Games.DX.Test.Behaviors
{
    class Bouncy : Components.Component
    {
        public Vector3 Force = new Vector3(0, 2, 0);
        public string[] Tags;

        public void OnTrigger(Collider collider)
        {
            if (Tags == null || (collider.Object != null && Tags.Contains(collider.Object.Tag)))
            {
                var rigidBody = collider.Object.SearchComponent<RigidBody>();
                if (rigidBody != null)
                    rigidBody.Push(Force, true);
            }
        }
    }
}
