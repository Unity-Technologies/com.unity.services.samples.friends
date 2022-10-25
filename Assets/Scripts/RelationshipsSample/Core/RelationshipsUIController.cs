using UnityEngine;

namespace UnityGamingServicesUsesCases.Relationships
{
    public abstract class RelationshipsUIController : MonoBehaviour, IRelationshipsUIController
    {
        public virtual ILocalPlayerView LocalPlayerView { get; set; }
        public virtual IRelationshipBarView RelationshipBarView { get; set; }
        public virtual IAddFriendView AddFriendView { get; set; }
        public virtual IFriendsListView FriendsListView { get; set; }
        public virtual IRequestListView RequestListView { get; set; }
        public virtual IBlockedListView BlockListView { get; set; }
        public abstract void Init();
    }
}