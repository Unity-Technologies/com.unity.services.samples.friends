using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UnityGamingServicesUsesCases.Relationships
{
    public class FriendsEntryView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_NameText = null;
        [SerializeField] private TextMeshProUGUI m_PresenceText = null;
        public Button button1 = null;
        public Button button2 = null;

        public void Init(string playerName, string presence)
        {
            m_NameText.text = playerName;
            m_PresenceText.text = presence;
        }
    }
}