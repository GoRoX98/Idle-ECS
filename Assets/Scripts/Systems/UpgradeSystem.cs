using Leopotam.Ecs;
using UnityEngine;

namespace Client 
{
    sealed class UpgradeSystem : IEcsRunSystem 
    {
        readonly EcsWorld _world;
        private StaticData _staticData;

        private EcsFilter<UpgradeComponent> _filterUpgrades;
        private EcsFilter<Player> _filterPlayer;
        private EcsFilter<BuisnessComponent> _filterBuisnesses;

        public void Run() 
        {
            if(!_filterUpgrades.IsEmpty())
            {
                Buisness buis = _staticData.Buienesses.Find(match => match.ID == _filterUpgrades.Get1(0).BuisnessID);
                ref BuisnessComponent bc = ref _filterBuisnesses.Get1(FindBCID(buis.ID));
                int cost = 0;
                if(CheckAvaible(buis, bc, _filterUpgrades.Get1(0).TypeUpgrade, ref cost))
                {
                    Upgrade(ref bc, cost, _filterUpgrades.Get1(0).TypeUpgrade);
                    _filterUpgrades.GetEntity(0).Destroy();
                }
                else
                    _filterUpgrades.GetEntity(0).Destroy();
            }
        }

        private void Upgrade(ref BuisnessComponent bc, int cost, EventEnum type)
        {
            ref IncomeComponent income = ref _world.NewEntity().Get<IncomeComponent>();
            income = new IncomeComponent(-cost);

            switch (type)
            {
                case EventEnum.LvlUP:
                    bc.CurrentLVL += 1;
                    break;
                case EventEnum.FirstUpgrade:
                    bc.FirstUpgraded = true;
                    break;
                case EventEnum.SecondUpgrade:
                    bc.SecondUpgraded = true;
                    break;
            }

            _world.NewEntity().Get<UpgradeSuccessComponentFlag>();
        }

        private bool CheckAvaible(Buisness buis, BuisnessComponent bc, EventEnum type, ref int cost)
        {
            int money = _filterPlayer.Get1(0).Money;

            switch (type)
            {
                case EventEnum.LvlUP:
                    cost = (bc.CurrentLVL + 1) * buis.BuisnessConfiguration.BaseCost;
                    break;
                case EventEnum.FirstUpgrade:
                    cost = buis.BuisnessConfiguration.FirstUpgrade.Cost;
                    if (bc.FirstUpgraded)
                        return false;
                    break;
                case EventEnum.SecondUpgrade:
                    cost = buis.BuisnessConfiguration.SecondUpgrade.Cost;
                    if (bc.SecondUpgraded)
                        return false;
                    break;
            }

            return money >= cost ? true : false; ;
        }

        private int FindBCID(int id)
        {
            foreach (var i in _filterBuisnesses)
            {
                if (id == _filterBuisnesses.Get1(i).BuisnessID)
                {
                    return i;
                }
            }

            return 0;
        }
    }
}