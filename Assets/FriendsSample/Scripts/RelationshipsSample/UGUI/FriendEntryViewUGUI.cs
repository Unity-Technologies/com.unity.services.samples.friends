using TMPro;
using Unity.Services.Friends.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.Services.Toolkits.Friends.UGUI
{
    public class FriendEntryViewUGUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_NameText = null;
        [SerializeField] private TextMeshProUGUI m_PresenceText = null;
        [SerializeField] private TextMeshProUGUI m_ActivityText = null;
        [SerializeField] private Image m_PresenceColorImage = null;

        public Button removeFriendButton = null;
        public Button blockFriendButton = null;

        public void Init(string playerName, PresenceAvailabilityOptions presence, string activity)
        {
            m_NameText.text = playerName;
            m_PresenceText.text = presence.ToString();
            var index = FriendsUtils.RemapEnumIndex(presence);
            var presenceColor = ColorUtils.GetPresenceColor(index);
            m_PresenceText.color = presenceColor;
            m_PresenceColorImage.color = presenceColor;
            m_ActivityText.text = activity;
        }
    }
}