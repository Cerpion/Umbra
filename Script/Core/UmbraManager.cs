using Godot;
using Godot.Collections;

namespace Umbra
{
[Tool]
    public partial class UmbraManager : Node
    {
        private Array<VirtualCamera> _virtualCameras = new();

        [Signal] public delegate void PriorityChangeEventHandler();
        [Signal] public delegate void CameraLenChangedEventHandler();

        public void NotifyLenChanged()
        {
            EmitSignal(SignalName.CameraLenChanged);
        }

        public void NotifyPriorityChange()
        {
            EmitSignal(SignalName.PriorityChange);
        }

        public override void _EnterTree()
        {
            if (!Engine.HasSingleton(nameof(UmbraManager)))
            {
                Engine.RegisterSingleton(nameof(UmbraManager), this);
            }
        }

        public void SubscribeVirtualCamera(VirtualCamera virtualCamera)
        {
            virtualCamera.PriorityChanged += NotifyPriorityChange;
            virtualCamera.Len.Changed += NotifyLenChanged;

            _virtualCameras.Add(virtualCamera);
            //GD.Print("Subscribe" + virtualCamera.Name);

            EmitSignal(SignalName.PriorityChange);
        }

        public void UnsubscribeVirtualCamera(VirtualCamera virtualCamera)
        {
            virtualCamera.PriorityChanged -= NotifyPriorityChange;
            virtualCamera.Len.Changed -= NotifyLenChanged;

            _virtualCameras.Remove(virtualCamera);
            //GD.Print("Unsubscribe" + virtualCamera.Name);

            EmitSignal(SignalName.PriorityChange);
        }

        public VirtualCamera GetBestCamera()
        {

            //GD.Print("Count: " + _virtualCameras.Count);

            if (_virtualCameras.Count == 0)
                return null;


            VirtualCamera bestCamera = _virtualCameras[0];

            foreach (var camera in _virtualCameras)
            {
                if (camera.Priority > bestCamera.Priority)
                {
                    bestCamera = camera;
                }
            }

            return bestCamera;
        }
    }
}