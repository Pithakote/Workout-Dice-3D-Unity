using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using GooglePlayGames.BasicApi.SavedGame;
using GooglePlayGames.BasicApi.Events;
public class JsonReadWriteSystem : MonoSingleton<JsonReadWriteSystem>
{
    [SerializeField]
    Toggle audioToggle, exerciseGuyToggle;
    OptionsData dataHoldingObject, oData;
    BinaryFormatter formatter;
    public Action<SavedGameRequestStatus> OnSave;
    public Action<SavedGameRequestStatus> OnLoad;
    private void Awake()
    {
        formatter = new BinaryFormatter();
        oData = new OptionsData();
    }

    public void LoadCloudData()
    {
        LoadFromCloud();
    }
    public void SaveToJson()
    {
      //  OptionsData oData = new OptionsData();
        oData.isAudioPlaying = audioToggle.isOn;
        oData.isGuyExercising = exerciseGuyToggle.isOn;
        oData.savedTime = DateTime.Now.TimeOfDay.ToString();
        string path = Application.persistentDataPath + "/data.qnd";
       // BinaryFormatter formatter = new BinaryFormatter();
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
      //  BinaryFormatter formatter = new BinaryFormatter();
        FileStream fs = new FileStream(Application.persistentDataPath + "/data.qnd", FileMode.Open);
        dataHoldingObject = formatter.Deserialize(fs) as OptionsData;
        fs.Close();
        // string json = File.ReadAllText(Application.dataPath + "/OptionDataFile.json");
        // OptionsData oData = JsonUtility.FromJson<OptionsData>(json);

        audioToggle.isOn = dataHoldingObject.isAudioPlaying;
        exerciseGuyToggle.isOn = dataHoldingObject.isGuyExercising;
    }

    public byte[] SaveToByte()
    {
        using (MemoryStream ms = new MemoryStream())
        {
            //OptionsData oData = new OptionsData();
            oData.isAudioPlaying = audioToggle.isOn;
            oData.isGuyExercising = exerciseGuyToggle.isOn;
            oData.savedTime = DateTime.Now.TimeOfDay.ToString();

            //  BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms, oData);
            return ms.GetBuffer();
        }
    }

    public OptionsData LoadFromByte(byte [] data)
    {
      //  BinaryFormatter formatter = new BinaryFormatter();

        using (MemoryStream ms = new MemoryStream(data))
        {
            return (OptionsData)formatter.Deserialize(ms);
        }
    }

    //google cloud
    public void SaveToCloud(Action<PlayServiceError> errorCalback = null)
    {
        GPGSManager.instance.OpenCloudSave(OnSaveResponse, errorCalback);
    }

    void OnSaveResponse(SavedGameRequestStatus status, ISavedGameMetadata meta)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            byte [] data = SaveToByte();
            SavedGameMetadataUpdate update = new SavedGameMetadataUpdate.Builder()
            .WithUpdatedDescription("Last Save: " + DateTime.Now.ToString()).Build();

            var platform = (PlayGamesPlatform)Social.Active;
            platform.SavedGame.CommitUpdate(meta, update, data, SaveCallback);
        }
        else
            OnSave?.Invoke(status);
    }
    void SaveCallback(SavedGameRequestStatus status, ISavedGameMetadata meta)
    {
        OnSave?.Invoke(status);
    }

    public void LoadFromCloud(Action<PlayServiceError> errorCalback = null)
    {
        GPGSManager.instance.OpenCloudSave(OnLoadResponse, errorCalback);
    }
    void OnLoadResponse(SavedGameRequestStatus status, ISavedGameMetadata meta)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            var platform = (PlayGamesPlatform)Social.Active;
            platform.SavedGame.ReadBinaryData(meta, LoadCallback);
        }
        else
            OnLoad?.Invoke(status);
    }
    void LoadCallback(SavedGameRequestStatus status, byte [] data)
    {
        oData = LoadFromByte(data);
        audioToggle.isOn = oData.isAudioPlaying;
        exerciseGuyToggle.isOn = oData.isGuyExercising;

        OnLoad?.Invoke(status);
    }

}
