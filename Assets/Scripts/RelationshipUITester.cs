using System.Collections;
using System.Collections.Generic;
using Unity.Services.Friends.Models;
using UnityEngine;
using UnityEngine.Serialization;

namespace UnityGamingServicesUsesCases.Relationships.UI
{
    public class RelationshipUITester : MonoBehaviour
    {
        [SerializeField]
        PlayerProfile m_LocalPlayerProfile;

        [SerializeField]
        RelationshipUIController m_Controller;

        [SerializeField]
        List<PlayerProfile> m_FriendsList;

        [SerializeField]
        List<PlayerProfile> m_RequestList;

        [SerializeField]
        List<PlayerProfile> m_BlockedList;

        void Start()
        {
            TestLogin();

            m_Controller.friendsListControl.BindList(m_FriendsList);
            m_Controller.friendsListControl.onRemoveFriend += TestRemove;
            m_Controller.friendsListControl.onBlockFriend += TestBlock;

            m_Controller.requestListControl.BindList(m_RequestList);
            m_Controller.requestListControl.onAcceptUser += TestAccept;
            m_Controller.requestListControl.onDenyUser += TestDeny;
            m_Controller.requestListControl.onBlockUser += TestBlock;

            m_Controller.blockedListControl.BindList(m_BlockedList);
            m_Controller.blockedListControl.onUnBlockuser += TestUnblock;

            m_Controller.requestFriendPopupControl.tryRequestFriend += TestRequestFriend;
        }

        void TestLogin()
        {
            m_Controller.localPlayerControl.SetName(m_LocalPlayerProfile.Name);
            m_Controller.localPlayerControl.SetActivity("Activities!");

            m_Controller.localPlayerControl.SetStatus(PresenceAvailabilityOptions.ONLINE);
        }

        void TestUserChangedPresence(PresenceAvailabilityOptions presence)
        {
            Debug.Log($"User Switch presence to {presence}");
        }

        void TestUserChangedActivity(string activity)
        {
            Debug.Log($"User Switch activity to {activity}");
        }

        int tryTimes = 0;

        void TestRequestFriend(string inputID)
        {
            if (tryTimes > 3)
            {
                m_Controller.requestFriendPopupControl.Show(false);
                tryTimes = 0;
                return;
            }

            Debug.Log($"Requested Friend: {inputID}");
            tryTimes++;
            m_Controller.requestFriendPopupControl.ShowWarning();
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
