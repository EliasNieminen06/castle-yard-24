using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private void LateUpdate()
    {
        Vector3 faceDirection = (transform.position - Camera.main.transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(faceDirection);
    }
}
