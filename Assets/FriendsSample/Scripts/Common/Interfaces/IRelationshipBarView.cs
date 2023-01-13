using System;

namespace Unity.Services.Toolkits.Friends
{
    public interface IRelationshipBarView
    {
        Action onShowAddFriend { get; set; }
        void Refresh();
    }


}

