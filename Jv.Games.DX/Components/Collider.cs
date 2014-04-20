using System;

namespace Jv.Games.DX.Components
{
    public abstract class Collider : Component
    {
        public bool IsTrigger;

        public abstract bool Intersects(Collider other);
    }

    public class AxisAlignedBoxCollider : Collider
    {
        public float RadiusWidth = 0.5f;
        public float RadiusHeight = 0.5f;
        public float RadiusDepth = 0.5f;

        public override bool Intersects(Collider other)
        {
            if ((other is AxisAlignedBoxCollider))
                return Intersects((AxisAlignedBoxCollider)other);

            throw new NotImplementedException();
        }

        public bool Intersects(AxisAlignedBoxCollider box)
        {
            var center = Object.GlobalTransform.TranslationVector;
            var otherCenter = box.Object.GlobalTransform.TranslationVector;

            if (Math.Abs(center.X - otherCenter.X) > (RadiusWidth + box.RadiusWidth)) return false;
            if (Math.Abs(center.Y - otherCenter.Y) > (RadiusHeight + box.RadiusHeight)) return false;
            if (Math.Abs(center.Z - otherCenter.Z) > (RadiusDepth + box.RadiusDepth)) return false;
            return true;
        }
    }
}
