using SharpDX;

namespace Jv.Games.DX.Test.Behaviors
{
    class Gravity : Mover
    {
        public Gravity()
        {
            Direction = new Vector3(0, -9.8f, 0);
            Acceleration = true;
            Continuous = true;
        }
    }
}
