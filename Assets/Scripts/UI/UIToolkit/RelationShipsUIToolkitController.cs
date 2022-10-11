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
        public RelationshipBarView relationshipBarView { get; private set; }
        public RequestFriendPopupView requestFriendPopupView { get; private set; }
        public FriendsListView friendsListView { get; private set; }
        public RequestListView requestListView { get; private set; }
        public BlockedListView blockList { get; private set; }

        VisualElement m_Root;
        const string k_LocalPlayerViewName = "local-player-entry";

        public void Init()
        {
            m_Root = socialUIDoc.rootVisualElement;

            var localPlayerControlView = m_Root.Q(k_LocalPlayerViewName);

            LocalPlayerView = new LocalPlayerView(localPlayerControlView);

            requestFriendPopupView = new RequestFriendPopupView(m_Root);
            friendsListView = new FriendsListView(m_Root, friendEntryTemplate);
            requestListView = new RequestListView(m_Root, requestEntryTemplate);
            blockList = new BlockedListView(m_Root, blockedEntryTemplate);

            relationshipBarView = new RelationshipBarView(m_Root);
            relationshipBarView.onFriends = OnFriendList;
            relationshipBarView.onRequests = OnRequestList;
            relationshipBarView.onBlocks = OnBlockList;
            relationshipBarView.onAddFriend = ShowAddFriendPopup;
            requestFriendPopupView.Hide();
        }

        void OnFriendList()
        {
            friendsListView.Show();
            requestListView.Hide();
            blockList.Hide();
        }

        void OnRequestList()
        {
            requestListView.Show();
            friendsListView.Hide();
            blockList.Hide();
        }

        void OnBlockList()
        {
            blockList.Show();
            requestListView.Hide();
            friendsListView.Hide();
        }

        void ShowAddFriendPopup()
        {
            requestFriendPopupView.Show();
        }
    }
}