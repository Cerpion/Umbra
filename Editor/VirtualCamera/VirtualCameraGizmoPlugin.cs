using Godot;
using Umbra;

public partial class VirtualCameraGizmoPlugin : EditorNode3DGizmoPlugin
{
    private const float Depth = 0.45f;
    private const float Aspect = 16f / 9f;

    public VirtualCameraGizmoPlugin()
    {
        CreateMaterial("frustum", Color.Color8(245, 93, 39));
        CreateIconMaterial("icon", GD.Load<Texture2D>("res://addons/Umbra/Icons/VirtualCameraViewport.svg"),false);
    }

    public override string _GetGizmoName() => "VirtualCamera";

    public override bool _HasGizmo(Node3D node) => node is VirtualCamera;

    public override void _Redraw(EditorNode3DGizmo gizmo)
    {
        gizmo.Clear();
        gizmo.AddUnscaledBillboard(GetMaterial("icon", gizmo), 0.03f);

        var cam = gizmo.GetNode3D() as VirtualCamera;
        if (cam.IsActive)
        {
            return;
        }

        Transform3D pose = GetPose(cam);

        float halfHeight = Mathf.Tan(Mathf.DegToRad(cam.Len.FOV) * 0.5f) * Depth;
        float halfWidth = halfHeight * Aspect;

        Vector3 origin = pose * Vector3.Zero;
        Vector3 a = pose * new Vector3(-halfWidth, halfHeight, -Depth);
        Vector3 b = pose * new Vector3(halfWidth, halfHeight, -Depth);
        Vector3 c = pose * new Vector3(halfWidth, -halfHeight, -Depth);
        Vector3 d = pose * new Vector3(-halfWidth, -halfHeight, -Depth);

        Vector3 t1 = pose * new Vector3(-halfWidth * 0.4f, halfHeight, -Depth);
        Vector3 t2 = pose * new Vector3(halfWidth * 0.4f, halfHeight, -Depth);
        Vector3 t3 = pose * new Vector3(0f, halfHeight * 1.3f, -Depth);

        Vector3[] lines =
        [
            origin, a, origin, b, origin, c, origin, d,   // aristas de la pirámide
            a, b, b, c, c, d, d, a,                       // rectángulo del fondo
            t1, t2, t2, t3, t3, t1                        // triángulo
        ];

        gizmo.AddLines(lines, GetMaterial("frustum", gizmo));
    }

    private static Transform3D GetPose(VirtualCamera cam)
    {
        var desired = new Transform3D(
            new Basis(Quaternion.FromEuler(cam.DesiredRotation)),
            cam.DesiredPosition);

        return cam.GlobalTransform.AffineInverse() * desired;
    }
}
