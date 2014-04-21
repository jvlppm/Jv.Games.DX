﻿using SharpDX;
using System;

namespace Jv.Games.DX.Components
{
    public class RigidBody : Component, IUpdateable
    {
        public static float MeterSize = 1;

        Vector3 _acceleration;
        Vector3 _instantAcceleration;
        public Collider Collider;
        Scene _scene;

        public Vector3 Momentum;
        public float Mass = 1;
        public Vector3 Friction;
        public Vector3? MaxSpeed;

        public RigidBody()
        {
            _acceleration = Vector3.Zero;
            _instantAcceleration = Vector3.Zero;
            Momentum = Vector3.Zero;
        }

        public override void Init()
        {
            _scene = Object.GetParent<Scene>();
            Collider = Collider ?? Object.SearchComponent<Collider>();
            base.Init();
        }

        public bool TryMove(TimeSpan deltaTime, Vector3 acceleration, int? axis = null)
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
                toMove = new Vector3(
                    Math.Abs(speed.X) > MaxSpeed.Value.X ? MaxSpeed.Value.X * (speed.X < 0 ? -1 : 1) : speed.X,
                    Math.Abs(speed.Y) > MaxSpeed.Value.Y ? MaxSpeed.Value.Y * (speed.Y < 0 ? -1 : 1) : speed.Y,
                    Math.Abs(speed.Z) > MaxSpeed.Value.Z ? MaxSpeed.Value.Z * (speed.Z < 0 ? -1 : 1) : speed.Z);

                //var speed = new Vector3(toMove.X, 0, toMove.Z).Length();
                //var maxSpeed = MaxSpeed.Value.Length();
                //if (speed > maxSpeed)
                //    toMove *= new Vector3(maxSpeed / speed, 1, maxSpeed / speed);
            }

            var oldTransform = Object.Transform;
            Object.Translate(toMove * MeterSize * (float)deltaTime.TotalSeconds);
            if (ValidPosition())
            {
                Momentum += acceleration;
                return true;
            }

            Object.Transform = oldTransform;
            return false;
        }

        public void Update(TimeSpan deltaTime)
        {
            var addedInstantAccel = _instantAcceleration;
            var addedAccel = _acceleration;

            var accellSecs = addedAccel * (float)deltaTime.TotalSeconds + addedInstantAccel;

            if (!TryMove(deltaTime, accellSecs))
            {
                if (!TryMove(deltaTime, accellSecs, 1))
                    Momentum.Y = 0;
                if (!TryMove(deltaTime, accellSecs, 0))
                    Momentum.X = 0;
                if (!TryMove(deltaTime, accellSecs, 2))
                    Momentum.Z = 0;
            }

            Momentum *= new Vector3
            (
                Math.Max(1 - Friction.X * (float)deltaTime.TotalSeconds, 0),
                Math.Max(1 - Friction.Y * (float)deltaTime.TotalSeconds, 0),
                Math.Max(1 - Friction.Z * (float)deltaTime.TotalSeconds, 0)
            );

            _instantAcceleration -= addedInstantAccel;
            _acceleration -= addedAccel;
        }

        public bool ValidPosition()
        {
            if (Collider == null)
                return true;

            foreach (var other in _scene.Colliders)
            {
                if (other == Collider)
                    continue;

                if (Collider.Intersects(other))
                {
                    if (Collider.IsTrigger || other.IsTrigger)
                    {
                        Object.SendMessage("OnTrigger", true, other);
                        other.Object.SendMessage("OnTrigger", true, Collider);
                    }

                    if (!Collider.IsTrigger && !other.IsTrigger)
                        return false;
                }
            }

            return true;
        }

        public void Push(Vector3 force, bool instantaneous = false, bool acceleration = false)
        {
            if (!acceleration)
                force /= Mass;

            if (!instantaneous)
                _acceleration += force;
            else
                _instantAcceleration += force;
        }
    }
}
