using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;

namespace Client {
    sealed class BuisnessesInitSystem : IEcsInitSystem 
    {
        private EcsWorld _world;
        private StaticData _staticData;
        private SceneData _sceneData;

        public void Init () 
        {
            RectTransform bpanel = _sceneData.BuisnessesPanel.GetComponent<RectTransform>();
            float cardHeight = _staticData.Prefab.GetComponent<RectTransform>().rect.height + _sceneData.BuisnessesPanel.GetComponent<VerticalLayoutGroup>().spacing;

            foreach (var buis in _staticData.Buienesses)
            {
                EcsEntity buisnessEntity = _world.NewEntity();
                ref BuisnessComponent buisness = ref buisnessEntity.Get<BuisnessComponent>();

                buisness.Buisness = buis;

                GameObject buisObj = Object.Instantiate(_staticData.Prefab, _sceneData.BuisnessesPanel.transform);
                BuisnessCard card = buisObj.GetComponent<BuisnessCard>();

                bpanel.sizeDelta += new Vector2(0, cardHeight);
            }
        }

        private void LoadBuisnessData(BuisnessComponent bc)
        {
            
        }

        private void UpdateBuisnessCard(BuisnessCard card, BuisnessComponent buis)
        {
            card.BuisnessName.text = buis.Buisness.name;
        }
    }
}