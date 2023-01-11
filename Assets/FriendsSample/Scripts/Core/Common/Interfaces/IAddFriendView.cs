using System;

namespace Unity.Services.Toolkits.Friends
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

