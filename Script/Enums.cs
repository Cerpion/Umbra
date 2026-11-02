namespace Umbra.Enums
{
    public enum PositionModuleType
    {
        None,
        Orbit,
        ThirdPerson,
        Follow,
        SplineDolly
    }

    public enum RotationModuleType
    {
        None,
        HardLook,
    }

    public enum SplineMovement
    {
        Manual,
        Automatic,
        NearestPointToTarget
    }

    public enum SplineRotation
    {
        Default,
        Spline,
        SplineNoRoll
    }

}
