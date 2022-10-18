using UnityEngine;

namespace UnityGamingServicesUsesCases.Relationships.UGUI
{
    public class RelationshipsUGUIControllerR : MonoBehaviour, IRelationshipsUIController
    {
        [SerializeField] private LocalPlayerViewUGUI m_LocalPlayerViewUGUI;
        [SerializeField] private SendRequestViewUGUI m_SendRequestViewUGUI;
        [SerializeField] private NavBarViewUGUI m_NavBarViewUGUI;
        [SerializeField] private FriendsViewUGUI m_FriendsViewUGUI;
        [SerializeField] private RequestsViewUGUI m_RequestsViewUGUI;
        [SerializeField] private BlocksViewUGUI m_BlocksViewUGUI;
        public ILocalPlayerView LocalPlayerView => m_LocalPlayerViewUGUI;
        public IRelationshipBarView RelationshipBarView => m_NavBarViewUGUI;
        public IRequestFriendView SendRequestPopupView =>m_SendRequestViewUGUI;
        public IFriendsListView FriendsListView => m_FriendsViewUGUI;
        public IRequestListView RequestListView => m_RequestsViewUGUI;
        public IBlockedListView BlockListView => m_BlocksViewUGUI;

        //TODO: to remove before release
        private void Reset()
        {
            m_LocalPlayerViewUGUI = GetComponentInChildren<LocalPlayerViewUGUI>();
            m_SendRequestViewUGUI = GetComponentInChildren<SendRequestViewUGUI>();
            m_NavBarViewUGUI = GetComponentInChildren<NavBarViewUGUI>();
            m_FriendsViewUGUI = GetComponentInChildren<FriendsViewUGUI>();
            m_RequestsViewUGUI = GetComponentInChildren<RequestsViewUGUI>();
            m_BlocksViewUGUI = GetComponentInChildren<BlocksViewUGUI>();
        }

        public void Init()
        {
            m_NavBarViewUGUI.onShowFriends += ShowFriends;
            m_NavBarViewUGUI.onShowRequests += ShowRequests;
            m_NavBarViewUGUI.onShowBlocks += ShowBlocks;
            m_NavBarViewUGUI.onShowRequestFriend += ShowSendRequestPopUp;
            m_SendRequestViewUGUI.Init();
            HideAll();
        }

        private void ShowSendRequestPopUp()
        {
            m_SendRequestViewUGUI.Show();
        }

        void HideAll()
        {
            FriendsListView.Hide();
            RequestListView.Hide();
            BlockListView.Hide();
        }

        void ShowFriends()
        {
            FriendsListView.Show();
            RequestListView.Hide();
            BlockListView.Hide();
        }

        void ShowRequests()
        {
            RequestListView.Show();
            FriendsListView.Hide();
            BlockListView.Hide();
        }

        void ShowBlocks()
        {
            BlockListView.Show();
            RequestListView.Hide();
            FriendsListView.Hide();
        }
    }
}