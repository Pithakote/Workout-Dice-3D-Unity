using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    CameraManager cameraManager;
    [SerializeField]
    JsonReadWriteSystem saveSystem;
    private static GameManager  instance, duplicateInstance;

    [SerializeField]
    ThrowHandler throwHandler;

    public CameraManager cameraManagerGetter { get { return cameraManager; } }
    public JsonReadWriteSystem SaveSystem { get { return saveSystem; } }
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
