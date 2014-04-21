using SharpDX;

namespace Jv.Games.DX
{
    public class Camera : GameObject
    {
        public Viewport Viewport;
        public Matrix View;
        public Matrix Projection;

        public Camera()
        {
            View = Matrix.Identity;
            Projection = Matrix.Identity;
        }

        public void SetPerspective(float fov, float aspect, float near, float far)
        {
            Projection = Matrix.PerspectiveFovLH(MathUtil.DegreesToRadians(fov), aspect, near, far);
        }
    }
}
