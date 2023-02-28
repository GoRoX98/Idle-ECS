using UnityEngine;

[CreateAssetMenu(fileName = "Buisness", menuName = "Game/New Buisness")]
public class Buisness : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private BuisnessConfig _buisnessConfiguration;

    public string Name => _name;
    public BuisnessConfig BuisnessConfiguration => _buisnessConfiguration;
}
