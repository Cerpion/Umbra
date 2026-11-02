using Godot;
using Umbra.Enums;

namespace Umbra.Data
{
    [Tool]
    [GlobalClass]
    public partial class VirtualCameraConfig : Resource
    {
        [Export] public LenData LenData { get; private set; }
        [Export] public Godot.Collections.Dictionary<PositionModuleType, Resource> PositionData { get; private set; }
        [Export] public Godot.Collections.Dictionary<RotationModuleType, Resource> RotationModules { get; private set; }

        public void Initialize()
        {
            LenData = new LenData();
            LoadPositionData();
            CreateNoneRotationModule();
        }

        private void LoadPositionData()
        {
            PositionData = new Godot.Collections.Dictionary<PositionModuleType, Resource>
        {
            { PositionModuleType.None, null },
            { PositionModuleType.Orbit, new OrbitData() },
            { PositionModuleType.ThirdPerson, new ThirdPersonData() },
            { PositionModuleType.Follow, new FollowData() },
            { PositionModuleType.SplineDolly, new SplineDollyData() }
        };
        }

        public Resource GetPositionDataByID(PositionModuleType id)
        {
            if (PositionData.ContainsKey(id))
            {
                return PositionData[id];
            }

            return null;
        }

        private void CreateNoneRotationModule()
        {
            RotationModules = new Godot.Collections.Dictionary<RotationModuleType, Resource>
        {
            { RotationModuleType.None, null },
            { RotationModuleType.HardLook, null }
        };
        }

        public Resource GetRotationModuleByID(RotationModuleType id)
        {
            foreach (var module in RotationModules)
            {
                if (RotationModules.ContainsKey(id))
                {
                    return RotationModules[id];
                }
            }

            return null;
        }

    }
}