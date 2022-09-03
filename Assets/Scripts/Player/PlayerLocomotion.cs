using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    [SerializeField]
    private Transform cameraHolder;
    [SerializeField]
    private float gravityForce = 10f;
    [SerializeField]
    private float movementSpeed = 5f;
    [SerializeField]
    private float skinWidth = 0.2f;
    [SerializeField]
    private LayerMask groundCheckLayerMask = -1;
    [SerializeField]
    private float groundCheckDistance = 0.05f;

    private Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.mass = 70f;
        rigidbody.isKinematic = false;
        rigidbody.useGravity = false;
        rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
    }

    public void HandleMovement(Vector2 input)
    {
        if (IsGrounded())
        {
            print("Grounded");
        }
        else
        {
            print("Not grounded");
            rigidbody.AddForce(-this.transform.up * gravityForce);
        }
    }

    private bool IsGrounded()
    {
        Vector3 groundCheckOrigin = this.transform.position + this.transform.up * skinWidth;

        if (Physics.SphereCast(groundCheckOrigin, 0.3f, -transform.up, out RaycastHit hit, groundCheckDistance + skinWidth, groundCheckLayerMask))
        {
            return true;
        }

        return false;
    }
}
