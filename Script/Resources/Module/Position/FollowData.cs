using Godot;

namespace Umbra.Data
{
    [Tool]
    [GlobalClass]
    public partial class FollowData : Resource
    {
        [Export] public float FollowSpeed = 5f;
        [Export] public Vector3 FollowOffset;
    }
}