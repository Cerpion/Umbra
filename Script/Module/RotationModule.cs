using Godot;
namespace Umbra.Modules.Rotation
{
    public abstract partial class RotationModule : Node3D
    {
        public VirtualCamera Camera;
        public virtual void SetData(Resource data) { }
        public virtual void OnEnter() { }
        public virtual void OnExit() { }
        public virtual void Update(float delta) { }
    }
}