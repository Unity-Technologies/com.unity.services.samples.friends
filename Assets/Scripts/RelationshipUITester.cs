using System.Collections.Generic;
using Unity.Services.Friends.Models;
using UnityEngine;


namespace UnityGamingServicesUsesCases.Relationships.UI
{
    public class RelationshipUITester : MonoBehaviour
    {
        [SerializeField]
        PlayerProfile m_LocalPlayerProfile;

        [SerializeField]
        RelationshipUIController m_Controller;

        [SerializeField]
        List<FriendsEntryData> m_FriendsList;

        [SerializeField]
        List<PlayerProfile> m_RequestList;

        [SerializeField]
        List<PlayerProfile> m_BlockedList;

        void Start()
        {

            m_Controller.friendsListView.BindList(m_FriendsList);
            m_Controller.friendsListView.onRemoveFriend += TestRemove;
            m_Controller.friendsListView.onBlockFriend += TestBlock;

            m_Controller.requestListView.BindList(m_RequestList);
            m_Controller.requestListView.onAcceptUser += TestAccept;
            m_Controller.requestListView.onDeclineUser += TestDeny;
            m_Controller.requestListView.onBlockUser += TestBlock;

            m_Controller.blockList.BindList(m_BlockedList);
            m_Controller.blockList.onUnBlock += TestUnblock;

            m_Controller.requestFriendPopupView.tryAddFriend += TestRequestFriend;
        }

        void TestLogin()
        {
            m_Controller.localLocalPlayerView.Refresh("LocalTester", "id", "Testing",
                PresenceAvailabilityOptions.ONLINE);

            m_Controller.localLocalPlayerView.onPresenceChanged = TestUserChangedPresence;
        }

        void TestUserChangedPresence((PresenceAvailabilityOptions presence, string activity) presence)
        {
            Debug.Log($"User Switch presence to {presence.activity}");
        }

        int tryTimes = 0;

        void TestRequestFriend(string inputID)
        {
            if (tryTimes > 3)
            {
                m_Controller.requestFriendPopupView.Hide();
                tryTimes = 0;
                return;
            }

            Debug.Log($"Requested Friend: {inputID}");
            tryTimes++;
            m_Controller.requestFriendPopupView.ShowAddFriendFailedWarning();
        }

        void TestAccept(string id)
        {
            Debug.Log($"Accepted: {id}");
        }

        void TestDeny(string id)
        {
            Debug.Log($"Denied: {id}");
        }

        void TestBlock(string id)
        {
            Debug.Log($"Blocked: {id}");
        }

        void TestRemove(string id)
        {
            Debug.Log($"Removed: {id}");
        }

        void TestUnblock(string id)
        {
            Debug.Log($"Unblocked: {id}");
        }
    }
}