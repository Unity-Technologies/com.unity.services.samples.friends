using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityGamingServicesUsesCases.Relationships.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class RelationshipUIController : MonoBehaviour
    {
        [SerializeField]
        UIDocument socialUIDoc;

        [SerializeField]
        VisualTreeAsset friendEntryTemplate;

        [SerializeField]
        VisualTreeAsset requestEntryTemplate;

        [SerializeField]
        VisualTreeAsset blockedEntryTemplate;

        public LocalPlayerView localLocalPlayerView { get; private set; }
        public RelationshipBarView relationshipBarView { get; private set; }
        public RequestFriendPopupView requestFriendPopupView { get; private set; }
        public FriendsListView friendsListView { get; private set; }
        public RequestListView requestListView { get; private set; }
        public BlockedListView blockedListView { get; private set; }

        VisualElement m_Root;
        const string k_LocalPlayerViewName = "local-player-entry";

        void Awake()
        {
            m_Root = socialUIDoc.rootVisualElement;

            var localPlayerControlView = m_Root.Q(k_LocalPlayerViewName);

            localLocalPlayerView = new LocalPlayerView(localPlayerControlView);

            requestFriendPopupView = new RequestFriendPopupView(m_Root);
            friendsListView = new FriendsListView(m_Root, friendEntryTemplate);
            requestListView = new RequestListView(m_Root, requestEntryTemplate);
            blockedListView = new BlockedListView(m_Root, blockedEntryTemplate);

            relationshipBarView = new RelationshipBarView(m_Root);
            relationshipBarView.onListButton += SwitchView;
            relationshipBarView.onAddFriend += ShowAddFriendPopup;
            requestFriendPopupView.Show(false);
        }

        void SwitchView(ShowListState listToView)
        {
            friendsListView.Show(listToView == ShowListState.Friends);
            requestListView.Show(listToView == ShowListState.Requests);
            blockedListView.Show(listToView == ShowListState.Blocked);
        }

        void ShowAddFriendPopup()
        {
            requestFriendPopupView.Show(true);
        }
    }
}