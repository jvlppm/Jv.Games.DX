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
                    Vector3 mask = Vector3.Zero;
                    if ((rigidBody.Momentum.X < 0) != (Force.X < 0))
                    {
                        rigidBody.Acceleration.X = 0;
                        rigidBody.InstantAcceleration.X = 0;
                        rigidBody.Momentum.X = 0;
                        mask.X = 1;
                    }

                    if ((rigidBody.Momentum.Y < 0) != (Force.Y < 0))
                    {
                        rigidBody.Acceleration.Y = 0;
                        rigidBody.InstantAcceleration.Y = 0;
                        rigidBody.Momentum.Y = 0;
                        mask.Y = 1;
                    }

                    if ((rigidBody.Momentum.Z < 0) != (Force.Z < 0))
                    {
                        rigidBody.Acceleration.Z = 0;
                        rigidBody.InstantAcceleration.Z = 0;
                        rigidBody.Momentum.Z = 0;
                        mask.Z = 1;
                    }

                    rigidBody.Push(Force * mask, true);
                }
            }
        }
    }
}
