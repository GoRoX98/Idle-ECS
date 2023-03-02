using UnityEngine;
using Leopotam.Ecs;

namespace Client
{
    public class CardEventsSystem : IEcsInitSystem, IEcsDestroySystem
    {
        private EcsWorld _world;

        public void Init()
        {
            CardEvents.ClickLvlUpgrade += LvlUP;
            CardEvents.ClickFirstUpgrade += FirstUpgrade;
            CardEvents.ClickSecondUpgrade += SecondUpgrade;
        }

        public void Destroy()
        {
            CardEvents.ClickLvlUpgrade -= LvlUP;
            CardEvents.ClickFirstUpgrade -= FirstUpgrade;
            CardEvents.ClickSecondUpgrade -= SecondUpgrade;
        }

        public void LvlUP(int id)
        {
            EcsEntity upgradeEntity = _world.NewEntity();
            ref UpgradeComponent upgrade = ref upgradeEntity.Get<UpgradeComponent>();

            upgrade = new UpgradeComponent(id, EventEnum.LvlUP);
            Debug.Log("lvl up");
        }

        public void FirstUpgrade(int id)
        {
            EcsEntity upgradeEntity = _world.NewEntity();
            ref UpgradeComponent upgrade = ref upgradeEntity.Get<UpgradeComponent>();

            upgrade = new UpgradeComponent(id, EventEnum.FirstUpgrade);
            Debug.Log("FirstUpgrade");
        }

        public void SecondUpgrade(int id)
        {
            EcsEntity upgradeEntity = _world.NewEntity();
            ref UpgradeComponent upgrade = ref upgradeEntity.Get<UpgradeComponent>();

            upgrade = new UpgradeComponent(id, EventEnum.SecondUpgrade);
            Debug.Log("SecondUpgrade");
        }

    }
}

