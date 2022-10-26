using System;

namespace UnityGamingServicesUsesCases.Relationships
{
    public interface IRelationshipBarView
    {
        Action onShowAddFriend { get; set; }
        void Refresh();
    }


}

