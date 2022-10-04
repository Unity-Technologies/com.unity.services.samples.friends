using System;
using System.Collections.Generic;
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
        VisualTreeAsset blockEntryTemplate;

        public Action<ShowListState> OnListViewChanged;

        VisualElement m_Root;
        LocalPlayerControl m_LocalPlayerControl;
        public RelationshipBarControl RelationshipBarControl { get; private set; }

        RequestFriendControl m_RequestFriendControl;

        List<UIBaseControl> m_ModalList;

        void Awake()
        {
            InitViewClasses();
            RegisterRelationshipCallbacks();
        }

        void InitViewClasses()
        {
            m_Root = socialUIDoc.rootVisualElement;

            m_LocalPlayerControl = new LocalPlayerControl(m_Root);
            RelationshipBarControl = new RelationshipBarControl(m_Root,
                friendEntryTemplate,
                requestEntryTemplate,
                blockEntryTemplate);
            m_RequestFriendControl = new RequestFriendControl(m_Root);

        }

        void RegisterRelationshipCallbacks()
        {
            RelationshipBarControl.onFriendsListPressed += () => { OnListViewChanged?.Invoke(ShowListState.Friends); };
            RelationshipBarControl.onRequestListPressed += () => { OnListViewChanged?.Invoke(ShowListState.Requests); };
            RelationshipBarControl.onBlockedListPressed += () => { OnListViewChanged?.Invoke(ShowListState.Blocked); };
        }


    }

}
