using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System;
using Newtonsoft.Json;
[System.Serializable]
public class SaveSystem
{
    private string saveFile;
    public object GetData(string filename)
    {
        if (File.Exists(Application.persistentDataPath + "\\" + filename + ".json"))
        {

            string fileContents = File.ReadAllText(Application.persistentDataPath + "\\" + filename + ".json");

            object _tempLoadListData = JsonConvert.DeserializeObject<object>(fileContents);

            return _tempLoadListData;
        }
        return null;
    }

    public void SaveData(System.Object save, string filename)
    {
        saveFile = Application.persistentDataPath + "\\" + filename + ".json";
        string jsonString = JsonConvert.SerializeObject(save);


        File.WriteAllText(saveFile, jsonString);



    }
}
