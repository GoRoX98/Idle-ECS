using UnityEngine;
using Leopotam.Ecs;

namespace Client
{
    public class CardEventsSystem : IEcsInitSystem, IEcsDestroySystem
    {
        private EcsWorld _world;

        public void Init()
        {
            CardEvents.ClickLvlUpgrade += Upgrade;
            CardEvents.ClickFirstUpgrade += Upgrade;
            CardEvents.ClickSecondUpgrade += Upgrade;
        }

        public void Destroy()
        {
            CardEvents.ClickLvlUpgrade -= Upgrade;
            CardEvents.ClickFirstUpgrade -= Upgrade;
            CardEvents.ClickSecondUpgrade -= Upgrade;
        }

        private void Upgrade(int id, EventEnum type)
        {
            EcsEntity upgradeEntity = _world.NewEntity();
            ref UpgradeComponent upgrade = ref upgradeEntity.Get<UpgradeComponent>();

            upgrade = new UpgradeComponent(id, type);
        }

    }
}

