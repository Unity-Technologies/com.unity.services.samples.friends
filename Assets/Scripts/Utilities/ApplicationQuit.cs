using UnityEngine;

public class ApplicationQuit : MonoBehaviour
{
#if !UNITY_EDITOR
    [SerializeField] private bool m_DeleteSaveOnQuit = false;
#endif

    private void Update()
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