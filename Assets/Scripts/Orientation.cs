using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientationManager : MonoBehaviour
{
    public bool lockRotation = false;
    private Quaternion fixedRotation;

    void Start()
    {
        fixedRotation = transform.rotation; 
    }

    public void Rotate(Vector3 direction, float rotateSpeed)
    {
        if (lockRotation)
        {
            transform.rotation = fixedRotation;
            Debug.Log("Rotation is locked.");
        }
        else if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
            //Debug.Log($"Rotating to face direction: {direction}");
        }
        else
        {
            Debug.LogWarning("Direction vector is zero or too small, skipping rotation.");
        }
    }



    public void LockCameraRotation(bool shouldLock)
    {
        lockRotation = shouldLock;
        if (lockRotation)
        {
            fixedRotation = transform.rotation; 
        }
    }
}
