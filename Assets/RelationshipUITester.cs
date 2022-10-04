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
        }

    }

}
