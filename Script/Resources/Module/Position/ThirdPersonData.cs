using Godot;

namespace Umbra.Data
{
    [Tool]
    [GlobalClass]
    public partial class ThirdPersonData : Resource
    {
        [Export] public Shape3D ShapeArm;
        [Export(PropertyHint.Layers3DPhysics)] public uint CollisionMask;

        private float _distanceToTarget = 3f;
        [Export]
        public float DistanceToTarget
        {
            get => _distanceToTarget;
            set
            {
                _distanceToTarget = value;
                EmitChanged();
            }
        }


        private float _distanceSide = 0.5f;
        [Export]
        public float ShoulderOffsetX
        {
            get => _distanceSide;
            set
            {
                _distanceSide = value;
                EmitChanged();
            }
        }


        private float _verticalOffset = 0f;
        [Export]
        public float VerticalOffset
        {
            get => _verticalOffset;
            set
            {
                _verticalOffset = value;
                EmitChanged();
            }
        }

        [Export] public float FollowSpeed = 5f;
    }
}