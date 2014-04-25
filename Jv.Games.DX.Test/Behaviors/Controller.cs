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
        float _spareJumpForce;
        TimeSpan _holdingJump;
        bool _canJump = true;

        public float MinJumpForce = 1;
        public float AdditionalForceDelay = 4;
        public float AdditionalForce = 40;
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
            if (_rigidBody.Momentum.Y > 0)
                return false;

            return !_rigidBody.ValidPosition(new Vector3(0, -0.25f, 0));
        }

        public void Update(TimeSpan deltaTime)
        {
            var state = Keyboard.GetState();

            if (_canJump && IsOnFloor())
            {
                if (state.IsKeyDown(Keys.Space) || state.IsKeyDown(Keys.Up))
                {
                    if (_holdingJump < JumpTimeRange)
                    {
                        _rigidBody.Push(new Vector3(0, MinJumpForce, 0), true, true);
                        _spareJumpForce = AdditionalForceDelay;
                        _holdingJump += deltaTime;
                    }
                }
                else
                    _holdingJump = TimeSpan.Zero;
            }
            else if (state.IsKeyDown(Keys.Space) || state.IsKeyDown(Keys.Up))
            {
                _holdingJump += deltaTime;
                if (_spareJumpForce > 0 && _rigidBody.Momentum.Y > 0)
                {
                    var addToJump = (float)deltaTime.TotalSeconds * AdditionalForce;
                    _spareJumpForce -= addToJump;
                    if (_spareJumpForce < 0)
                        _spareJumpForce = 0;
                    else
                        this._rigidBody.Push(new Vector3(0, addToJump, 0), true, true);
                }
            }
            else
            {
                _holdingJump = TimeSpan.Zero;
                _spareJumpForce = 0;
                _canJump = true;
            }

            var move = state.IsKeyDown(Keys.ShiftKey) ? RunningForce : MoveForce;

            if (state.IsKeyDown(Keys.Right))
                _rigidBody.Push(new SharpDX.Vector3(move, 0, 0));

            if (state.IsKeyDown(Keys.Left))
                _rigidBody.Push(new SharpDX.Vector3(-move, 0, 0));
        }
    }
}
