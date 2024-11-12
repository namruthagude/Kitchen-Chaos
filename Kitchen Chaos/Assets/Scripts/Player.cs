using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 7f;
    // Calls this method for every frame
    private void Update()
    {
        Vector2 inputVector = new Vector2(0f, 0f);
        if (Input.GetKey(KeyCode.W))
        {
            inputVector.y = -1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputVector.y = 1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputVector.x = 1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVector.x = -1f;
        }

        inputVector = inputVector.normalized;

        Vector3 movDir = new Vector3(inputVector.x, 0f, inputVector.y);
        transform.position += movDir * moveSpeed * Time.deltaTime;
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, movDir, Time.deltaTime * rotateSpeed); 
        
    }
}
