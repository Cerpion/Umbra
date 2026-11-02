#if TOOLS
using Godot;

namespace Umbra.Editor
{
    [Tool]
    public partial class Umbra : EditorPlugin
    {
        private const string CameraBrain = "res://addons/Umbra/Script/Core/CameraBrain.cs";
        private const string CameraBrainIcon = "res://addons/Umbra/Icons/CameraBrain.svg";

        private const string VirtualCamera = "res://addons/Umbra/Script/Core/VirtualCamera.cs";
        private const string VirtualCameraIcon = "res://addons/Umbra/Icons/VirtualCamera.svg";

        private const string ManagerPath = "res://addons/Umbra/Script/Core/UmbraManager.cs";

        private VirtualCameraEditor _virtualCameraInspector;
        private VirtualCameraGizmoPlugin _gizmo;

        public override void _EnablePlugin()
        {
            AddAutoloadSingleton(nameof(UmbraManager), ManagerPath);
        }

        public override void _DisablePlugin()
        {
            RemoveAutoloadSingleton(nameof(UmbraManager));
        }

        public override void _EnterTree()
        {
            AddCustomType(nameof(CameraBrain), "Node", GD.Load<Script>(CameraBrain), GD.Load<Texture2D>(CameraBrainIcon));
            AddCustomType(nameof(VirtualCamera), "Node3D", GD.Load<Script>(VirtualCamera), GD.Load<Texture2D>(VirtualCameraIcon));

            _virtualCameraInspector = new VirtualCameraEditor();
            _gizmo = new VirtualCameraGizmoPlugin();
            AddInspectorPlugin(_virtualCameraInspector);
            AddNode3DGizmoPlugin(_gizmo);
        }

        public override void _ExitTree()
        {

            RemoveCustomType(nameof(CameraBrain));
            RemoveCustomType(nameof(VirtualCamera));

            RemoveInspectorPlugin(_virtualCameraInspector);
            RemoveNode3DGizmoPlugin(_gizmo);
        }
    }
#endif
}