using Godot;

namespace Umbra
{
    [Tool]
    public partial class CameraBrain : Node
    {
        [Export] public float BlendTime = 0.35f;
        [Export] public Curve BlendCurve = new Curve();

        public Camera3D MainCamera;
        public VirtualCamera VirtualCamera;

        private UmbraManager _umbraController;
        private Vector3 _startPosition;
        private Quaternion _startRotation;
        private float _startFov;
        private float _startNear;
        private float _startFar;

        private float _blend;


        public override string[] _GetConfigurationWarnings()
        {
            if (GetParent() is not Camera3D)
            {
                return ["It needs to be a child of Camera 3D."];
            }

            return [];
        }

        public override void _EnterTree()
        {
            BlendCurve.AddPoint(new Vector2(0, 0));
            BlendCurve.AddPoint(new Vector2(1, 1));

            MainCamera = GetParent() as Camera3D;
            _umbraController = Engine.GetSingleton(nameof(UmbraManager)) as UmbraManager;

            _umbraController.PriorityChange += OnPriorityChange;
            _umbraController.CameraLenChanged += OnCameraLenChange;
        }

        public override void _ExitTree()
        {
            _umbraController.PriorityChange -= OnPriorityChange;
            _umbraController.CameraLenChanged -= OnCameraLenChange;
        }

        private void OnCameraLenChange()
        {
            MainCamera.Fov = VirtualCamera.Len.FOV;
            MainCamera.Near = VirtualCamera.Len.Near;
            MainCamera.Far = VirtualCamera.Len.Far;
            MainCamera.Projection = VirtualCamera.Len.Projection;
        }

        private void OnPriorityChange()
        {
            var best = _umbraController.GetBestCamera();
            if (best == null || best == VirtualCamera)
            {
                return;
            }

            StartBlendTo(best);
        }

        private void StartBlendTo(VirtualCamera next)
        {
            VirtualCamera?.IsActive = false;
            VirtualCamera = next;
            VirtualCamera.IsActive = true;
            _blend = 0;

            _startPosition = MainCamera.GlobalPosition;
            _startRotation = MainCamera.GlobalTransform.Basis.GetRotationQuaternion();

            _startFov = MainCamera.Fov;
            _startNear = MainCamera.Near;
            _startFar = MainCamera.Far;
            MainCamera.Projection = next.Len.Projection;
            //OnCameraLenChange(next.Len);
        }

        public override void _Process(double delta)
        {
            if (VirtualCamera == null)
            {
                return;
            }

            float deltaTime = (float)delta;
            AdvanceBlend(deltaTime);
            ApplyBlendedPose(GetBlendWeight());
        }

        private void AdvanceBlend(float delta)
        {
            if (_blend >= 1f)
                return;

            float progress = delta / BlendTime;
            _blend += progress;

            if (_blend > 1f)
                _blend = 1f;
        }

        private float GetBlendWeight()
        {
            if (_blend >= 1f)
                return 1f;

            if (BlendCurve == null)
                return _blend;

            return BlendCurve.Sample(_blend);
        }

        public void ApplyBlendedPose(float delta)
        {
            var targetRotation = Quaternion.FromEuler(VirtualCamera.DesiredRotation);

            MainCamera.GlobalPosition = _startPosition.Lerp(VirtualCamera.DesiredPosition, delta);
            MainCamera.Quaternion = _startRotation.Slerp(targetRotation, delta);

            var len = VirtualCamera.Len;
            MainCamera.Fov = Mathf.Lerp(_startFov, len.FOV, delta);
            MainCamera.Near = Mathf.Lerp(_startNear, len.Near, delta);
            MainCamera.Far = Mathf.Lerp(_startFar, len.Far, delta);
        }
    }
}