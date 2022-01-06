using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkoutDiceSide : MonoBehaviour
{
    [SerializeField]
    Texture materialTexture;
    
    Material objectMaterial;
    private void Awake()
    {
        if (objectMaterial == null)
            objectMaterial = GetComponentInChildren<Renderer>().material;

        if (objectMaterial != null)
            objectMaterial.SetTexture("_BaseMap",materialTexture);
    }

}
