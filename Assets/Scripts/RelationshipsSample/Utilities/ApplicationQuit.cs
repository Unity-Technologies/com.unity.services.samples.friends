using UnityEngine;

namespace Unity.Services.Toolkits.Friends
{
    public class ApplicationQuit : MonoBehaviour
    {
        [SerializeField] bool m_DeleteSaveOnQuit = false;

        void Update()
        {
#if !UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
                if (m_DeleteSaveOnQuit)
                    PlayerPrefs.DeleteAll();
            }
#endif
        }
    }
}