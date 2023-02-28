using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Client 
{
    struct SaveComponent 
    {
        SaveData GameSave;

        public bool CheckSave()
        {
            return File.Exists(Application.persistentDataPath + "/GameSave.dat");
        }

        public void Save()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file;
            if (CheckSave())
                file = File.Open($"{Application.persistentDataPath} + /GameSave.dat", FileMode.Open);
            else
                file = File.Create(Application.persistentDataPath + "/GameSave.dat");


        }
    }
}