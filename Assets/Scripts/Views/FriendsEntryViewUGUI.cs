using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UnityGamingServicesUsesCases.Relationships.UGUI
{
    public class FriendsEntryViewUGUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_NameText = null;
        [SerializeField] private TextMeshProUGUI m_PresenceText = null;
        [SerializeField] private TextMeshProUGUI m_ActivityText = null;
        
        public Button button1 = null;
        public Button button2 = null;

        public void Init(string playerName, string presence, string activity)
        {
            m_NameText.text = playerName;
            m_PresenceText.text = presence;
            m_ActivityText.text = activity;
        }
    }
}