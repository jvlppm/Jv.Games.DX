using Jv.Games.DX.Components;
using Mage;
using Microsoft.Xna.Framework.Input;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jv.Games.DX.Test.Behaviors
{
    class Controller : Components.Component, IUpdateable
    {
        RigidBody _rigidBody;
        TimeSpan _holdingJump;
        TimeSpan _jumpTime;
        bool _canJump;

        public TimeSpan MaxJumpTime = TimeSpan.FromSeconds(0.2);
        public float JumpSpeed = 2.2f;
        public float MoveForce = 3;
        public float RunningForce = 5;
        public TimeSpan JumpTimeRange = TimeSpan.FromSeconds(0.2);


        public override void Init()
        {
            _rigidBody = Object.SearchComponent<RigidBody>();
            base.Init();
        }

        bool IsOnFloor()
        {
            if (_rigidBody.Momentum.Y != 0)
                return false;

            return !_rigidBody.ValidPosition(new Vector3(0, -0.05f, 0));
        }

        KeyboardState? _oldState;

        public void Update(TimeSpan deltaTime)
        {
            var state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Space) || state.IsKeyDown(Keys.Up))
                _holdingJump += deltaTime;
            else
            {
                _canJump = false;
                _holdingJump = TimeSpan.Zero;
            }

            if (IsOnFloor())
            {
                _canJump = true;
                _jumpTime = TimeSpan.Zero;
                if (_holdingJump > TimeSpan.Zero && _holdingJump < JumpTimeRange)
                {
                    _rigidBody.InstantAcceleration *= new Vector3(1, 0, 1);
                    _rigidBody.Momentum = new Vector3(_rigidBody.Momentum.X, JumpSpeed, _rigidBody.Momentum.Z);
                    _jumpTime += deltaTime;
                }
            }
            else if (_canJump)
            {
                if (_rigidBody.Momentum.Y < JumpSpeed / 2)
                    _canJump = false;
                else if (_jumpTime > TimeSpan.Zero && _jumpTime < MaxJumpTime)
                {
                    _rigidBody.InstantAcceleration *= new Vector3(1, 0, 1);
                    _rigidBody.Momentum = new Vector3(_rigidBody.Momentum.X, JumpSpeed, _rigidBody.Momentum.Z);
                    _jumpTime += deltaTime;
                }
            }

            var move = state.IsKeyDown(Keys.ShiftKey) ? RunningForce : MoveForce;

            if (state.IsKeyDown(Keys.Right))
                _rigidBody.Push(new SharpDX.Vector3(move, 0, 0));

            //if (_oldState != null && state.IsKeyDown(Keys.Z) && !_oldState.Value.IsKeyDown(Keys.Z))
            //    JumpSpeed += 0.1f;
            //if (_oldState != null && state.IsKeyDown(Keys.X) && !_oldState.Value.IsKeyDown(Keys.X))
            //    JumpSpeed -= 0.1f;

            //if (_oldState != null && state.IsKeyDown(Keys.A) && !_oldState.Value.IsKeyDown(Keys.A))
            //    MaxJumpTime += TimeSpan.FromSeconds(0.1);
            //if (_oldState != null && state.IsKeyDown(Keys.S) && !_oldState.Value.IsKeyDown(Keys.S))
            //    MaxJumpTime -= TimeSpan.FromSeconds(0.1);



            if (state.IsKeyDown(Keys.Left))
                _rigidBody.Push(new SharpDX.Vector3(-move, 0, 0));

            _oldState = state;
        }
    }
}
