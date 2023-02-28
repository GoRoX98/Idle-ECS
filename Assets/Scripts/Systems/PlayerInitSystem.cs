using Leopotam.Ecs;

namespace Client {
    sealed class PlayerInitSystem : IEcsInitSystem 
    {
        private EcsWorld _world;
        private StaticData _staticData;
        private SceneData _sceneData;

        public void Init () {
            EcsEntity playerEntity = _world.NewEntity();

            ref Player player = ref playerEntity.Get<Player>();

            
        }
    }
}