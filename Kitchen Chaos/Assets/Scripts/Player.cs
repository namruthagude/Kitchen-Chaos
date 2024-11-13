using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{ 
    [SerializeField]
    private float moveSpeed = 7f;
    [SerializeField]
    private GameInput gameInput;
    [SerializeField]
    private LayerMask counterLayerMask;
    private bool isWalking;
    private Vector3 lastInteractDir;

    // Calls this method for every frame
    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 movDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = 0.7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, movDir, moveDistance);
        if (!canMove)
        {
            //Cannot move toward movedir

            //Attempt to move in X direction
            Vector3 movDirX = new Vector3(inputVector.x, 0f, 0f).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, movDirX, moveDistance);
            if (canMove)
            {
                movDir = movDirX;
            }
            else
            {
                //Attempt to move in Z direction
                Vector3 movDirZ = new Vector3(0f, 0f, inputVector.y).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, movDirZ, moveDistance);
                if (canMove)
                {
                    movDir = movDirZ;
                }
                else
                {
                    // cannot move in any direction
                }
            }

        }
        if (canMove)
        {
            transform.position += movDir * moveDistance;
        }

        isWalking = movDir != Vector3.zero;
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, movDir, Time.deltaTime * rotateSpeed);

    }

    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 movDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if(movDir != Vector3.zero)
        {
            lastInteractDir = movDir;
        }

        float interactDistance = 2f;

        if(Physics.Raycast(transform.position,lastInteractDir, out RaycastHit raycastHit, interactDistance, counterLayerMask))
        {
            if(raycastHit.transform.TryGetComponent<ClearCounter>(out ClearCounter clearCounter))
            {
                clearCounter.Interact();
            }
        }
        
    }
}
