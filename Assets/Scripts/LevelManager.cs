using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LevelManager : MonoBehaviour
{
    [Header("Options")]
    [SerializeField]
    Toggle _audioToggle, _exercisingGuy;
    [Header("Error Texts")]
    [SerializeField]
    TMP_Text savingErrorText, loadingErrorText, signingInErrorText;
    [Header("Error UIs")]
    [SerializeField]
    GameObject signingIn, signingInUnsuccessful,
                savingScreen, savingScreenUnsucessful,
                loadingDataScreen, loadingDataUnsuccessful,
                googlePlayServiceError, authenticationFailedUI, userNotFoundUI,
                mainMenuUI, noInternetUI;
    [Header("No Internet Button")]
    [SerializeField]
    Button tryAgainButton;

    public TMP_Text SavingErrorText {get {return savingErrorText;}}
    public TMP_Text LoadingErrorText {get {return loadingErrorText;}}
    public TMP_Text SigningInErrorText { get {return signingInErrorText; }}
    public Toggle AudioToggle {get {return _audioToggle;}}
    public Toggle ExercisingGuy { get {return _exercisingGuy; }}
    public GameObject SigningIn { get { return signingIn; } }
    public GameObject SigningInUnsuccessful { get { return signingInUnsuccessful; } }
    public GameObject SavingScreen { get { return savingScreen; } }
    public GameObject SavingScreenUnsucessful { get { return savingScreenUnsucessful; } }
    public GameObject LoadingDataScreen { get { return loadingDataScreen; } }
    public GameObject LoadingDataUnsuccessful { get { return loadingDataUnsuccessful; } }
    public GameObject GooglePlayServiceError { get { return googlePlayServiceError; } }
    public GameObject AuthenticationFailedUI { get { return authenticationFailedUI; } }
    public GameObject UserNotFoundUI { get { return userNotFoundUI; } }
    public GameObject MainMenuUI { get { return mainMenuUI; } }
    public GameObject NoInternetUI { get { return noInternetUI; } }
    public Button TryAgainButton { get { return tryAgainButton; } }



    private void Awake()
    {
        ToggleUIToggles(false);
        ToggleInformationScreens(mainMenuUI, false);
    }
    public void ToggleUIToggles(bool toogleValue)
    {
        _audioToggle.isOn = toogleValue;
        _exercisingGuy.isOn = toogleValue;
    }

    public void ToggleInformationScreens(GameObject gameObject, bool show)
    {
        //backgroundUI.SetActive(show);
        gameObject.SetActive(show);
    }

    public void SetText(TMP_Text textToChange, string text)
    {
        textToChange.text = text;
    }

}
