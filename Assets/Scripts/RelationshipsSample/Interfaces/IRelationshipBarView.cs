using System;
using Unity.Services.Friends.Models;

namespace UnityGamingServicesUsesCases.Relationships
{
    public interface IRelationshipBarView
    {
        Action onShowRequestFriend { get; set; }
        void Refresh();
    }


}

