using Godot;
using Umbra.Data;

namespace Umbra.Modules.Position
{
    public partial class ThirdPersonModule : PositionModule
    {
        private ThirdPersonData _data;

        private SpringArm3D _sideSpringArm;
        private SpringArm3D _backSpringArm;
        private Marker3D _cameraMarker;

        public override void SetData(Resource data)
        {
            _data = (ThirdPersonData)data;
        }

        public override void BuildRig()
        {
            _sideSpringArm = new SpringArm3D
            {
                RotationDegrees = new Vector3(0, 90, 0),
                CollisionMask = _data.CollisionMask
            };

            AddChild(_sideSpringArm);


            _backSpringArm = new SpringArm3D
            {
                RotationDegrees = new Vector3(0, -90, 0),
                CollisionMask = _data.CollisionMask
            };

            _sideSpringArm.AddChild(_backSpringArm);


            _cameraMarker = new Marker3D();
            _backSpringArm.AddChild(_cameraMarker);
        }
        public override void Refresh()
        {
            _sideSpringArm.Shape = _data.ShapeArm;
            _backSpringArm.Shape = _data.ShapeArm;

            _cameraMarker.Position = new Vector3(_data.ShoulderOffsetX, _data.VerticalOffset, _data.DistanceToTarget);
        }

        public override void Update(float delta)
        {
            if (Camera.Target == null)
            {
                return;
            }

            _sideSpringArm.SpringLength = _data.ShoulderOffsetX;
            _backSpringArm.SpringLength = _data.DistanceToTarget;

            var target = Camera.Target;

            var finalPosition = target.GlobalPosition + (Vector3.Up * _data.VerticalOffset);
            var smoothPosition = Camera.GlobalPosition.Lerp(finalPosition, _data.FollowSpeed * delta);
            Camera.GlobalPosition = smoothPosition;



            Camera.GlobalRotation = target.GlobalRotation;

            Camera.DesiredPosition = _cameraMarker.GlobalPosition;
            Camera.DesiredRotation = _cameraMarker.GlobalRotation;
        }

    }
}