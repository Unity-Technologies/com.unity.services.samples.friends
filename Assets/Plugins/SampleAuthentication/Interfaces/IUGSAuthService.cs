using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
namespace Unity.Services.Samples
{

   public interface IUGSAuthService
   {
      IUGSPlayer LocalPlayer { get; }
      Task TryAuthenticate(string profileName="player");
      bool IsSignedIn();
   }
}

