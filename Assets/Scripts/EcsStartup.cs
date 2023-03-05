using Leopotam.Ecs;
using Leopotam.Ecs.Ui.Systems;
using UnityEngine;

namespace Client {
    sealed class EcsStartup : MonoBehaviour 
    {
        [SerializeField] private StaticData _staticData;
        [SerializeField] private SceneData _sceneData;
        EcsWorld _world;
        EcsSystems _systems;

        void Start () 
        {
            
            _world = new EcsWorld ();
            _systems = new EcsSystems (_world);
#if UNITY_EDITOR
            Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create (_world);
            Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create (_systems);
#endif

            _systems
                .Add(new SavesSystem())
                .Add(new PlayerSystem())
                .Add(new BuisnessesSystem())
                .Add(new CardEventsSystem())
                .Add(new UpgradeSystem())
                .Inject(_staticData)
                .Inject(_sceneData)
                .Init ();

            
        }

        void FixedUpdate () {
            _systems?.Run ();
        }

        private void OnApplicationQuit()
        {
            SavesSystem.OnGameSaveEvent?.Invoke();
        }

        private void OnApplicationFocus(bool focus)
        {
            if(!focus)
                SavesSystem.OnGameSaveEvent?.Invoke();
        }

        void OnDestroy () {
            if (_systems != null) {
                _systems.Destroy ();
                _systems = null;
                _world.Destroy ();
                _world = null;
            }
        }
    }
}