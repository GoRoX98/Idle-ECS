using Leopotam.Ecs;
using UnityEngine;

namespace Client {
    sealed class BuisnessesInitSystem : IEcsInitSystem 
    {
        private EcsWorld _world;
        private StaticData _staticData;
        private SceneData _sceneData;

        public void Init () {
            foreach(var buis in _staticData.Buienesses)
            {
                EcsEntity buisnessEntity = _world.NewEntity();
                ref BuisnessComponent buisness = ref buisnessEntity.Get<BuisnessComponent>();

                buisness.Buisness = buis;

                GameObject buisObj = Object.Instantiate(_staticData.Prefab, _sceneData.BuisnessesPanel.transform);
                BuisnessCard card = buisObj.GetComponent<BuisnessCard>();

                card.name = buisness.Buisness.name;
            }
        }
    }
}