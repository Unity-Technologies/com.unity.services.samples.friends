using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UnityGamingServicesUsesCases.Relationships.UGUI
{
    public class FriendEntryViewUGUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_NameText = null;
        [SerializeField] private TextMeshProUGUI m_PresenceText = null;
        [SerializeField] private TextMeshProUGUI m_ActivityText = null;
        
        public Button removeFriendButton = null;
        public Button blockFriendButton = null;

        public void Init(string playerName, string presence, string activity)
        {
            m_NameText.text = playerName;
            m_PresenceText.text = presence;
            m_ActivityText.text = activity;
        }
    }
}