using UnityEngine;

namespace Unity.Services.Toolkits.Friends
{

    [System.Serializable]
    public class PlayerProfile
    {
        //Decorating with [field: SerializeField] is shorthand for something like:
        //  public string Name => m_Name;
        //  [SerializeField]
        //  string m_Name;
        [field: SerializeField] public string Name { get; private set; }

        [field: SerializeField] public string Id { get; private set; }
        
        public PlayerProfile(string name, string id)
        {
            Name = name;
            Id = id;
        }

        public override string ToString()
        {
            return $"{Name} , Id :{Id}";
        }
    }
}
