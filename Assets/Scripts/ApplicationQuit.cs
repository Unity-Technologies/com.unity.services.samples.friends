using UnityEngine;

public class ApplicationQuit : MonoBehaviour
{
    [SerializeField] private bool m_DeleteSaveOnQuit = false;

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