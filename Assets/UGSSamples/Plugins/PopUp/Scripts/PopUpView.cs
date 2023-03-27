using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.Samples.UI
{
    public class PopUpView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_MessageText;
        [SerializeField] private Button m_Option1Button;
        [SerializeField] TMP_Text m_option1Text;

        [SerializeField] private Button m_Option2Button;
        [SerializeField] TMP_Text m_option2Text;
        int m_ButtonIndex = -1;


        private void Awake()
        {
            PopUpEvents.PopupRequestedAsync += ShowPopup;
            m_Option1Button.onClick.AddListener(OnOption1Clicked);
            m_Option2Button.onClick.AddListener(OnOption2Clicked);

            Close();
        }

        public async Task<int> ShowPopup(string message, string option1Text, string option2Text = default)
        {
            m_ButtonIndex = -1;
            OnShow(message);
            m_option1Text.text = option1Text;
            if (!string.IsNullOrEmpty(option2Text))
                m_option2Text.text = option2Text;
            else
                m_Option2Button.gameObject.SetActive(false);


            while (m_ButtonIndex < 0)
            {
                await Task.Delay(100);
            }


            Close();
            return m_ButtonIndex;
        }

        void OnOption1Clicked()
        {
            m_ButtonIndex = 0;
        }

        void OnOption2Clicked()
        {
            m_ButtonIndex = 1;
        }

        void OnShow(string message)
        {
            m_MessageText.text = message;
            gameObject.SetActive(true);
        }

        void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
