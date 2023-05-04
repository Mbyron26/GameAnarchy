namespace GameAnarchy;
using ICities;

public class CityServicesExtension : ThreadingExtensionBase {
    private byte frameIndex;

    public override void OnAfterSimulationFrame() {
        if (!(managers.loading is not null && managers.loading.loadingComplete && (managers.loading.currentMode == AppMode.Game)) || SimulationManager.instance.SimulationPaused)
            return;
        frameIndex = (byte)(SimulationManager.instance.m_currentFrameIndex & 15);
        if (CityServicesManager.FrameHander.Length <= frameIndex) 
            return;
        CityServicesManager.FrameHander[frameIndex].Invoke();
    }
}