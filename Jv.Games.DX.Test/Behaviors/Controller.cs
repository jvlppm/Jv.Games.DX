using Jv.Games.DX.Components;
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

        public float MinJumpForce = 1;
        public float MaxJumpForce = 1;
        public float MoveForce = 1;


        public override void Init()
        {
            _rigidBody = Object.SearchComponent<RigidBody>();
            base.Init();
        }

        public void Update(TimeSpan deltaTime)
        {
            var state = Keyboard.GetState();

            if (Math.Abs(_rigidBody.Momentum.Y) < 0.001 && _spareJumpForce == 0)
            {
                if (state.IsKeyDown(Keys.Space))
                {
                    _rigidBody.Push(new Vector3(0, MinJumpForce, 0), true, true);
                    if (MaxJumpForce > MinJumpForce)
                        _spareJumpForce = MaxJumpForce - MinJumpForce;
                }
            }
            else if (state.IsKeyDown(Keys.Space))
            {
                if (_spareJumpForce > 0 && _rigidBody.Momentum.Y > 0)
                {
                    var addToJump = MinJumpForce * 20 * (float)deltaTime.TotalSeconds;
                    _spareJumpForce -= addToJump;
                    if (_spareJumpForce < 0)
                    {
                        addToJump += _spareJumpForce;
                        _spareJumpForce = 0;
                    }
                    this._rigidBody.Push(new Vector3(0, addToJump, 0), true, true);
                }
            }
            else _spareJumpForce = 0;

            if (state.IsKeyDown(Keys.Right))
                _rigidBody.Push(new SharpDX.Vector3(MoveForce, 0, 0));

            if (state.IsKeyDown(Keys.Left))
                _rigidBody.Push(new SharpDX.Vector3(-MoveForce, 0, 0));
        }
    }
}
