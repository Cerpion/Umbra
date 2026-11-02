using Godot;
namespace Umbra.Modules.Position
{
    public partial class NoneModule : PositionModule
    {
        public override void Update(float delta)
        {
            Camera.DesiredPosition = Camera.GlobalPosition;
        }
    }
}