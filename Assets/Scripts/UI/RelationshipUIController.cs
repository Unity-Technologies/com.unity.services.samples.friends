using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
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

        public PlayerEntryControl localPlayerControl { get; private set; }
        public RelationshipBarControl relationshipBarControl { get; private set; }
        public RequestFriendPopupControl requestFriendPopupControl { get; private set; }
        public FriendsListControl friendsListControl { get; private set; }
        public RequestListControl requestListControl { get; private set; }
        public BlockedListControl blockedListControl { get; private set; }

        VisualElement m_Root;
        const string k_LocalPlayerViewName = "local-player-entry";

        void Awake()
        {
            m_Root = socialUIDoc.rootVisualElement;

            var localPlayerControlView = m_Root.Q(k_LocalPlayerViewName);

            localPlayerControl = new PlayerEntryControl(localPlayerControlView,true);
            requestFriendPopupControl = new RequestFriendPopupControl(m_Root);
            friendsListControl = new FriendsListControl(m_Root, friendEntryTemplate);
            requestListControl = new RequestListControl(m_Root, requestEntryTemplate);
            blockedListControl = new BlockedListControl(m_Root, blockedEntryTemplate);

            relationshipBarControl = new RelationshipBarControl(m_Root);
            relationshipBarControl.onListButtonClicked += SwitchView;
            relationshipBarControl.onAddFriendPressed += ShowAddFriendPopup;
            requestFriendPopupControl.Show(false);
        }

        void SwitchView(ShowListState listToView)
        {
            Debug.Log($"Switching View to {listToView}");
            friendsListControl.Show(listToView == ShowListState.Friends);
            requestListControl.Show(listToView == ShowListState.Requests);
            blockedListControl.Show(listToView == ShowListState.Blocked);
        }

        void ShowAddFriendPopup()
        {
            Debug.Log("Show Friend!");
            requestFriendPopupControl.Show(true);
        }
    }
}
