using Godot;
namespace Umbra.Modules.Rotation
{
    public partial class HardLookAtRotation : RotationModule
    {
        public override void Update(float delta)
        {
            if (Camera.Follow == null)
                return;

            Vector3 dir = (Camera.Follow.GlobalPosition - Camera.DesiredPosition).Normalized();

            Camera.DesiredRotation = Basis.LookingAt(dir).GetEuler();
        }
    }
}
