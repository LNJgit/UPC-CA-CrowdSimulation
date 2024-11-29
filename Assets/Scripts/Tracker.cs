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

        // Calculate the signed angle in degrees between the two vectors
        float angle = Vector3.SignedAngle(lastForward, currentForward, Vector3.up);

        float scaledDirection = angle*10;

        return Mathf.Clamp(scaledDirection, -1f, 1f); // Clamp to avoid extreme values
    }



    public float calculateDirectionLocked()
    {
        // Calculate direction based on displacement
        Vector3 displacement = transform.position - lastPosition;
        Vector3 right = transform.right;

        float direction = Vector3.Dot(displacement.normalized, right);

        if (Mathf.Abs(direction) < 0.1f)
        {
            return 0f; // Forward or backward or no movement
        }
        else if (direction > 0)
        {
            return 1f; // Right
        }
        else
        {
            return -1f; // Left
        }
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

        //Debug.Log($" Time: {Time.time}, Direction: {getDirection()}, Speed: {speed}");
        if (!orientationManager.lockRotation)
        {
            currentDirection = calculateDirection();
        }
        else
        {
            currentDirection = calculateDirectionLocked();
        }
        

        lastPosition = currentPosition;
        lastRotation = currentRotation;
    }
}
