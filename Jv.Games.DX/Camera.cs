using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jv.Games.DX
{
    class Camera : GameObject
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

        public void SetPerspective(float fovy, float aspect, float near, float far)
        {
            var top = near * (float)Math.Tan(fovy * Math.PI / 360.0);
            var right = top * aspect;
            Projection = Frustum(-right, right, -top, top, near, far);
        }

        static Matrix Frustum(float left, float right, float bottom, float top, float zNear, float zFar)
        {
            var zDelta = (zFar - zNear);
            var dir = (right - left);
            var height = (top - bottom);
            var zNear2 = 2 * zNear;

            return new Matrix(new[]{
                2 * zNear / dir, 0, (right + left) / dir, 0,
                0, zNear2 / height, (top + bottom) / height, 0,
                0, 0, -(zFar + zNear) / zDelta, -zNear2 * zFar / zDelta,
                0, 0, -1, 0
            });
        }
    }
}
