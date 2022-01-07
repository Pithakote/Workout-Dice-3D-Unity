using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public enum TypeOfWorkout
{ 
    reps,
    minutes,
    seconds
}

[System.Serializable]
public struct WorkoutInformation
{
    [SerializeField]
    string nameOfWorkout;
    [SerializeField]
    float number;
    [SerializeField]
    TypeOfWorkout typeOfWorkout;
    [SerializeField]
    CinemachineVirtualCamera assignedCamera;
    public string NameOfWorkout { get { return nameOfWorkout; } }
    public float Number { get { return number; } }
    public TypeOfWorkout TypeOfWorkout { get { return typeOfWorkout; } }
    public CinemachineVirtualCamera AssignedCamera { get { return assignedCamera; } }
}
public class WorkoutDiceSide : MonoBehaviour
{
    [SerializeField]
    Texture materialTexture;
    
    Material objectMaterial;
    [SerializeField]
    WorkoutInformation workoutInformation;

    public WorkoutInformation WorkoutInformationGetter { get {return workoutInformation; } }
    private void Awake()
    {
        if (objectMaterial == null)
            objectMaterial = GetComponentInChildren<Renderer>().material;

        if (objectMaterial != null)
            objectMaterial.SetTexture("_BaseMap",materialTexture);
        objectMaterial.SetTexture("_EmissionMap", materialTexture);
    }

}
