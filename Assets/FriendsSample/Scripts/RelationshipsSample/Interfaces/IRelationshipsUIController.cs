
namespace Unity.Services.Toolkits.Friends
{
    public interface IRelationshipsUIController
    {
        ILocalPlayerView LocalPlayerView { get; }
        IRelationshipBarView RelationshipBarView { get; }
        IAddFriendView AddFriendView { get; }
        IFriendsListView FriendsListView { get; }
        IRequestListView RequestListView { get; }
        IBlockedListView BlockListView { get; }

        void Init();
    }
}
