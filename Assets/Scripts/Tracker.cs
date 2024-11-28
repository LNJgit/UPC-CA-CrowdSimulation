using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    private Vector3 lastPosition;
    private Quaternion lastRotation;
    private Vector3 displacementWorld;
    private float speed;
    private Vector3 forwardVector;

    private float currentDirection;

    public OrientationManager orientationManager;

    public float getSpeed()
    {
        float dotProduct = Vector3.Dot(forwardVector.normalized, displacementWorld.normalized);

        if (Mathf.Abs(dotProduct) < 0.1)
        {
            return 0.0f;
        }
        if (dotProduct < 0)
        {
            return -speed; // Negative speed when moving backward
        }
        return speed; // Positive speed when moving forward
    }

    public float calculateDirection()
    {
        Vector3 currentForward = transform.forward;
        Vector3 lastForward = lastRotation * Vector3.forward;

        
        Vector3 crossProduct = Vector3.Cross(lastForward, currentForward);
        if (crossProduct.y > 0)
        {
            return 1f; 
        }
        else if (crossProduct.y < 0)
        {
            return -1f; 
        }

        return 0f; 
    }

    public float calculateDirectionLocked()
    {
        // Calculate direction based on displacement
        Vector3 displacement = transform.position - lastPosition;
        Vector3 right = transform.right;

        float direction = Vector3.Dot(displacement.normalized, right);
        return direction;
    }

    public float getDirection()
    {
        return currentDirection;
    } 

    void Start()
    {
        forwardVector = transform.forward;
        lastPosition = transform.position;
        lastRotation = transform.rotation;
        orientationManager = GetComponent<OrientationManager>();
        if (orientationManager == null)
        {
            Debug.LogError("OrientationManager component not found on the same GameObject.");
        }
    }

    void Update()
    {
        Vector3 currentPosition = transform.position;
        Quaternion currentRotation = transform.rotation;

        // Calculate displacement
        displacementWorld = currentPosition - lastPosition;
        speed = displacementWorld.magnitude / Time.deltaTime;

        forwardVector = transform.forward;

        
        currentDirection = calculateDirectionLocked();
        

        // Debug both speed and direction on the same line
        Debug.Log("TRACKER | Time: " + Time.time.ToString("F2") + " | Speed: " + speed.ToString("F2") + " | Direction: " + currentDirection);

        lastPosition = currentPosition;
        lastRotation = currentRotation;
    }


}
