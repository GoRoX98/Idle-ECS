using Leopotam.Ecs;
using UnityEngine;

namespace Client {
    sealed class EcsStartup : MonoBehaviour 
    {
        [SerializeField] private StaticData _staticData;
        [SerializeField] private SceneData _sceneData;
        EcsWorld _world;
        EcsSystems _systems;

        void Start () {
            // void can be switched to IEnumerator for support coroutines.
            
            _world = new EcsWorld ();
            _systems = new EcsSystems (_world);
            RuntimeData runtimeData = new RuntimeData();
#if UNITY_EDITOR
            Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create (_world);
            Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create (_systems);
#endif

            _systems
                .Add(new SavesSystem())
                .Add(new PlayerInitSystem())
                .Add(new BuisnessesSystem())
                .Inject(_staticData)
                .Inject(_sceneData)
                .Init ();
        }

        void FixedUpdate () {
            _systems?.Run ();
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