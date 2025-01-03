using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    public float moveSpeed = 1f;

    public void Move(float horizontal, float vertical)
    {
        
        Vector3 movement = new Vector3(horizontal, 0, vertical).normalized * moveSpeed * Time.deltaTime;
        
        transform.Translate(movement, Space.Self);
    }
}
