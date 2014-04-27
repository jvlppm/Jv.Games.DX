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
        float _startJumpHeight;
        float _maxJumpHeight;

        public float JumpSpeed = 4;
        public float MinJumpHeight = 0;
        public float MaxJumpHeight = 2;
        public float AdditionalForceDelay = 4;
        public float AdditionalForce = 2;
        public float MoveForce = 20;
        public float RunningForce = 30;
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

        public void Update(TimeSpan deltaTime)
        {
            var state = Keyboard.GetState();

            System.Diagnostics.Debug.WriteLine(_holdingJump);

            if (state.IsKeyDown(Keys.Space) || state.IsKeyDown(Keys.Up))
                _holdingJump += deltaTime;
            else
            {
                _holdingJump = TimeSpan.Zero;
                _maxJumpHeight = 0;
            }

            if (IsOnFloor())
            {
                if (state.IsKeyDown(Keys.Space) || state.IsKeyDown(Keys.Up))
                {
                    if (_holdingJump < JumpTimeRange && _maxJumpHeight == 0)
                    {
                        _rigidBody.Momentum.Y = 0;
                        _rigidBody.Push(new Vector3(0, JumpSpeed, 0), true);
                        _startJumpHeight = Object.Transform.TranslationVector.Y;
                    }
                }
            }
            else
            {
                var jumpHeight = Object.Transform.TranslationVector.Y - _startJumpHeight;
                if (_rigidBody.Momentum.Y < JumpSpeed / 2)
                    _maxJumpHeight = MaxJumpHeight;

                if (jumpHeight > _maxJumpHeight)
                    _maxJumpHeight = jumpHeight;

                if (_maxJumpHeight > 0 && (_maxJumpHeight < MinJumpHeight || ((state.IsKeyDown(Keys.Space) || state.IsKeyDown(Keys.Up)) && _maxJumpHeight < MaxJumpHeight)))
                {
                    _rigidBody.Momentum.Y = JumpSpeed;
                }
                else
                    _maxJumpHeight = 0;
            }

            var move = state.IsKeyDown(Keys.ShiftKey) ? RunningForce : MoveForce;

            if (state.IsKeyDown(Keys.Right))
                _rigidBody.Push(new SharpDX.Vector3(move, 0, 0));

            if (state.IsKeyDown(Keys.Left))
                _rigidBody.Push(new SharpDX.Vector3(-move, 0, 0));
        }
    }
}
