using JetBrains.Annotations;
using UnityEngine;

namespace Unity.Services.Samples.Utilities
{
    public class ApplicationQuit : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Quit();
        }

        [UsedImplicitly]
        public void Quit()
        {
            Application.Quit();
        }
    }
}