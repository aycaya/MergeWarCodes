using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StableScale : MonoBehaviour
{
    public Vector3 Offset;
    [SerializeField] Vector3 desiredRotation;



    private void Update()
    {
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x / transform.lossyScale.x, gameObject.transform.localScale.y / transform.lossyScale.y, gameObject.transform.localScale.z / transform.lossyScale.z);
        gameObject.transform.position = gameObject.transform.parent.position + Offset;
        transform.eulerAngles = desiredRotation;
    }
    private void LateUpdate()
    {
        transform.eulerAngles = desiredRotation;

    }
}
