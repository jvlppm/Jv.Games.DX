using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jv.Games.DX
{
    public class Camera : GameObject
    {
        IDisposable _sceneRegistration;
        public Viewport Viewport;
        public Matrix View;
        public Matrix Projection;

        public Camera()
        {
            View = Matrix.Identity;
            Projection = Matrix.Identity;
        }

        public override void Init()
        {
            if (_sceneRegistration != null)
                throw new InvalidOperationException("The camera is already registered in a scene.");

            _sceneRegistration = GetParent<Scene>().Register(this);
            base.Init();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _sceneRegistration.Dispose();
            base.Dispose(disposing);
        }

        public void SetPerspective(float fov, float aspect, float near, float far)
        {
            Projection = Matrix.PerspectiveFovLH(MathUtil.DegreesToRadians(fov), aspect, near, far);
        }
    }
}
