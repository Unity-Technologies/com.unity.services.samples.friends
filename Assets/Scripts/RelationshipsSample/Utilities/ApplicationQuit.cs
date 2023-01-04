using UnityEngine;
using UnityEngine.UI;

namespace Unity.Services.Toolkits.Friends
{
    public class ApplicationQuit : MonoBehaviour
    {
        [SerializeField] Button m_QuitButton;

        void Awake()
        {
            m_QuitButton.onClick.AddListener(Quit);
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Quit();
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}
