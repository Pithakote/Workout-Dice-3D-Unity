using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    CameraManager cameraManager;

    private static GameManager  instance, duplicateInstance;

    [SerializeField]
    ThrowHandler throwHandler;

    public CameraManager cameraManagerGetter { get { return cameraManager; } }
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
       
    }



}
