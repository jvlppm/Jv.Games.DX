using Jv.Games.DX.Components;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jv.Games.DX.Test.Behaviors
{
    class Rotating : Components.Component, IUpdateable
    {
        float _speed = MathUtil.DegreesToRadians(-45);

        public float DegreesPerSecond
        {
            get { return MathUtil.RadiansToDegrees(_speed); }
            set { _speed = MathUtil.DegreesToRadians(value); }
        }

        Vector3 Axis = new Vector3(0, 1, 0);

        public void Update(TimeSpan deltaTime)
        {
            Object.Transform = Object.Transform * Matrix.RotationAxis(Axis, (float)(_speed * deltaTime.TotalSeconds));
        }
    }
}
