using UnityEngine;
using UnityEngine.UIElements;

namespace UnityGamingServicesUsesCases.Relationships.UIToolkit
{
    public class RelationShipsUIToolkitController : MonoBehaviour, IRelationshipsUIController
    {
        [SerializeField]
        UIDocument socialUIDoc;

        [SerializeField]
        VisualTreeAsset friendEntryTemplate;

        [SerializeField]
        VisualTreeAsset requestEntryTemplate;

        [SerializeField]
        VisualTreeAsset blockedEntryTemplate;

        public ILocalPlayerView LocalPlayerView { get; set; }
        public IRelationshipBarView RelationshipBarView { get; private set; }
        public IRequestFriendView RequestFriendView { get; private set; }
        public IFriendsListView FriendsListView { get; private set; }
        public IRequestListView RequestListView { get; private set; }
        public IBlockedListView BlockListView { get; private set; }

        VisualElement m_Root;
        const string k_LocalPlayerViewName = "local-player-entry";

        public void Init()
        {
            m_Root = socialUIDoc.rootVisualElement;

            var localPlayerControlView = m_Root.Q(k_LocalPlayerViewName);

            LocalPlayerView = new LocalPlayerView(localPlayerControlView);

            RequestFriendView = new RequestFriendView(m_Root);

            FriendsListView = new FriendsListView(m_Root, friendEntryTemplate);
            RequestListView = new RequestListView(m_Root, requestEntryTemplate);
            BlockListView = new BlockedListView(m_Root, blockedEntryTemplate);

            RelationshipBarView = new RelationshipBarView(m_Root);
            RelationshipBarView.onShowFriends = OnFriendList;
            RelationshipBarView.onShowRequests = OnRequestList;
            RelationshipBarView.onShowBlocks = OnBlockList;
            RelationshipBarView.onShowRequestFriend = ShowAddFriendPopup;
            RequestFriendView.Hide();
        }

        void OnFriendList()
        {
            FriendsListView.Show();
            RequestListView.Hide();
            BlockListView.Hide();
            FriendsListView.Refresh();
        }

        void OnRequestList()
        {
            RequestListView.Show();
            FriendsListView.Hide();
            BlockListView.Hide();
            RequestListView.Refresh();
        }

        void OnBlockList()
        {
            BlockListView.Show();
            RequestListView.Hide();
            FriendsListView.Hide();
            BlockListView.Refresh();
        }

        void ShowAddFriendPopup()
        {
            if (RequestFriendView.IsShowing)
                RequestFriendView.Hide();
            else
                RequestFriendView.Show();
        }
    }
}