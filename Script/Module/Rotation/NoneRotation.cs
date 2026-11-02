using Godot;

namespace Umbra.Modules.Rotation
{
    public partial class NoneRotation : RotationModule
    {
        public override void Update(float delta)
        {
            Camera.DesiredRotation = Camera.Rotation;
        }
    }
}