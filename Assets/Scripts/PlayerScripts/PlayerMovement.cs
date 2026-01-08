using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] GameObject lumberjackModel;
    [SerializeField] Rigidbody rb;

    float movementSpeed = 5;

    Vector3 moveDirection;

    void Start()
    {
        moveDirection = Vector3.zero;
    }

    public void OnMove(InputValue inputValue)
    {
        Vector2 inputVector = inputValue.Get<Vector2>();

        if (inputVector.sqrMagnitude > 1f)
            inputVector.Normalize();

        moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);
    }

    void Movement()
    {
        rb.linearVelocity = moveDirection * movementSpeed;
    }

    void Rotation()
    {
        if (rb.linearVelocity.magnitude > 1)
        {
            lumberjackModel.transform.rotation = Quaternion.LookRotation(rb.linearVelocity);
        }
    }

    void Update()
    {
        Movement();
        Rotation();
    }
}
