using UnityEngine;

namespace UnityGamingServicesUsesCases.Relationships.UGUI
{
    public sealed class RelationshipsUGUIController : RelationshipsUIController
    {
        [SerializeField] private LocalPlayerViewUGUI m_LocalPlayerViewUGUI;
        [SerializeField] private AddFriendViewUGUI m_AddFriendViewUGUI;
        [SerializeField] private NavBarViewUGUI m_NavBarViewUGUI;
        [SerializeField] private FriendsViewUGUI m_FriendsViewUGUI;
        [SerializeField] private RequestsViewUGUI m_RequestsViewUGUI;
        [SerializeField] private BlocksViewUGUI m_BlocksViewUGUI;
        public override ILocalPlayerView LocalPlayerView => m_LocalPlayerViewUGUI;
        public override IRelationshipBarView RelationshipBarView => m_NavBarViewUGUI;
        public override IAddFriendView AddFriendView => m_AddFriendViewUGUI;
        public override IFriendsListView FriendsListView => m_FriendsViewUGUI;
        public override IRequestListView RequestListView => m_RequestsViewUGUI;
        public override IBlockedListView BlockListView => m_BlocksViewUGUI;

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

        public override void Init()
        {
            var listViews = new IListView[] { FriendsListView, RequestListView, BlockListView };
            m_NavBarViewUGUI.Init(listViews);
            m_NavBarViewUGUI.onShowRequestFriend += ShowSendRequestPopUp;
            
            m_AddFriendViewUGUI.Init();
        }

        private void ShowSendRequestPopUp()
        {
            m_AddFriendViewUGUI.Show();
        }
    }
}