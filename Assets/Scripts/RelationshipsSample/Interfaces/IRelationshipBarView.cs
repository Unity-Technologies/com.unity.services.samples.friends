using System;
using Unity.Services.Friends.Models;

namespace UnityGamingServicesUsesCases.Relationships
{
    public interface IRelationshipBarView
    {
        Action onShowAddFriend { get; set; }
        void Refresh();
    }


}

