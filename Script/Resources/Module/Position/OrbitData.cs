using Godot;

namespace Umbra.Data
{
    [Tool]
    [GlobalClass]
    public partial class OrbitData : Resource
    {
        [Export] public Shape3D ShapeArm;
        [Export(PropertyHint.Layers3DPhysics)] public uint CollisionMask;


        private float _distanceToTarget = 3;
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

        [Export] public float FollowSpeed = 5f;

        [Export] public Vector3 Offset;

        [Export] public Vector2 Hrange = new Vector2(-80, 80);

        private float _horizontalValue = 0;
        [Export]
        public float HorizontalValue
        {
            get => _horizontalValue;
            set
            {
                _horizontalValue = value;
                EmitChanged();
            }
        }

        public float _verticalValue = 0;
        [Export]
        public float VerticalValue
        {
            get => _verticalValue;
            set
            {
                _verticalValue = value;
                EmitChanged();
            }
        }
    }
}