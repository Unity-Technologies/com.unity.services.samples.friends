using UnityEditor;
using UnityEngine;

namespace UnityGamingServicesUsesCases.Relationships
{
    public static class RelationshipsSampleTools
    {

        private const int PlayerAmount = 5;

        [MenuItem("RelationshipsSampleTools/Print Player Ids")]
        public static void Print()
        {
            for (int i = 0; i < PlayerAmount; i++)
            {
                var name = $"Player_{i}";
                var id = PlayerPrefs.GetString(name);
                Debug.Log($"<b>{name}</b> has Id: <b>{id}</b>");
            }
        }

        [MenuItem("RelationshipsSampleTools/Delete Player Prefs")]
        public static void DeletePlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
            Debug.Log($"All Player Prefs Deleted");
        }
    }
}