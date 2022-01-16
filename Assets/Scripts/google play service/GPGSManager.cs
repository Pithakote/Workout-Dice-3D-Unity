using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;
using TMPro;
using GooglePlayGames.BasicApi.SavedGame;
using System;

public class GPGSManager : MonoSingleton<GPGSManager>
{
    [SerializeField] bool enableSaveGame = true;
    [SerializeField] string cloudSaveName = "workoutdice3dSaveFile";
    [SerializeField] DataSource dataSource;
    [SerializeField] ConflictResolutionStrategy conflictStrategy;
    [SerializeField] TMP_Text statusText, descriptionText;

    [SerializeField]
    GameManager instance;

    private void Awake()
    {
        Debug.Log("Hour of day is: "+ DateTime.Now.ToString("HH"));
        Debug.Log("Minute of day is: "+ DateTime.Now.ToString("mm"));
        Debug.Log("Date day is: " + DateTime.Now.Date.ToString("dd"));
        Debug.Log("Date month is: " + DateTime.Now.Date.ToString("MM"));
        Debug.Log("Date year is: " + DateTime.Now.Date.ToString("yyyy"));
        PlayGamesClientConfiguration.Builder builder = new PlayGamesClientConfiguration.Builder();

        //for enabling save games
        if (enableSaveGame)
            builder.EnableSavedGames();

        PlayGamesPlatform.InitializeInstance(builder.Build());
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }
    private void Start()
    {
        instance = GameManager.Instance;
#if UNITY_ANDROID
        SignIn();
#endif
    }
    public void SignIn(Action successCallback = null, Action errorCallback = null)
    {
        try
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    successCallback?.Invoke();
                    instance.SaveSystem.LoadCloudData();
                }
            });
        }
        catch (Exception e)
        {
            Debug.Log(e);
            errorCallback?.Invoke();
        }
    }

    public void SignOut()
    {
        if (Social.localUser.authenticated)
            PlayGamesPlatform.Instance.SignOut();
    }

    public void OpenCloudSave(Action<SavedGameRequestStatus, ISavedGameMetadata> callback, Action<PlayServiceError> errorCallback = null)
    {
        PlayServiceError error = global::PlayServiceError.None;
        if (!Social.localUser.authenticated)
            error |= global::PlayServiceError.NotAuthenticated;
        if(PlayGamesClientConfiguration.DefaultConfiguration.EnableSavedGames)
            error |= global::PlayServiceError.SaveGameNotEnabled;
        if(string.IsNullOrWhiteSpace(cloudSaveName))
            error |= global::PlayServiceError.CloudSaveNameNotSet;
        if (error != global::PlayServiceError.None)
            errorCallback?.Invoke(error);

        var platform = (PlayGamesPlatform)Social.Active;
     
        //string finalSaveName = cloudSaveName +
        //                        "Date"+ DateTime.Now.Date.ToString("dd")+ DateTime.Now.Date.ToString("MM")+ DateTime.Now.Date.ToString("yyyy")+
        //                        "Time"+ DateTime.Now.ToString("HH") + DateTime.Now.ToString("mm");
        platform.SavedGame.OpenWithAutomaticConflictResolution(cloudSaveName, dataSource, conflictStrategy, callback);
    }
}

public enum PlayServiceError : byte
{ 
    None = 0,
    TimeOut = 1,
    NotAuthenticated = 2,
    SaveGameNotEnabled = 4,
    CloudSaveNameNotSet = 8,
}
