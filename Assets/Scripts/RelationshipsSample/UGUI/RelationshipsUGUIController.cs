using UnityEngine;

namespace UnityGamingServicesUsesCases.Relationships.UGUI
{
    public class RelationshipsUGUIController : MonoBehaviour, IRelationshipsUIController
    {
        [SerializeField] private LocalPlayerViewUGUI m_LocalPlayerViewUGUI;
        [SerializeField] private AddFriendViewUGUI m_AddFriendViewUGUI;
        [SerializeField] private NavBarViewUGUI m_NavBarViewUGUI;
        [SerializeField] private FriendsViewUGUI m_FriendsViewUGUI;
        [SerializeField] private RequestsViewUGUI m_RequestsViewUGUI;
        [SerializeField] private BlocksViewUGUI m_BlocksViewUGUI;
        public ILocalPlayerView LocalPlayerView => m_LocalPlayerViewUGUI;
        public IRelationshipBarView RelationshipBarView => m_NavBarViewUGUI;
        public IAddFriendView AddFriendView =>m_AddFriendViewUGUI;
        public IFriendsListView FriendsListView => m_FriendsViewUGUI;
        public IRequestListView RequestListView => m_RequestsViewUGUI;
        public IBlockedListView BlockListView => m_BlocksViewUGUI;

        //TODO: to remove before release
        private void Reset()
        {
            m_LocalPlayerViewUGUI = GetComponentInChildren<LocalPlayerViewUGUI>();
            m_AddFriendViewUGUI = GetComponentInChildren<AddFriendViewUGUI>();
            m_NavBarViewUGUI = GetComponentInChildren<NavBarViewUGUI>();
            m_FriendsViewUGUI = GetComponentInChildren<FriendsViewUGUI>();
            m_RequestsViewUGUI = GetComponentInChildren<RequestsViewUGUI>();
            m_BlocksViewUGUI = GetComponentInChildren<BlocksViewUGUI>();
        }

        public void Init()
        {
            m_AddFriendViewUGUI.Init();
            m_NavBarViewUGUI.Init(new IListView[]{FriendsListView,RequestListView,BlockListView});
            m_NavBarViewUGUI.onShowRequestFriend += ShowSendRequestPopUp;
        }

        private void ShowSendRequestPopUp()
        {
            m_AddFriendViewUGUI.Show();
        }
    }
}