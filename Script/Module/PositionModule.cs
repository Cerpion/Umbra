using Godot;
namespace Umbra.Modules.Position
{
    public abstract partial class PositionModule : Node3D
    {
        public VirtualCamera Camera;
        public virtual void SetData(Resource data) { }
        public virtual void BuildRig() { }
        public virtual void OnEnter() { }
        public virtual void OnExit() { }
        public virtual void Refresh() { }
        public abstract void Update(float delta);
    }
}