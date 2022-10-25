using UnityEngine;
using UnityEngine.UIElements;

namespace UnityGamingServicesUsesCases.Relationships.UIToolkit
{
    public sealed class RelationshipsUIToolkitController : RelationshipsUIController
    {
        [SerializeField] UIDocument m_SocialUIDoc;

        [SerializeField] VisualTreeAsset m_FriendEntryTemplate;

        [SerializeField] VisualTreeAsset m_RequestEntryTemplate;

        [SerializeField] VisualTreeAsset m_BlockedEntryTemplate;
        
        public override ILocalPlayerView LocalPlayerView { get; set; }
        public override IRelationshipBarView RelationshipBarView { get; set; }
        public override IAddFriendView AddFriendView { get; set; }
        public override IFriendsListView FriendsListView { get; set; }
        public override IRequestListView RequestListView { get; set; }
        public override IBlockedListView BlockListView { get; set; }

        const string k_LocalPlayerViewName = "local-player-entry";

        public override void Init()
        {
            var root = m_SocialUIDoc.rootVisualElement;

            var localPlayerControlView = root.Q(k_LocalPlayerViewName);

            LocalPlayerView = new LocalPlayerViewUIToolkit(localPlayerControlView);

            AddFriendView = new AddFriendViewUIToolkit(root);

            FriendsListView = new FriendsListViewUIToolkit(root, m_FriendEntryTemplate);
            RequestListView = new RequestListViewUIToolkit(root, m_RequestEntryTemplate);
            BlockListView = new BlockedListViewUIToolkit(root, m_BlockedEntryTemplate);

            var listViews = new IListView[] { FriendsListView, RequestListView, BlockListView };
            RelationshipBarView = new NavBarViewUIToolkit(root, listViews);
            RelationshipBarView.onShowRequestFriend = ShowAddFriendPopup;
            AddFriendView.Hide();
        }

        void ShowAddFriendPopup()
        {
            AddFriendView.Show();
        }
    }
}