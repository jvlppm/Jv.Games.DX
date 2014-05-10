using Jv.Games.DX.Components;
using Jv.Games.DX.Test.Objects;
using SharpDX;
using System;
using System.Linq;

namespace Jv.Games.DX.Test.Behaviors
{
    class WalkToCastleAfterPoll : Components.Component, IUpdateable
    {
        enum State
        {
            Walking,
            LookingToCamera,
            EnteringCastle
        }

        bool _walking;
        RigidBody _body;
        Collider _collider;
        
        GameObject _castle;
        TimeSpan _delayCount;
        State _state;

        public float Speed = 2;
        public TimeSpan LookToScreenDelay = TimeSpan.FromSeconds(1);

        public override void Init()
        {
            _body = Object.SearchComponent<RigidBody>();
            _collider = Object.SearchComponent<Collider>();

            base.Init();
        }

        public void OnPollSlideComplete()
        {
            _state = State.Walking;
            _walking = true;
            _body.Momentum = Vector3.Zero;

            _castle = Object.GetAncestor<Scene>().SearchObjectsRecursively((Sprite s) => s.Name == "castle").FirstOrDefault();
        }

        public void Update(TimeSpan deltaTime)
        {
            if (!_walking)
                return;

            switch(_state)
            {
                case State.Walking:
                    if (Object.GlobalTransform.TranslationVector.X < _castle.GlobalTransform.TranslationVector.X)
                        _body.Push(new Vector3(Speed, 0, 0));
                    else {
                        Object.SearchComponent<LookForward>().LookTo(new Vector3(0, 0, 1));
                        _body.Momentum = Vector3.Zero;
                        _delayCount = TimeSpan.Zero;
                        _state = State.LookingToCamera;
                    }
                    break;
                case State.LookingToCamera:
                    _delayCount += deltaTime;
                    if (_delayCount > LookToScreenDelay)
                    {
                        Object.SearchComponent<LookForward>().LookTo(new Vector3(0, 0, -1));
                        _state = State.EnteringCastle;
                    }
                    break;
                case State.EnteringCastle:
                    if (Object.GlobalTransform.TranslationVector.Z < _castle.GlobalTransform.TranslationVector.Z + 0.5f)
                    {
                        _body.Push(new Vector3(0, 0, Speed));
                        Object.SearchComponent<LookForward>().LookTo(new Vector3(0, 0, -1));
                    }
                    else
                    {
                        _body.Momentum = Vector3.Zero;
                        Object.GetAncestor<Scene>().SendMessage("OnLevelComplete", false);
                        _walking = false;
                    }
                    break;
            }
        }

        public void OnDeath()
        {
            _walking = false;
        }
    }
}
