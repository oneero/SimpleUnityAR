using SVector3 = System.Numerics.Vector3;
using UVector3 = UnityEngine.Vector3;

public static class TypeExtensions
{
    /**
     * Extension methods for converting between Unity Vector3 and System.Numerics.Vector3.
     */
    public static SVector3 ToSystem(this UVector3 vector) => new SVector3(vector.x, vector.y, vector.z);
    public static UVector3 ToUnity(this SVector3 vector) => new UVector3(vector.X, vector.Y, vector.Z);
}
