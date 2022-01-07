using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
public class ThrowHandler : MonoBehaviour
{
    [SerializeField]
    TMP_Text workoutText, selectedDiceText;
    bool isThrown = false, isDiceMoving = false;
    [SerializeField]
    GameObject selectedDice, diceHolder;
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
    float[] numberOfRotatingNumbers;
    [SerializeField]
    float forceToApply = 4f;
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

        selectedDiceWorkoutComponent = selectedDiceRigidbody.GetComponent<WorkoutDice>();
        selectedDiceText.text = selectedDiceWorkoutComponent.NameOfDice;
    }
    private void FixedUpdate()
    {
       // Debug.Log("The velocity magnitude is: " + selectedDiceRigidbody.velocity.magnitude);
       // Debug.Log("The angular velocity magnitude is: " + selectedDiceRigidbody.angularVelocity.magnitude);
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
        selectedDiceRigidbody = selectedDice.GetComponent<Rigidbody>();
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
        selectedDiceWorkoutComponent = selectedDiceRigidbody.GetComponent<WorkoutDice>();
        selectedDiceText.text = selectedDiceWorkoutComponent.NameOfDice;
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
        selectedDiceWorkoutComponent = selectedDiceRigidbody.GetComponent<WorkoutDice>();
        selectedDiceText.text = selectedDiceWorkoutComponent.NameOfDice;
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
        

       // selectedDiceRigidbody.AddRelativeForce(new Vector3 (0,1*forceToApply,1 * forceToApply), ForceMode.Impulse);
       // selectedDiceRigidbody.AddRelativeTorque((Vector3.up) * forceToRotate, ForceMode.Impulse);

        selectedDiceRigidbody.AddForce((Vector3.up * Random.Range(1, forceToApply + 1)  + Vector3.forward * Random.Range(1, forceToApply + 1)), ForceMode.Impulse);
        selectedDiceRigidbody.AddTorque((selectedDice.transform.up + selectedDice.transform.forward + selectedDice.transform.right) * numberOfRotatingNumbers[Random.Range(0, numberOfRotatingNumbers.Length)], ForceMode.Impulse);
        throwCanvas.SetActive(false);
    }

    public void ResetDice()
    {
        selectedDice.transform.position = diceHolder.transform.position;     
        
        gameManager.cameraManagerGetter.SwitchPripritiesVirtualCameras(gameManager.cameraManagerGetter.DefaultVirtualCamera, gameManager.cameraManagerGetter.EndVirtualCamera);
        gameManager.cameraManagerGetter.SetCameraLook(gameManager.cameraManagerGetter.DefaultVirtualCamera, diceHolder.transform);

        mainMenuCanavas.gameObject.SetActive(true);
        resetCanvas.SetActive(false);
    }
}
