using Jv.Games.DX.Components;
using SharpDX;
using System;
using System.Linq;

namespace Jv.Games.DX.Test.Behaviors
{
    class Bouncy : Components.Component
    {
        public Vector3 Force = new Vector3(0, 1, 0);
        public Vector3 Restitution = new Vector3(1, 0.5f, 1);
        public string[] Tags;

        public void OnTrigger(Collider collider)
        {
            if (Tags == null || (collider.Object != null && Tags.Contains(collider.Object.Tag)))
            {
                var rigidBody = collider.Object.SearchComponent<RigidBody>();
                if (rigidBody != null)
                {
                    if ((rigidBody.Momentum.X < 0) != (Force.X < 0))
                        rigidBody.Momentum.X = 0;
                    else
                        rigidBody.Momentum.X *= Restitution.X;

                    if ((rigidBody.Momentum.Y < 0) != (Force.Y < 0))
                        rigidBody.Momentum.Y = 0;
                    else
                        rigidBody.Momentum.Y *= Restitution.Y;

                    if ((rigidBody.Momentum.Z < 0) != (Force.Z < 0))
                        rigidBody.Momentum.Z = 0;
                    else
                        rigidBody.Momentum.Z *= Restitution.Z;

                    rigidBody.Push(Force, true);
                }
            }
        }
    }
}
