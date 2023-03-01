using UnityEngine;

[CreateAssetMenu(fileName = "Buisness", menuName = "Game/New Buisness")]
[System.Serializable]
public class Buisness : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private int _iD;
    [SerializeField] private BuisnessConfig _buisnessConfiguration;

    public string Name => _name;
    public int ID => _iD;
    public BuisnessConfig BuisnessConfiguration => _buisnessConfiguration;
}
