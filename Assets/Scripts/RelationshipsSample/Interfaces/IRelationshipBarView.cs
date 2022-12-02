using System;

namespace Unity.Services.Toolkits.Relationships
{
    public interface IRelationshipBarView
    {
        Action onShowAddFriend { get; set; }
        void Refresh();
    }


}

