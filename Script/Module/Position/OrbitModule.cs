using Godot;
using Umbra.Data;

namespace Umbra.Modules.Position
{
    public partial class OrbitModule : PositionModule
    {
        private OrbitData _data;

        public float Hvalue { get; set; }
        public float Vvalue { get; set; }


        private SpringArm3D _backSpringArm;
        private Marker3D _cameraMarket;

        public override void SetData(Resource data)
        {
            _data = (OrbitData)data;
        }

        public override void BuildRig()
        {
            _backSpringArm = new SpringArm3D();
            _backSpringArm.CollisionMask = _data.CollisionMask;
            AddChild(_backSpringArm);

            _cameraMarket = new Marker3D();
            _backSpringArm.AddChild(_cameraMarket);
        }

        public override void Refresh()
        {
            _backSpringArm.Shape = _data.ShapeArm;
            _cameraMarket.Position = new Vector3(0, 0, _data.DistanceToTarget);
        }

        public override void Update(float delta)
        {
            if (Camera.Target == null)
            {
                return;
            }

            _backSpringArm.SpringLength = _data.DistanceToTarget;

            var finalPosition = Camera.Target.GlobalPosition + _data.Offset;
            var smoothPosition = Camera.GlobalPosition.Lerp(finalPosition, _data.FollowSpeed * delta);
            Camera.GlobalPosition = smoothPosition;

            _data.HorizontalValue = Mathf.Clamp(_data.HorizontalValue, Mathf.DegToRad(_data.Hrange.X), Mathf.DegToRad(_data.Hrange.Y));
            _data.VerticalValue = Mathf.Wrap(_data.VerticalValue, -Mathf.Pi, Mathf.Pi);

            Camera.Rotation = new Vector3(_data.HorizontalValue, _data.VerticalValue, 0);
            Camera.DesiredPosition = _cameraMarket.GlobalPosition;
        }
    }
}