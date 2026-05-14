using Android.Content;
using Android.Hardware;
using Universal_Control_Full.Services;

namespace Universal_Control_Full.Platforms.Android.Services;

public class AndroidIRService : IIRService
{
    private readonly ConsumerIrManager? _irManager;

    public AndroidIRService()
    {
        _irManager = Microsoft.Maui.ApplicationModel.Platform.CurrentActivity?
            .GetSystemService(Context.ConsumerIrService) as ConsumerIrManager;
    }

    public bool HasIrEmitter() => _irManager?.HasIrEmitter ?? false;

    public void Transmit(int carrierFrequency, int[] pattern)
    {
        if (HasIrEmitter())
        {
            _irManager?.Transmit(carrierFrequency, pattern);
        }
    }
}