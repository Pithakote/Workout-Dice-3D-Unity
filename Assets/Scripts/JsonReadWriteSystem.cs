using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
public class JsonReadWriteSystem : MonoBehaviour
{
    [SerializeField]
    Toggle audioToggle, exerciseGuyToggle;
    OptionsData dataHoldingObject;
    public void SaveToJson()
    {
        OptionsData oData = new OptionsData();
        oData.isAudioPlaying = audioToggle.isOn;
        oData.isGuyExercising = exerciseGuyToggle.isOn;

        string path = Application.persistentDataPath + "/data.qnd";
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fs = new FileStream(path, FileMode.Create);
        formatter.Serialize(fs,oData);
        fs.Close();

        //string json = JsonUtility.ToJson(oData, true);
        //File.WriteAllText(Application.dataPath + "/OptionDataFile.json", json);
    }

    public void LoadFromJson()
    {
        if (!File.Exists(Application.persistentDataPath + "/data.qnd"))
        {
            return;
        }
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fs = new FileStream(Application.persistentDataPath + "/data.qnd", FileMode.Open);
        dataHoldingObject = formatter.Deserialize(fs) as OptionsData;
        fs.Close();
       // string json = File.ReadAllText(Application.dataPath + "/OptionDataFile.json");
       // OptionsData oData = JsonUtility.FromJson<OptionsData>(json);

        audioToggle.isOn = dataHoldingObject.isAudioPlaying;
        exerciseGuyToggle.isOn = dataHoldingObject.isGuyExercising;
    }
}
