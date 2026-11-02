using Godot;
using Umbra.Data;
using Umbra.Enums;

namespace Umbra.Modules.Position
{
    public partial class SplineDollyModule : PositionModule
    {
        private SplineDollyData _data;
        private Path3D _path;

        private float _progress;
        private SplineMovement _lastMovement;

        public override void SetData(Resource data)
        {
            _data = (SplineDollyData)data;
        }

        public override void OnEnter()
        {
            _path = Camera.GetNodeOrNull<Path3D>(_data.PathNodePath);

            if (_path?.Curve == null)
            {
                GD.PushWarning("SplineDolly: no se encontró el Path3D.");
                return;
            }

            float length = _path.Curve.GetBakedLength();

            _progress = Mathf.Clamp(_data.Position, 0f, length);
            _lastMovement = _data.SplineMovement;
        }

        public override void OnExit()
        {
            Camera.DollyRotation = false;
            _path = null;
            Camera = null;
        }

        public override void Update(float delta)
        {
            if (Camera == null || _path?.Curve == null)
                return;

            Curve3D curve = _path.Curve;
            float length = curve.GetBakedLength();

            if (length <= 0f)
                return;

            if (_lastMovement != _data.SplineMovement)
            {
                if (_data.SplineMovement == SplineMovement.Manual)
                    _data.Position = _progress;

                _lastMovement = _data.SplineMovement;
            }

            switch (_data.SplineMovement)
            {
                case SplineMovement.Manual:
                    _progress = Mathf.Clamp(_data.Position, 0f, length);
                    break;

                case SplineMovement.Automatic:
                    _progress = Mathf.Clamp(_progress + _data.Speed * delta, 0f, length);
                    break;

                case SplineMovement.NearestPointToTarget:
                    UpdateNearestPoint(curve, delta, length);
                    break;
            }

            UpdateCameraTransform(curve);
        }

        private void UpdateNearestPoint(Curve3D curve, float delta, float length)
        {
            if (Camera.Target == null)
                return;

            Vector3 localTargetPosition = _path.ToLocal(Camera.Target.GlobalPosition);

            float targetProgress = curve.GetClosestOffset(localTargetPosition);

            targetProgress = Mathf.Clamp(targetProgress, 0f, length);

            if (_data.FollowSpeed <= 0f)
            {
                _progress = targetProgress;
                return;
            }

            float weight = Mathf.Clamp(_data.FollowSpeed * delta, 0f, 1f);
            _progress = Mathf.Lerp(_progress, targetProgress, weight);
        }

        private void UpdateCameraTransform(Curve3D curve)
        {
            Transform3D splineTransform = curve.SampleBakedWithRotation(_progress, true, true);
            Vector3 localPosition = splineTransform.Origin + splineTransform.Basis * _data.SplineOffset;
            Camera.DesiredPosition = _path.ToGlobal(localPosition);

            if (_data.CameraRotation == SplineRotation.Default)
            {
                Camera.DollyRotation = false;
                return;
            }

            Transform3D worldTransform = _path.GlobalTransform * splineTransform;
            Basis splineBasis = worldTransform.Basis.Orthonormalized();
            Vector3 forward = -splineBasis.Z.Normalized();

            if (_data.CameraRotation == SplineRotation.Spline)
            {
                Vector3 up = splineBasis.Y.Normalized();
                Camera.DesiredRotation = Basis.LookingAt(forward, up).GetEuler();
                Camera.DollyRotation = true;
            }
            else
            {
                Camera.DollyRotation = true;
                Vector3 horizontalForward = new Vector3(forward.X, 0f, forward.Z);

                if (horizontalForward.LengthSquared() < 0.0001f)
                    return;

                horizontalForward = horizontalForward.Normalized();
                Camera.DesiredRotation = Basis.LookingAt(horizontalForward, Vector3.Up).GetEuler();
            }
        }
    }
}