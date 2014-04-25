using Jv.Games.DX.Components;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jv.Games.DX.Test.Behaviors
{
    class LookAtObject : Components.Component, IUpdateable
    {
        Camera _camera;
        public GameObject Target;

        public LookAtObject(GameObject target)
        {
            Target = target;
        }

        public override void Init()
        {
            _camera = (Camera)Object;
            base.Init();
        }

        public void Update(TimeSpan deltaTime)
        {
            var eye = _camera.GlobalTransform.TranslationVector;
            var center = Target.GlobalTransform.TranslationVector;
            var up = new Vector3(0, 1, 0);

            _camera.View = Matrix.LookAtLH(eye, center, up);
        }
    }
}
