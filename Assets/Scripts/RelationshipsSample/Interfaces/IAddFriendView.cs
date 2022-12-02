using System;

namespace Unity.Services.Toolkits.Relationships
{
    public interface IAddFriendView
    {
        void FriendRequestSuccess();
        void FriendRequestFailed();
        Action<string> onFriendRequestSent { get; set; }
        void Show();
        void Hide();
    }

}

