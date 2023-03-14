using UnityEngine;

// A simple class for limiting FPS for samples
public class FpsLimiter : MonoBehaviour
{
    void Start()
    {
        Application.targetFrameRate = 60; // Used on non-PC platforms
        QualitySettings.vSyncCount = 1; // Used on PC platforms
    }
}
