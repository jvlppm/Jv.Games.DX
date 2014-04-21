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
        bool _canJump = true;

        public float MinJumpForce = 1;
        public float AdditionalForceDelay = 4;
        public float AdditionalForce = 40;
        public float MoveForce = 1;


        public override void Init()
        {
            _rigidBody = Object.SearchComponent<RigidBody>();
            base.Init();
        }

        bool IsOnFloor()
        {
            if (_rigidBody.Momentum.Y != 0)
                return false;

            var oldTransform = Object.Transform;
            Object.Translate(0, -0.25f, 0);
            var result = !_rigidBody.ValidPosition();
            Object.Transform = oldTransform;
            return result;
        }

        public void Update(TimeSpan deltaTime)
        {
            var state = Keyboard.GetState();

            if (_canJump && IsOnFloor())
            {
                if (state.IsKeyDown(Keys.Space))
                {
                    _rigidBody.Push(new Vector3(0, MinJumpForce, 0), true, true);
                    _spareJumpForce = AdditionalForceDelay;
                    _canJump = false;
                }
            }
            else if (state.IsKeyDown(Keys.Space))
            {
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
                _spareJumpForce = 0;
                _canJump = true;
            }

            if (state.IsKeyDown(Keys.Right))
                _rigidBody.Push(new SharpDX.Vector3(MoveForce, 0, 0));

            if (state.IsKeyDown(Keys.Left))
                _rigidBody.Push(new SharpDX.Vector3(-MoveForce, 0, 0));
        }
    }
}
