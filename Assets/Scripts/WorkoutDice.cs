using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkoutDice : MonoBehaviour
{
    bool isThrown = false;

    public bool IsThrown {get { return isThrown; }}

    GameManager gameManager;
    
    private void Start()
    {
        gameManager = GameManager.Instance;
    }

   
}
