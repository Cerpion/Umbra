using Godot;
using Umbra;

public partial class VirtualCameraEditor : EditorInspectorPlugin
{

    public override bool _CanHandle(GodotObject @object)
    {
        if (@object is VirtualCamera)
        {
            return true;
        }

        return false;
    }

    public override bool _ParseProperty(GodotObject @object, Variant.Type type, string name, PropertyHint hintType, string hintString, PropertyUsageFlags usageFlags, bool wide)
    {
        if (name == nameof(VirtualCamera.Data))
            return true;

        return false;
    }
}