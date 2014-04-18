using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jv.Games.DX.Test.Behaviors
{
    class LookAtObject : Components.Component
    {
        Camera _camera;
        GameObject _target;

        public LookAtObject(GameObject target)
        {
            _target = target;
        }

        public override void Init()
        {
            _camera = (Camera)Object;
            base.Init();
        }

        public override void Update(TimeSpan deltaTime)
        {
            var eye = _camera.GlobalTransform.TranslationVector;
            var center = _target.GlobalTransform.TranslationVector;
            var up = new Vector3(0, 1, 0);

            _camera.View = Matrix.LookAtLH(eye, center, up);
            base.Update(deltaTime);
        }
    }
}
