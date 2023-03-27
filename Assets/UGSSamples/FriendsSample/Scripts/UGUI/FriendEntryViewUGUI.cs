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


        public void Init(string playerName, PresenceAvailabilityOptions presenceAvailabilityOptions, Activity activity)
        {
            m_NameText.text = playerName;
            var index = (int)presenceAvailabilityOptions - 1;
            var presenceColor = ColorUtils.GetPresenceColor(index);
            m_PresenceColorImage.color = presenceColor;
            m_ActivityText.text = activity.Status;
            InitParty(activity);

        }

        void InitParty(Activity activity)
        {
            #if LOBBY_SDK_AVAILABLE
            inviteFriendButton.gameObject.SetActive(true);
            joinFriendButton.gameObject.SetActive(activity.m_ActivityType == Activity.ActivityType.Party);
            #endif
        }
    }
}
