using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Rigidbody2D rig;

    private Vector2 movementDirection;

    private void FixedUpdate()
    {
        rig.linearVelocity = movementDirection.normalized * speed;
    }

    void OnMove(InputValue value)
    {
        movementDirection = value.Get<Vector2>();
    }
}
