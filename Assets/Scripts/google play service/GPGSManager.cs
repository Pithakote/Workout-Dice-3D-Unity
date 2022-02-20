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
using UnityEngine.Networking;
public class GPGSManager : MonoSingleton<GPGSManager>
{
    [SerializeField] bool enableSaveGame = true;
    [SerializeField] string cloudSaveName = "workoutdice3dSaveFile";
    [SerializeField] DataSource dataSource;
    [SerializeField] ConflictResolutionStrategy conflictStrategy;
    [SerializeField] TMP_Text statusText, descriptionText;

    [SerializeField]
    GameManager instance;
    [SerializeField]
    LevelManager levelManager;

    Action SigningInError, SigningInSuccess;
    public Action<GameObject , TMP_Text , string > NoInternetError;
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

        //instance.SaveSystem.OnSave += AfterSave;
        //instance.SaveSystem.OnLoad += AfterLoad;
        instance.SaveSystem.OnLoadError += OnLoadPlayServiceError;
        instance.SaveSystem.OnSaveError += OnSavePlayServiceError;
        SigningInError += SigningInErrorVoid;
        SigningInSuccess += SigningInSuccessVoid;
        NoInternetError += OnNoInternetError;
        levelManager = instance.LevelManager;
//#if UNITY_ANDROID
        SignIn(SigningInSuccess, SigningInError);
//#endif
    }
    #region ErrorHandling
    void OnNoInternetError(GameObject ErrorUI, TMP_Text textToSetTo, string text)
    {
        levelManager.ToggleInformationScreens(levelManager.MainMenuUI, false);
        levelManager.ToggleInformationScreens(ErrorUI, true);
        levelManager.SetText(textToSetTo,text);
    }
    void AfterSave(SavedGameRequestStatus status)
    {
        switch (status)
        {
            case SavedGameRequestStatus.Success:
                {
                    levelManager.ToggleInformationScreens(levelManager.SavingScreen, false);
                    //levelManager.ToggleInformationScreens(levelManager.MainMenuUI, true);
                    break;
                }
            default:
                {
                    levelManager.SetText(levelManager.SavingErrorText, status.ToString());
                    levelManager.ToggleInformationScreens(levelManager.SavingScreen, false);
                    levelManager.ToggleInformationScreens(levelManager.SavingScreenUnsucessful, true);
                    break;
                }
        }
    }

    void AfterLoad(SavedGameRequestStatus status)
    {
        switch (status)
        {
            
            case SavedGameRequestStatus.Success:
                {
                    levelManager.ToggleInformationScreens(levelManager.LoadingDataScreen, false);
                    //levelManager.ToggleInformationScreens(levelManager.MainMenuUI, true);
                    break;
                }
            default:
                {
                    levelManager.SetText(levelManager.LoadingErrorText, status.ToString());
                    levelManager.ToggleInformationScreens(levelManager.LoadingDataScreen, false);
                    levelManager.ToggleInformationScreens(levelManager.LoadingDataUnsuccessful, true);
                    break;
                }
        }
    }

    void OnLoadPlayServiceError(PlayServiceError error)
    {
        levelManager.SetText(levelManager.LoadingErrorText, error.ToString());
        levelManager.ToggleInformationScreens(levelManager.LoadingDataUnsuccessful, true);
    }
    void OnSavePlayServiceError(PlayServiceError error)
    {
        levelManager.SetText(levelManager.SavingErrorText, error.ToString());
        levelManager.ToggleInformationScreens(levelManager.SavingScreenUnsucessful, true);
    }
    void SigningInErrorVoid()
    {
        levelManager.ToggleInformationScreens(levelManager.SigningIn, false);
        levelManager.ToggleInformationScreens(levelManager.SigningInUnsuccessful, true);
    }
    void SigningInSuccessVoid()
    {
        Debug.Log("SigningInSuccessVoid called");
        levelManager.ToggleInformationScreens(levelManager.SigningIn, false);
        levelManager.ToggleInformationScreens(levelManager.MainMenuUI, true);
    }
    #endregion
    public void SignIn(Action successCallback = null, Action errorCallback = null)
    {
        levelManager.ToggleInformationScreens(levelManager.SigningIn, true);
        try
        {

            PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptOnce, (code) =>
            {
                if (code == SignInStatus.Success)
                {
                    levelManager.ToggleInformationScreens(levelManager.SigningIn, false);
                    Debug.Log("SigningInSuccessVoid called befoe invoke");
                    successCallback?.Invoke();
                    Debug.Log("SigningInSuccessVoid called after invoke");
                    instance.SaveSystem.LoadCloudData();
                }
                else
                {
                    levelManager.ToggleInformationScreens(levelManager.SigningIn, false);
                    levelManager.ToggleInformationScreens(levelManager.AuthenticationFailedUI, true);
                    Debug.Log("Unsuccessful in authenticating");
                }
            }
            );
        }
        catch (Exception e)
        {
            Debug.Log("SIGNING IN EXCEPTION: "+e);
            errorCallback?.Invoke();
        }
    }

    public void SignOut()
    {
        if (Social.localUser.authenticated)
            PlayGamesPlatform.Instance.SignOut();
    }

    public void OpenCloudSave(GameObject ErrorUI, TMP_Text saveloaderrorText, out bool isAuthenticated, Action<SavedGameRequestStatus, ISavedGameMetadata> callback, Action<PlayServiceError> errorCallback = null)
    {
      


        PlayServiceError error = global::PlayServiceError.None;
        if (!PlayGamesPlatform.Instance.IsAuthenticated())
        {
            error |= global::PlayServiceError.NotAuthenticated;
            errorCallback?.Invoke(error);
            isAuthenticated = false;
            return;
        }
        else
            isAuthenticated = true;

        if (PlayGamesClientConfiguration.DefaultConfiguration.EnableSavedGames)
            error |= global::PlayServiceError.SaveGameNotEnabled;
        if(string.IsNullOrWhiteSpace(cloudSaveName))
            error |= global::PlayServiceError.CloudSaveNameNotSet;
        if (error != global::PlayServiceError.None)
            errorCallback?.Invoke(error);

        var platform = (PlayGamesPlatform)Social.Active;
        platform.SavedGame.OpenWithAutomaticConflictResolution(cloudSaveName, dataSource, conflictStrategy, callback);

        StartCoroutine(CheckInternet((isConnected) =>
        {
            if (!isConnected)
            {
                NoInternetError.Invoke(ErrorUI, saveloaderrorText, "Internet not available. Saving and Loading will not work on your account");
                return;
            }
            else
            {
                levelManager.ToggleInformationScreens(levelManager.MainMenuUI, true);
                Debug.Log("Connected to internet");
            }
        }));
    }

    public IEnumerator CheckInternet(Action <bool> Action)
    {
        UnityWebRequest request = new UnityWebRequest("www.google.com");
        yield return request.SendWebRequest();

        if (request.error != null)
        {
            Action(false);
        }
        else
        {
            Action(true);
        }
       
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




