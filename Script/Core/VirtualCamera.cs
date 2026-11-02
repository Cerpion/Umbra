using Umbra.Data;
using Umbra.Enums;
using Godot;
using System;
using Umbra.Modules.Position;
using Umbra.Modules.Rotation;

namespace Umbra
{
    [Tool]
    public partial class VirtualCamera : Node3D
    {
        public bool IsActive;
        public Vector3 DesiredPosition;
        public Vector3 DesiredRotation;

        [Signal] public delegate void PriorityChangedEventHandler();
        [Export] public VirtualCameraConfig Data;


        private int _priority;
        [Export]
        public int Priority
        {
            get => _priority;
            set
            {
                if (_priority == value) return;
                _priority = Mathf.Max(0, value);

                if (!IsNodeReady()) { return; }

                EmitSignal(SignalName.PriorityChanged);
            }
        }
        [Export] public Node3D Target { set; get; }
        [Export] public Node3D Follow { set; get; }
        [Export] public LenData Len { private set; get; }



        [ExportCategory("Position Module")]
        private PositionModuleType _positionModuleType = PositionModuleType.None;
        [Export]
        public PositionModuleType PositionModuleType
        {
            get => _positionModuleType;
            set
            {
                if (_positionModuleType == value)
                    return;

                _positionModuleType = value;
                LoadPositionModule();
            }
        }

        public PositionModule PositionModule;
        [Export] public Resource CurrentPositionData;



        [ExportCategory("Rotation Module")]
        private RotationModuleType _rotationModuleType = RotationModuleType.None;
        [Export]
        public RotationModuleType RotationModuleType
        {
            get => _rotationModuleType;
            set
            {
                if (_rotationModuleType == value)
                    return;

                _rotationModuleType = value;
                LoadRotationModule();
            }
        }

        public bool DollyRotation { get; internal set; }
        public RotationModule RotationModule;

        public override void _EnterTree()
        {
            if (Data == null)
            {
                Data = new VirtualCameraConfig();
                Data.Initialize();
            }

            Len = Data.LenData;

            LoadPositionModule();
            LoadRotationModule();

            (Engine.GetSingleton(nameof(UmbraManager)) as UmbraManager).SubscribeVirtualCamera(this);
        }

        public override void _ExitTree()
        {
            PositionModule?.OnExit();
            PositionModule?.QueueFree();
            PositionModule = null;

            RotationModule?.OnExit();
            RotationModule?.QueueFree();
            RotationModule = null;

            (Engine.GetSingleton(nameof(UmbraManager)) as UmbraManager)?.UnsubscribeVirtualCamera(this);
        }

        private void LoadPositionModule()
        {
            if (Data == null || !IsInsideTree()) return;

            PositionModule?.OnExit();
            PositionModule?.QueueFree();

            PositionModule = _positionModuleType switch
            {
                PositionModuleType.None => new NoneModule(),
                PositionModuleType.Orbit => new OrbitModule(),
                PositionModuleType.ThirdPerson => new ThirdPersonModule(),
                PositionModuleType.Follow => new FollowModule(),
                PositionModuleType.SplineDolly => new SplineDollyModule(),
                _ => throw new NotImplementedException(),
            };


            CurrentPositionData = Data.GetPositionDataByID(_positionModuleType);

            PositionModule.Camera = this;
            PositionModule.SetData(CurrentPositionData);
            AddChild(PositionModule);
            PositionModule.BuildRig();
            PositionModule.OnEnter();
        }

        private void LoadRotationModule()
        {
            if (Data == null || !IsInsideTree()) return;


            var rotationModule = Data.GetRotationModuleByID(_rotationModuleType);

            RotationModule?.OnExit();
            RotationModule?.QueueFree();

            RotationModule = _rotationModuleType switch
            {
                RotationModuleType.None => new NoneRotation(),
                RotationModuleType.HardLook => new HardLookAtRotation(),
                _ => throw new NotImplementedException(),
            };


            RotationModule.Camera = this;
            RotationModule.SetData(rotationModule);
            AddChild(RotationModule);
            RotationModule.OnEnter();
        }

        public override void _Process(double delta)
        {
            if (Engine.IsEditorHint())
            {
                UpdateGizmos();

                PositionModule?.Update((float)delta);
                PositionModule?.Refresh();

                if (!DollyRotation)
                    RotationModule?.Update((float)delta);
                return;
            }


            if (!IsActive)
                return;

            PositionModule?.Update((float)delta);

            if (!DollyRotation)
                RotationModule?.Update((float)delta);
        }
    }
}