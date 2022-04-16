using Godot;

namespace MP.Extensions
{
    public static class Vector3Extensions
    {
        public static Vector3 ProjectOnPlane(this Vector3 me, Vector3 planeNormal)
        {
            return new Plane(planeNormal, 0).Project(me);
        }
        
        public static Vector3 NormalizedClamp(this Vector3 me, float length)
        {
            var lengthSquared = me.LengthSquared();
            if (lengthSquared < length * length)
                return me;
            return me.Normalized() * length;
        }

        public static Vector3 Clamp(this Vector3 me, float maxLength, float minLength)
        {
            var lengthSquared = me.LengthSquared();
            if (lengthSquared < maxLength * maxLength && lengthSquared > minLength * minLength)
                return me;

            if (me.x > maxLength)
                me.x = maxLength;
            if (me.z > maxLength)
                me.z = maxLength;
            if (me.y > maxLength)
                me.y = maxLength;

            if (me.x < minLength)
                me.x = minLength;
            if (me.z < minLength)
                me.z = minLength;
            if (me.y < minLength)
                me.y = minLength;

            return me;
        }

        public static Vector3 GetVectorsRotation(this Vector3 from, Vector3 to)
        {
            var dot = from.Dot(to);
            var cross = from.Cross(to);
            var length_product = from.Length() * to.Length();
            if (length_product == 0)
            {
                GD.PushError("Cannot calculate rotation from or to ZERO vector");
                return from;
            }
            var cos_fi = dot / length_product;
            var sin_fi = cross.Length() / length_product;

            var fi = Mathf.Atan2(sin_fi, cos_fi);
            return cross.Normalized() * fi;
        }

        public static float Angle(this Vector3 from, Vector3 to)
        {
            var dot = from.Dot(to);
            var angle = Mathf.Acos(dot);
            return angle;
        }

    }
}