using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DatabaseObjectSO : ScriptableObject
{
    public List<ObjectData> ObjectsData;

    [System.Serializable]
    public class ObjectData
    {

        [field: SerializeField] private int id;
        [field: SerializeField] private string name1;
        [field: SerializeField] private Sprite icon;
        [field: SerializeField] private Vector2Int size = Vector2Int.one;
        [field: SerializeField] private Construct prefab;

        public int Id { get => id; }
        public string Name { get => name1; }
        public Vector2Int Size { get => size; }
        public Construct Construct { get => prefab; }
        public Sprite Icon { get => icon;  }
    }
}
