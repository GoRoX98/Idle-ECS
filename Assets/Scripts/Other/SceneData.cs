using TMPro;
using UnityEngine;

public class SceneData : MonoBehaviour
{
    [SerializeField] private GameObject _buisnessesPanel;
    [SerializeField] private TextMeshProUGUI _playerBalanceUI;
    public TextMeshProUGUI BalanceUI => _playerBalanceUI;
    public GameObject BuisnessesPanel => _buisnessesPanel;
}
