using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject currentCamera;

    MaterialPropertyBlock matBlock;
    MeshRenderer meshRenderer;

    UnitActivity unitActivity;

    void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera");
        currentCamera = mainCamera;

        meshRenderer = GetComponent<MeshRenderer>();
        matBlock = new MaterialPropertyBlock();

        unitActivity = GetComponentInParent<UnitActivity>();
    }

    void Update()
    { 
        Transform camXForm = mainCamera.transform;
        Vector3 forward = transform.position - camXForm.position;
        forward.Normalize();
        Vector3 up = Vector3.Cross(forward, camXForm.right);
        transform.rotation = Quaternion.LookRotation(forward, up);

        meshRenderer.GetPropertyBlock(matBlock);
        matBlock.SetFloat("_Fill", unitActivity.hpCurrent / (float)unitActivity.hpMax);
        meshRenderer.SetPropertyBlock(matBlock);

    }
}
