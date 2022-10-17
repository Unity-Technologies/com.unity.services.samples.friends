using System;

namespace UnityGamingServicesUsesCases.Relationships
{
    public interface IRequestFriendView
    {
        void RequestFriendSuccess();
        void RequestFriendFailed();
        Action<string> tryAddFriend { get; set; }
        bool IsShowing { get; }
        void Show();
        void Hide();
    }

}

