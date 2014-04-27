using Jv.Games.DX.Components;
using SharpDX;
using System;

namespace Jv.Games.DX.Test.Behaviors
{
    class HeadStomp : Components.Component, IUpdateable
    {
        float _currentMove;
        const float MaxMove = 0.3f;
        public Func<Collider, bool> CanDestroy;
        int _moveDirection = 1, _lastDirection;
        bool _moving;
        Matrix _originalPosition;
        Collider _lastCollider;
        public bool ValidatePosition;
        RigidBody _body;

        public override void Init()
        {
            _body = Object.SearchComponent<RigidBody>();
            if (_body == null && ValidatePosition)
                throw new InvalidOperationException("No RigidBody found");
            base.Init();
        }

        public void OnCollide(Collider collider)
        {
            if (_moving)
                return;

            _lastCollider = collider;

            var body = collider.Object.SearchComponent<RigidBody>();
            if (body == null || body.Momentum.Y < 0)
                return;

            if (!ValidCollision(collider))
                return;

            Object.SendMessage("OnHeadStomp", true, collider);
            _moving = true;
            _originalPosition = Object.Transform;
        }

        bool ValidCollision(Collider collider)
        {
            var other = collider.Object.GlobalTransform.TranslationVector;
            var pos = Object.GlobalTransform.TranslationVector;
            var col = Object.SearchComponent<AxisAlignedBoxCollider>();

            var box = collider as AxisAlignedBoxCollider;
            if (box == null)
                return false;

            if (other.Y + box.RadiusHeight < pos.Y)
                return true;

            var dist = box.Object.Transform.TranslationVector - Object.Transform.TranslationVector;
            var distL = new Vector2(dist.X, dist.Z).Length();

            if (distL >= 0.5f)
                return false;

            return false;
        }

        public void Update(System.TimeSpan deltaTime)
        {
            var delta = (float)deltaTime.TotalSeconds * 2 * _moveDirection;

            if (ValidatePosition && !_body.ValidPosition(Vector3.Zero))
                _body.Push(new Vector3(2 * _lastDirection, 3, _body.Momentum.Z), true);
            else if (_body != null)
                _lastDirection = _body.Momentum.X > 0? 1 : -1;

            if (!_moving)
                return;



            Object.Translate(new Vector3(0, delta, 0));
            _currentMove += delta;

            if (_currentMove > MaxMove && _moveDirection > 0)
            {
                _moveDirection *= -1;
                if (CanDestroy != null && CanDestroy(_lastCollider))
                    Object.Dispose();
            }
            if (_currentMove < 0 && _moveDirection < 0)
            {
                _moving = false;
                _moveDirection *= -1;
                if (!ValidatePosition)
                    Object.Transform = _originalPosition;
            }
        }
    }
}
