using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UnityGamingServicesUsesCases.Relationships.UGUI
{
    public class GenericEntryViewUGUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_Text = null;
        public Button button1 = null;
        public Button button2 = null;

        public void Init(string playerName)
        {
            m_Text.text = playerName;
        }
    }
}