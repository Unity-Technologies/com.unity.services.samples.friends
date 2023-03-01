using UnityEngine;

namespace Unity.Services.Samples.Friends
{

    [System.Serializable]
    public class UGSPlayer : IUGSPlayer
    {
        //Decorating with [field: SerializeField] is shorthand for:
        //  public string Name => m_Name;
        //  [SerializeField]
        //  string m_Name;
        [field: SerializeField] public string Name { get; private set; }

        [field: SerializeField] public string Id { get; private set; }
        public void SetName(string newName)
        {
            Name = newName;
        }

        public UGSPlayer(string name, string id)
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
