using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public OrientationManager orientationManager;
    public MovementManager movementManager;
    public float rotateSpeed = 100f;

    void Update()
    {
        if (orientationManager == null || movementManager == null) return;

        HandleInput();
    }

    private void HandleInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal != 0)
        {
            if (orientationManager.lockRotation)
            {
                movementManager.Move(horizontal,vertical);
            }
            else
            {
                //orientationManager.Rotate(horizontal, rotateSpeed);
            }
        }

        if (vertical != 0)
        {
            movementManager.Move(horizontal, vertical);
        }
    }
}
