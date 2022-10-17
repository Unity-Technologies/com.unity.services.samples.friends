
namespace UnityGamingServicesUsesCases.Relationships
{
    public interface IRelationshipsUIController
    {
        ILocalPlayerView LocalPlayerView { get; }
        IRelationshipBarView RelationshipBarView { get; }
        IRequestFriendView SendRequestPopupView { get; }
        IFriendsListView FriendsListView { get; }
        IRequestListView RequestListView { get; }
        IBlockedListView BlockListView { get; }

        void Init();
    }
}
