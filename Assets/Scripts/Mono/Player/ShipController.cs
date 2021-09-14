using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [SerializeField] private float boundaryXMin = -18.7f;
    [SerializeField] private float boundaryXMax = 18.7f;
    [SerializeField] private float boundaryZMin = -24.35f;
    [SerializeField] private float boundaryZMax = 24.35f;
    [SerializeField] private float moveSpeed = 2f;
    private float currentSpeed;

    Rigidbody rb = null;

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
        this.rb = this.GetComponent<Rigidbody>();
        currentSpeed = moveSpeed;
    }

    private void FixedUpdate()
    {
        MoveShip();
    }

    public void MoveShip()
    {
        float moveForwardAmount = Input.GetAxis("Vertical") * currentSpeed;
        Vector3 forwardsBack = new Vector3(1, 0, 0) * moveForwardAmount;

        float moveLeftAmount = Input.GetAxis("Horizontal") * currentSpeed;
        Vector3 leftRight = new Vector3(0, 0, 1) * moveLeftAmount;

        this.rb.velocity = new Vector3(moveForwardAmount * currentSpeed * -1, 0, moveLeftAmount * currentSpeed);

        this.rb.position = new Vector3(
            Mathf.Clamp(this.rb.position.x, boundaryXMin, boundaryXMax),
            0.0f,
            Mathf.Clamp(this.rb.position.z, boundaryZMin, boundaryZMax));
    }
}
