using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class OptionsData
{
    public bool isAudioPlaying;
    public bool isGuyExercising;
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
    GameObject mainMenuCanvas, optionsCanvas;

    private void Start()
    {
        jsonFile.LoadFromJson();
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
        jsonFile.SaveToJson();
        mainMenuCanvas.SetActive(true);
        optionsCanvas.SetActive(false);
    }
    public void GoToOptions()
    {
        mainMenuCanvas.SetActive(false);
        optionsCanvas.SetActive(true);
    }
}
