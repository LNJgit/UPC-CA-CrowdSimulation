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

    public void Rotate(float horizontal, float rotateSpeed)
    {
        if (lockRotation)
        {
            transform.rotation = fixedRotation;
        }
        else
        {
            float rotationAmount = horizontal * rotateSpeed * Time.deltaTime;
            transform.Rotate(0, rotationAmount, 0);
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
