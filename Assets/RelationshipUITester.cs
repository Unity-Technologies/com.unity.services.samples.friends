using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityGamingServicesUsesCases.Relationships.UI
{
    public class RelationshipUITester : MonoBehaviour
    {
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
            m_Controller.RelationshipBarControl.SetFriendsListReference(m_FriendsList);
            m_Controller.RelationshipBarControl.SetRequestListReference(m_RequestList);
            m_Controller.RelationshipBarControl.SetBlockedListReference(m_BlockedList);
            m_Controller.RelationshipBarControl.onAcceptFriend += TestAccept;
            m_Controller.RelationshipBarControl.onDenyFriend += TestDeny;
            m_Controller.RelationshipBarControl.onBlockUser += TestBlock;
            m_Controller.RelationshipBarControl.onRemoveFriend += TestRemove;
            m_Controller.RelationshipBarControl.onUnblockuser += TestUnblock;
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