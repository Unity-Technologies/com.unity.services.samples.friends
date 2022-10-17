using System;

namespace UnityGamingServicesUsesCases.Relationships
{
    public interface IRequestFriendView
    {
        void RequestFriendSuccess();
        void RequestFriendFailed();
        Action<string> tryRequestFriend { get; set; }
        void Show();
        void Hide();
    }

}

