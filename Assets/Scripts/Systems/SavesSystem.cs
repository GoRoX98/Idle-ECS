using Leopotam.Ecs;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Client 
{
    sealed class SavesSystem : IEcsInitSystem, IEcsDestroySystem
    {
        // auto-injected fields.
        private EcsWorld _world;
        private StaticData _staticData;
        private SceneData _sceneData;
        private EcsFilter<SaveComponent> _filterSaveData;
        private EcsFilter<Player> _filterPlayer;
        private EcsFilter<BuisnessComponent> _filterBuisnesses;

        public delegate void GameSaveEvent();
        public static GameSaveEvent OnGameSaveEvent;


        #region EcsMethods

        public void Init () {
            EcsEntity saveEntity = _world.NewEntity();

            ref SaveComponent save = ref saveEntity.Get<SaveComponent>();

            if (_staticData.Reset || !CheckSave())
                FirstStart();
            else
                Load();

            OnGameSaveEvent += Save;
        }

        public void Destroy()
        {
            OnGameSaveEvent?.Invoke();
            OnGameSaveEvent -= Save;
        }

        #endregion

        public bool CheckSave()
        {
            return File.Exists(Application.persistentDataPath + "/GameSave.dat");
        }

        private void FirstStart()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file;

            if(CheckSave())
                File.Delete(Application.persistentDataPath + "/GameSave.dat");

            file = File.Create(Application.persistentDataPath + "/GameSave.dat");

            _filterSaveData.Get1(0).Money = _staticData.StartMoney;
            _filterSaveData.Get1(0).Buisnesses = null;

            bf.Serialize(file, _filterSaveData.Get1(0));
            file.Close();
        }

        public void Save()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file;

            if (CheckSave())
                file = File.Open(Application.persistentDataPath + "/GameSave.dat", FileMode.Open);
            else
                file = File.Create(Application.persistentDataPath + "/GameSave.dat");

            _filterSaveData.Get1(0).Money = _filterPlayer.Get1(0).Money;
            _filterSaveData.Get1(0).Buisnesses = new List<BuisnessComponent>();
            foreach (var i in _filterBuisnesses)
            {
                _filterSaveData.Get1(0).Buisnesses.Add(_filterBuisnesses.Get1(i));
            }

            bf.Serialize(file, _filterSaveData.Get1(0));
            file.Close();
        }

        private void Load()
        {
            if (!CheckSave())
            {
                Debug.Log("Save not found");
                return;
            }

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/GameSave.dat", FileMode.Open);
            _filterSaveData.Get1(0) = (SaveComponent)bf.Deserialize(file);
            file.Close();

            _filterPlayer.Get1(0) = new Player(_filterSaveData.Get1(0).Money);
            foreach (var i in _filterBuisnesses)
            {
                _filterBuisnesses.Get1(i) = _filterSaveData.Get1(0).Buisnesses[i];
            }
        }
    }
}