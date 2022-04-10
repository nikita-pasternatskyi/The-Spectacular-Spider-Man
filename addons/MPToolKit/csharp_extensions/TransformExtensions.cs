using Godot;


public static class TransformExtensions
{

    public static Transform LookAtSmooth(this Transform me, Vector3 targetVector, Vector3 upVector, float smooth)
    {

        var newTransform = me.LookingAt(targetVector, upVector);
        var oldTransform = me;

        oldTransform.basis.x = oldTransform.basis.x.LinearInterpolate(newTransform.basis.x, smooth);
        oldTransform.basis.y = oldTransform.basis.y.LinearInterpolate(newTransform.basis.y, smooth);
        oldTransform.basis.z = oldTransform.basis.z.LinearInterpolate(newTransform.basis.z, smooth);

        oldTransform = oldTransform.Orthonormalized();
        me = oldTransform;

        return me;
    }
}
