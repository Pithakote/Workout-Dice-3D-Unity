using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
public class ThrowHandler : MonoBehaviour
{
    [SerializeField]
    TMP_Text workoutText;
    bool isThrown = false, isDiceMoving = false;
    [SerializeField]
    GameObject selectedDice;
    WorkoutDice selectedDiceWorkoutComponent;
    WorkoutDiceSide selectedDiceWorkoutComponentSide;
    CinemachineVirtualCamera selectedDiceSideCamera;
    [SerializeField]
    GameObject[] workoutDiceArray;
    [SerializeField]
    int selectedArrayNumber;
   

    [SerializeField]
    GameObject mainMenuCanavas, throwCanvas, resetCanvas;

    Rigidbody selectedDiceRigidbody;
    [SerializeField]
    float forceToApply = 100f, forceToRotate = 100f;
    [SerializeField]
    GameManager gameManager;
    public GameObject SelectedDice { get { return selectedDice; } }
    private void Start()
    {
        gameManager = GameManager.Instance;
        if (workoutDiceArray.Length <= 0)
            return;

        selectedArrayNumber = Random.Range(0, workoutDiceArray.Length);
        SetDiceFromArrayActive();
    }
    private void FixedUpdate()
    {
        Debug.Log("The velocity magnitude is: " + selectedDiceRigidbody.velocity.magnitude);
        Debug.Log("The angular velocity magnitude is: " + selectedDiceRigidbody.angularVelocity.magnitude);
        if (isThrown == false)
            return;

        if (selectedDiceRigidbody.velocity.magnitude > 0 && selectedDiceRigidbody.angularVelocity.magnitude > 0)
        {
            isDiceMoving = true;
        }
        if (isDiceMoving && selectedDiceRigidbody.velocity.magnitude <= 0 && selectedDiceRigidbody.angularVelocity.magnitude <= 0)
        {
            resetCanvas.SetActive(true);
            selectedDiceWorkoutComponent = selectedDiceRigidbody.GetComponent<WorkoutDice>();
            selectedDiceWorkoutComponentSide = selectedDiceWorkoutComponent.FindTheCorrectSide();
            selectedDiceSideCamera = selectedDiceWorkoutComponentSide.WorkoutInformationGetter.AssignedCamera;
            workoutText.text = "Do " + selectedDiceWorkoutComponentSide.WorkoutInformationGetter.NameOfWorkout +
                                " for " + selectedDiceWorkoutComponentSide.WorkoutInformationGetter.Number +
                                " " + selectedDiceWorkoutComponentSide.WorkoutInformationGetter.TypeOfWorkout;

            gameManager.cameraManagerGetter.SetEndCamera(selectedDiceSideCamera);
            gameManager.cameraManagerGetter.SwitchPripritiesVirtualCameras(gameManager.cameraManagerGetter.EndVirtualCamera, gameManager.cameraManagerGetter.DefaultVirtualCamera);

            isDiceMoving = false;
            isThrown = false;
        }
    }
    void SetDiceFromArrayActive()
    {
        selectedDice = workoutDiceArray[selectedArrayNumber].gameObject;

        foreach (GameObject workoutDiceObjects in workoutDiceArray)
        {
            if (selectedDice.name != workoutDiceObjects.name)
                workoutDiceObjects.SetActive(false);
        }

        workoutDiceArray[selectedArrayNumber].gameObject.SetActive(true);
    }

    public void NextDice()
    {
        selectedDice.SetActive(false);
        if ((selectedArrayNumber + 1) >= workoutDiceArray.Length)
        {
            selectedArrayNumber = 0;
        }
        else
        {
            selectedArrayNumber++;
        }

        SetDiceFromArrayActive();
    }

    public void PreviousDice()
    {
        selectedDice.SetActive(false);
        if ((selectedArrayNumber - 1) < 0)
        {
            selectedArrayNumber = workoutDiceArray.Length-1;
        }
        else
        {
            selectedArrayNumber--;
        }
        SetDiceFromArrayActive();
    }
    public void SelectDice()
    {
        if (throwCanvas == null || mainMenuCanavas == null)
            return;

      //  mainCamera.gameObject.transform.SetParent(selectedDice.transform);
        throwCanvas.gameObject.SetActive(true);
        mainMenuCanavas.gameObject.SetActive(false);

        gameManager.cameraManagerGetter.SetCameraLook(gameManager.cameraManagerGetter.DefaultVirtualCamera, selectedDice.transform);
    }

    public void ThrowDice()
    {
        isThrown = true;
        selectedDiceRigidbody = selectedDice.GetComponent<Rigidbody>();

       // selectedDiceRigidbody.AddRelativeForce(new Vector3 (0,1*forceToApply,1 * forceToApply), ForceMode.Impulse);
       // selectedDiceRigidbody.AddRelativeTorque((Vector3.up) * forceToRotate, ForceMode.Impulse);

        selectedDiceRigidbody.AddForce((Vector3.up + Vector3.forward) * forceToApply, ForceMode.Impulse);
        selectedDiceRigidbody.AddTorque(selectedDice.transform.up * forceToRotate, ForceMode.Impulse);
        throwCanvas.SetActive(false);
    }
}
