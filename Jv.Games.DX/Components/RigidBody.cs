using SharpDX;
using System;

namespace Jv.Games.DX.Components
{
    public class RigidBody : Component, IUpdateable
    {
        public static float MeterSize = 1;

        public Vector3 Acceleration;
        public Vector3 InstantAcceleration;
        public Collider Collider;
        Scene _scene;

        public Vector3 Momentum;
        public float Mass = 1;
        public Vector3 Friction;
        public Vector3? MaxSpeed;

        public RigidBody()
        {
            Acceleration = Vector3.Zero;
            InstantAcceleration = Vector3.Zero;
            Momentum = Vector3.Zero;
        }

        public override void Init()
        {
            _scene = Object.GetAncestor<Scene>();
            Collider = Collider ?? Object.SearchComponent<Collider>();
            base.Init();
        }

        bool TryMove(TimeSpan deltaTime, Vector3 acceleration, bool notifyColliders, int? axis = null)
        {
            if (axis != null)
            {
                switch (axis)
                {
                    case 0: acceleration = new Vector3(acceleration.X, 0, 0); break;
                    case 1: acceleration = new Vector3(0, acceleration.Y, 0); break;
                    case 2: acceleration = new Vector3(0, 0, acceleration.Z); break;
                    default: throw new InvalidOperationException("Invalid axis");
                }
            }

            var toMove = Momentum + acceleration / 2;

            if (axis != null)
            {
                switch (axis)
                {
                    case 0: toMove = new Vector3(toMove.X, 0, 0); break;
                    case 1: toMove = new Vector3(0, toMove.Y, 0); break;
                    case 2: toMove = new Vector3(0, 0, toMove.Z); break;
                    default: throw new InvalidOperationException("Invalid axis");
                }
            }

            if (MaxSpeed != null && toMove != Vector3.Zero)
            {
                var speed = toMove;
                var maxSpeed = MeterSize * MaxSpeed * (float)deltaTime.TotalSeconds;
                toMove = new Vector3(
                    Math.Abs(speed.X) > maxSpeed.Value.X ? maxSpeed.Value.X * (speed.X < 0 ? -1 : 1) : speed.X,
                    Math.Abs(speed.Y) > maxSpeed.Value.Y ? maxSpeed.Value.Y * (speed.Y < 0 ? -1 : 1) : speed.Y,
                    Math.Abs(speed.Z) > maxSpeed.Value.Z ? maxSpeed.Value.Z * (speed.Z < 0 ? -1 : 1) : speed.Z);

                //var speed = new Vector3(toMove.X, 0, toMove.Z).Length();
                //var maxSpeed = MaxSpeed.Value.Length();
                //if (speed > maxSpeed)
                //    toMove *= new Vector3(maxSpeed / speed, 1, maxSpeed / speed);
            }

            var oldTransform = Object.Transform;
            Object.Translate(toMove * MeterSize * (float)deltaTime.TotalSeconds);
            if (ValidPosition(notifyColliders))
            {
                Momentum += acceleration;
                return true;
            }

            Object.Transform = oldTransform;
            return false;
        }

        public void Update(TimeSpan deltaTime)
        {
            var addedInstantAccel = InstantAcceleration;
            var addedAccel = Acceleration;

            var accellSecs = addedAccel * (float)deltaTime.TotalSeconds + addedInstantAccel;

            if (!TryMove(deltaTime, accellSecs, true))
            {
                if (!TryMove(deltaTime, accellSecs, false, 1))
                    Momentum.Y = 0;
                if (!TryMove(deltaTime, accellSecs, false, 0))
                    Momentum.X = 0;
                if (!TryMove(deltaTime, accellSecs, false, 2))
                    Momentum.Z = 0;
            }

            Momentum *= new Vector3
            (
                Math.Max(1 - Friction.X * (float)deltaTime.TotalSeconds, 0),
                Math.Max(1 - Friction.Y * (float)deltaTime.TotalSeconds, 0),
                Math.Max(1 - Friction.Z * (float)deltaTime.TotalSeconds, 0)
            );

            InstantAcceleration -= addedInstantAccel;
            Acceleration -= addedAccel;
        }

        public bool ValidPosition(Vector3 offset)
        {
            var origTransform = Object.Transform;
            Object.Translate(offset);
            var res = ValidPosition(false);
            Object.Transform = origTransform;
            return res;
        }

        bool ValidPosition(bool notifyColliders)
        {
            if (Collider == null)
                return true;

            bool result = true;

            foreach (var other in _scene.Colliders)
            {
                if (other == Collider)
                    continue;

                if (Collider.Intersects(other))
                {
                    if (notifyColliders)
                    {
                        string message = Collider.IsTrigger || other.IsTrigger ?
                                            "OnTrigger" : "OnCollide";
                        Object.SendMessage(message, true, other);
                        other.Object.SendMessage(message, true, Collider);
                    }

                    if (!Collider.IsTrigger && !other.IsTrigger)
                        result = false;
                }
            }

            return result;
        }

        public void Push(Vector3 force, bool instantaneous = false, bool acceleration = false)
        {
            if (!acceleration)
                force /= Mass;

            if (!instantaneous)
                Acceleration += force;
            else
                InstantAcceleration += force;
        }
    }
}
