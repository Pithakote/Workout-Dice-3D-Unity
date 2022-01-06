using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowHandler : MonoBehaviour
{
    bool isThrown = false, isDiceMoving = false;
    [SerializeField]
    GameObject selectedDice;
    [SerializeField]
    GameObject[] workoutDiceArray;
    [SerializeField]
    int selectedArrayNumber;
   

    [SerializeField]
    GameObject mainMenuCanavas, throwCanvas;

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
            gameManager.cameraManagerGetter.SetEndCamera(selectedDice);
            gameManager.cameraManagerGetter.SwitchPripritiesVirtualCameras(gameManager.cameraManagerGetter.EndVirtualCamera, gameManager.cameraManagerGetter.DefaultVirtualCamera);
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

        selectedDiceRigidbody.AddRelativeForce((Vector3.forward + Vector3.up) * forceToApply, ForceMode.Impulse);
        selectedDiceRigidbody.AddRelativeTorque((Vector3.up) * forceToRotate, ForceMode.Impulse);
    }
}
