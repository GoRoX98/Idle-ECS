using Leopotam.Ecs;
using UnityEngine;


namespace Client {
    sealed class PlayerInitSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private SceneData _sceneData;

        private EcsFilter<IncomeComponent> _filterIncome;
        private EcsFilter<Player> _filterPlayer;
        private EcsFilter<SaveComponent> _filterSave;

        public void Init () {
            EcsEntity playerEntity = _world.NewEntity();

            ref Player player = ref playerEntity.Get<Player>();

            if (!_filterSave.IsEmpty())
                player.Money = _filterSave.Get1(0).Money;

            _sceneData.BalanceUI.text = "Balance: " + player.Money.ToString() + "$";
        }

        public void Run()
        {
            if(!_filterIncome.IsEmpty())
            {
                _filterPlayer.Get1(0).Money += _filterIncome.Get1(0).Money;
                Debug.Log("New Income: " + _filterIncome.Get1(0).Money);
                _filterIncome.GetEntity(0).Destroy();
                UpdateBalance();
            }
        }

        private void UpdateBalance()
        {
            _sceneData.BalanceUI.text = "Balance: " + _filterPlayer.Get1(0).Money + "$";
        }
    }
}