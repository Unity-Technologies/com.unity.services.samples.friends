using TMPro;
using Unity.Services.Friends.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.Services.Samples.Friends.UGUI
{
    public class FriendEntryViewUGUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI m_NameText = null;
        [SerializeField] TextMeshProUGUI m_ActivityText = null;
        [SerializeField] Image m_PresenceColorImage = null;

        public Button removeFriendButton = null;
        public Button blockFriendButton = null;
        public Button inviteFriendButton = null;
        public Button joinFriendButton = null;

        public void Init(string playerName, PresenceAvailabilityOptions presenceAvailabilityOptions,
            Activity friendActivity)
        {
            m_NameText.text = playerName;
            var index = (int)presenceAvailabilityOptions - 1;
            var presenceColor = ColorUtils.GetPresenceColor(index);
            m_PresenceColorImage.color = presenceColor;
            m_ActivityText.text = friendActivity.Status;
        }
#if LOBBY_SDK_AVAILABLE
        public void UpdateFriendPartyState(string localPlayerPartyCode, Activity friendActivity)
        {
            var localPlayerInParty = !string.IsNullOrEmpty(localPlayerPartyCode);
            var inFriendsParty = localPlayerPartyCode == friendActivity.m_ActivityData;
            var showInviteButton = localPlayerInParty && !inFriendsParty;
            var showJoinFriendButton = !inFriendsParty && friendActivity.m_ActivityType == Activity.ActivityType.Party;
            inviteFriendButton.gameObject.SetActive(showInviteButton);
            joinFriendButton.gameObject.SetActive(showJoinFriendButton);
        }
#endif
    }
}