using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class OptionsData
{
    public bool isAudioPlaying;
    public bool isGuyExercising;
    public string savedTime, savedDate;
}
public class Options : MonoBehaviour
{
    [SerializeField]
    JsonReadWriteSystem jsonFile;
    [SerializeField]
    AudioSource speakerSource;
    [SerializeField]
    GameObject exercisingGuy;

    [SerializeField]
    GameObject mainMenuCanvas, optionsCanvas, creditsCanvas;
    GameManager gameManagerInstance;
    private void Start()
    {
        //jsonFile.LoadFromJson();
        //jsonFile.LoadCloudData();
        gameManagerInstance = GameManager.Instance;
    }
    public void ToggleMusic(bool tog)
    {
        if(tog)
        speakerSource.Play();
        else
        speakerSource.Stop();
    }

    public void ToggleExercisingGuy(bool tog)
    {
        if (tog)
            exercisingGuy.SetActive(true);
        else
            exercisingGuy.SetActive(false);
    }

    public void GoBack()
    {
        // jsonFile.SaveToJson();
        optionsCanvas.SetActive(false);
        jsonFile.SaveToCloud(out bool authenticated, gameManagerInstance.SaveSystem.OnSaveError);
       // if (!authenticated)
       //     return;
        //mainMenuCanvas.SetActive(true);
    }
    public void GoToOptions()
    {
        mainMenuCanvas.SetActive(false);
        optionsCanvas.SetActive(true);
    }
    public void GoToOptionsFromCredits()
    {
        optionsCanvas.SetActive(true);
        creditsCanvas.SetActive(false);
    }
    public void GoToCredits()
    {
        optionsCanvas.SetActive(false);
        creditsCanvas.SetActive(true);
    }

    public void CreditsOpenURL(string urlString)
    {
        Application.OpenURL(urlString);
    }
}
