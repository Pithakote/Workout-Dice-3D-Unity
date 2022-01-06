using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkoutDice : MonoBehaviour
{
    [SerializeField]
    WorkoutDiceSide[] workoutDiceSideArray;
    [SerializeField]
    WorkoutDiceSide selectedWorkoutDiceSide;

    public WorkoutDiceSide SelectedWorkoutDiceSide { get { return selectedWorkoutDiceSide; } }
    public WorkoutDiceSide FindTheCorrectSide()
    {
        foreach (WorkoutDiceSide diceSides in workoutDiceSideArray)
        {
            if (Vector3.Dot(diceSides.transform.up.normalized, Vector3.up) > 0.9f)
            {
                selectedWorkoutDiceSide = diceSides;
                break;
            }
        }

        return selectedWorkoutDiceSide;
    }
   
}
