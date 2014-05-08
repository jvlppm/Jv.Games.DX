using Jv.Games.DX.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jv.Games.DX.Test.Behaviors
{
    class MainPlayerDeath : Components.Component
    {
        class AnimationController : Component, IUpdateable
        {
            enum State
            {
                WaitingDelay,
                DelayCompleted,
                WaitingUpAndDown,
                DeathFinished
            }

            State _state;

            public event EventHandler OnDeathFinalized;
            TimeSpan _delay = TimeSpan.FromSeconds(0.2);
            GameObject _deadObject;
            float _jumpForce;

            public AnimationController(GameObject deadObject, TimeSpan delay, float jumpForce)
            {
                _deadObject = deadObject;
                _delay = delay;
                _jumpForce = jumpForce;

                _deadBody = new RigidBody { Object = deadObject };
                _gravity = new Gravity { Object = _deadObject, RigidBody = _deadBody };
            }

            TimeSpan _currentDelay;
            RigidBody _deadBody;
            Gravity _gravity;
            float _deathHeight;

            public void Update(TimeSpan deltaTime)
            {
                switch (_state)
                {
                    case State.WaitingDelay:
                        {
                            _currentDelay += deltaTime;
                            if (_currentDelay <= _delay)
                                return;

                            _state = State.DelayCompleted;
                        }
                        break;
                    case State.DelayCompleted:
                        _deadBody.Push(new SharpDX.Vector3(0, _jumpForce, 0), true, true);
                        _state = State.WaitingUpAndDown;
                        _deathHeight = _deadObject.GlobalTransform.TranslationVector.Y - 5;
                        break;

                    case State.WaitingUpAndDown:
                        _gravity.Update(deltaTime);
                        _deadBody.Update(deltaTime);
                        if (_deadObject.GlobalTransform.TranslationVector.Y <= _deathHeight)
                            _state = State.DeathFinished;
                        break;

                    case State.DeathFinished:
                        OnDeathFinalized(this, EventArgs.Empty);
                        break;
                }
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    if (_deadBody != null)
                    {
                        _deadBody.Dispose();
                        _deadBody = null;
                    }
                    if (_gravity != null)
                    {
                        _gravity.Dispose();
                        _gravity = null;
                    }
                }
                base.Dispose(disposing);
            }
        }

        bool _dead;
        AnimationController _controller;

        public event EventHandler OnDeathStarted;
        public event EventHandler OnDeathFinalized;
        public float JumpForce = 4f;
        public TimeSpan Delay = TimeSpan.FromSeconds(0.2);

        public void OnDeath()
        {
            if (_dead)
                return;

            Object.SearchComponent<Collider>().IsTrigger = true;
            Object.SearchComponent<RigidBody>().Momentum = new SharpDX.Vector3(0, 0, 1);
            Object.SearchComponent<LookForward>().Update(TimeSpan.Zero);
            Object.SearchComponent<RigidBody>().Momentum = SharpDX.Vector3.Zero;


            if (OnDeathStarted != null)
                OnDeathStarted(this, EventArgs.Empty);

            _dead = true;

            Object.Enabled = false;
            _controller = new AnimationController(Object, Delay, JumpForce);
            _controller.OnDeathFinalized += controller_OnDeathFinalized;
            Object.GetAncestor<Scene>().Add(_controller);
            _controller.Init();
        }

        public void Reset()
        {
            _controller.Dispose();
            _controller = null;
            _dead = false;
        }

        void controller_OnDeathFinalized(object sender, EventArgs e)
        {
            Object.SearchComponent<Collider>().IsTrigger = false;

            if (OnDeathFinalized != null)
                OnDeathFinalized(this, EventArgs.Empty);

            if (_controller != null)
                _controller.Dispose();
        }

        protected override void Dispose(bool disposing)
        {
            if (_controller != null)
            {
                _controller.Dispose();
                _controller = null;
            }
        }
    }
}
