using Godot;
using static Godot.Camera3D;

namespace Umbra.Data
{
    [Tool]
    [GlobalClass]
    public partial class LenData : Resource
    {
        private float _fov = 75;
        [Export]
        public float FOV
        {
            get => _fov;
            set
            {
                _fov = value;
                EmitChanged();
            }
        }


        private float _near = 0.05f;
        [Export]
        public float Near
        {
            get => _near;
            set
            {
                _near = value;
                EmitChanged();
            }
        }


        private float _far = 1000.0f;
        [Export]
        public float Far
        {
            get => _far;
            set
            {
                _far = value;
                EmitChanged();
            }
        }


        private ProjectionType _projection = ProjectionType.Perspective;
        [Export]
        public ProjectionType Projection
        {
            get => _projection;
            set
            {
                _projection = value;
                EmitChanged();
            }
        }
    }
}