using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = .25f;
    private float currentSpeed;
    [SerializeField] private float turnSpeed = 2f;

    private Rigidbody rb = null;

    public float MoveSpeed
    {
        set
        {
            if (value < 0)
            {
                value = 0;
            }
            this.currentSpeed = value;
        }
    }

    public void ResetMoveSpeed()
    {
        this.currentSpeed = this.moveSpeed;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = moveSpeed;
    }

    private void FixedUpdate()
    {
        MoveTank();
        TurnTank();
    }

    public void MoveTank()
    {
        // calculate the move amount
        float moveAmountThisFrame = Input.GetAxis("Vertical") * currentSpeed;
        // create a vector from amount and direction
        Vector3 moveOffset = transform.forward * moveAmountThisFrame;
        // apply vector to the rigidbody
        rb.MovePosition(rb.position + moveOffset);
        // technically adjusting vector is more accurate! (but more complex)
    }

    public void TurnTank()
    {
        // calculate the turn amount
        float turnAmountThisFrame = Input.GetAxis("Horizontal") * turnSpeed;
        // create a Quaternion from amount and direction (x,y,z)
        Quaternion turnOffset = Quaternion.Euler(0, turnAmountThisFrame, 0);
        // apply quaternion to the rigidbody
        rb.MoveRotation(rb.rotation * turnOffset);
    }
}
