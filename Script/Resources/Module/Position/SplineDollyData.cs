using Godot;
using Umbra.Enums;

namespace Umbra.Data
{
    [Tool]
    [GlobalClass]
    public partial class SplineDollyData : Resource
    {
        [Export] public NodePath PathNodePath { get; set; }
        [Export] public float Position { get; set; }
        [Export] public Vector3 SplineOffset { get; set; }
        [Export] public SplineMovement SplineMovement { get; set; }
        [Export] public SplineRotation CameraRotation { get; set; }
        [Export] public float Speed { get; set; } = 2f;
        [Export] public float FollowSpeed { get; set; } = 2f;
    }
}