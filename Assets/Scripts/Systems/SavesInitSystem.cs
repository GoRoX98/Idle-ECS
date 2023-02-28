using Leopotam.Ecs;

namespace Client {
    sealed class SavesInitSystem : IEcsInitSystem {
        // auto-injected fields.
        private EcsWorld _world = null;
        private StaticData _staticData;
        private SceneData _sceneData;

        public void Init () {
            EcsEntity saveEntity = _world.NewEntity();

            ref SaveComponent save = ref saveEntity.Get<SaveComponent>();
        }
    }
}