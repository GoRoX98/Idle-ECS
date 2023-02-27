using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Static Data", menuName = "Game/New StaticData")]
public class StaticData : ScriptableObject
{
    [SerializeField] private GameObject _buisnessPrefab;
    [SerializeField] private List<Buisness> _buisnesses;
    public GameObject Prefab => _buisnessPrefab;
    public List<Buisness> Buienesses => _buisnesses;
}
