using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    public float moveSpeed = 1f;

    public void Move(float horizontal, float vertical)
    {
        // Create a movement vector in local space
        Vector3 movement = new Vector3(horizontal, 0, vertical).normalized * moveSpeed * Time.deltaTime;
        // Apply the movement in local space
        transform.Translate(movement, Space.Self);
    }
}
