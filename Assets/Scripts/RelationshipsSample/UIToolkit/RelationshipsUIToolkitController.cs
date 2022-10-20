using UnityEngine;
using UnityEngine.UIElements;

namespace UnityGamingServicesUsesCases.Relationships.UIToolkit
{
    public class RelationshipsUIToolkitController : MonoBehaviour, IRelationshipsUIController
    {
        [SerializeField]
        UIDocument m_SocialUIDoc;

        [SerializeField]
        VisualTreeAsset m_FriendEntryTemplate;

        [SerializeField]
        VisualTreeAsset m_RequestEntryTemplate;

        [SerializeField]
        VisualTreeAsset m_BlockedEntryTemplate;

        public ILocalPlayerView LocalPlayerView { get; private set; }
        public IRelationshipBarView RelationshipBarView { get; private set; }
        public IAddFriendView AddFriendView { get; private set; }
        public IFriendsListView FriendsListView { get; private set; }
        public IRequestListView RequestListView { get; private set; }
        public IBlockedListView BlockListView { get; private set; }

        const string k_LocalPlayerViewName = "local-player-entry";

        public void Init()
        {
            var root = m_SocialUIDoc.rootVisualElement;

            var localPlayerControlView = root.Q(k_LocalPlayerViewName);

            LocalPlayerView = new LocalPlayerViewUIToolkit(localPlayerControlView);

            AddFriendView = new AddFriendViewUIToolkit(root);

            FriendsListView = new FriendsListViewUIToolkit(root, m_FriendEntryTemplate);
            RequestListView = new RequestListViewUIToolkit(root, m_RequestEntryTemplate);
            BlockListView = new BlockedListViewUIToolkit(root, m_BlockedEntryTemplate);

            RelationshipBarView = new NavBarViewUIToolkit(root);
            RelationshipBarView.onShowFriends = OnFriendList;
            RelationshipBarView.onShowRequests = OnRequestList;
            RelationshipBarView.onShowBlocks = OnBlockList;
            RelationshipBarView.onShowRequestFriend = ShowAddFriendPopup;
            AddFriendView.Hide();
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
            AddFriendView.Show();
        }
    }
}
