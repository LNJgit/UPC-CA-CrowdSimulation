using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locomotion : MonoBehaviour
{
    private Tracker Tracker;
    public Animator animator;

    // Smoothing variables
    public float speedSmoothTime = 0.2f;
    public float directionSmoothTime = 0.2f;
    private float speedSmoothVelocity;
    private float directionSmoothVelocity;

    // Current values for smoothing
    private float currentSpeed;
    private float currentDirection;

    void Start()
    {
        Tracker = FindObjectOfType<Tracker>();
    }

    void Update()
    {
        if (Tracker != null)
        {
            float targetSpeed = Tracker.getSpeed();
            float targetDirection = Tracker.getDirection();

            currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);
            currentDirection = Mathf.SmoothDamp(currentDirection, targetDirection, ref directionSmoothVelocity, directionSmoothTime);

            animator.SetFloat("Speed", currentSpeed);
            animator.SetFloat("Direction", currentDirection);
        }
        else
        {
            Debug.LogError("Tracker reference is not assigned!");
        }
    }

}
