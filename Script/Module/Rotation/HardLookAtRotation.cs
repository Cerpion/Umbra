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

            if (dir.LengthSquared() < Mathf.Epsilon)
                return;

            Camera.DesiredRotation = Basis.LookingAt(dir).GetEuler();
        }
    }
}
