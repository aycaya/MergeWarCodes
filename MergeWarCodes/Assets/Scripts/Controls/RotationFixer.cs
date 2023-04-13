using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationFixer : MonoBehaviour
{
    [SerializeField] Vector3 desiredRotation;


    void Update()
    {
        transform.eulerAngles = desiredRotation;
    }
    private void LateUpdate()
    {
        transform.eulerAngles = desiredRotation;
    }
}
