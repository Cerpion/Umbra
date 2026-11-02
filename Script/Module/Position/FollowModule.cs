using Godot;
using Umbra.Data;

namespace Umbra.Modules.Position
{
    public partial class FollowModule : PositionModule
    {
        private FollowData _data;

        public override void SetData(Resource data)
        {
            _data = (FollowData)data;
        }

        public override void Update(float delta)
        {
            if (Camera.Target == null)
            {
                return;
            }

            var targetPosition = Camera.Target.GlobalPosition + _data.FollowOffset;

            Camera.GlobalPosition = Camera.GlobalPosition.Lerp(targetPosition, _data.FollowSpeed * delta);
            Camera.DesiredPosition = Camera.GlobalPosition;
        }
    }
}