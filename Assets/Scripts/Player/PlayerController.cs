using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;

    // Movement
    private float moveSpeed = 6f;
    private float gravity = -9.8f;
    private float jumpHeight = 1.2f;
    private bool isGrounded;
    private Vector3 velocity;

    // Look
    public Camera cam;
    private float sensitivity = 2.5f;
    private float xRotation = 0f;

    // Attack
    private float attackDistance = 2f;
    public LayerMask attackLayer;
    private bool attacking = false;
    private bool readyToAttack = true;

    // Hammer
    public GameObject hammer;
    private Animator hammerAnim;
    public AnimationClip hammerSmash;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        hammerAnim = hammer.GetComponent<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        isGrounded = characterController.isGrounded;

        Move();
        Look();

        if (Input.GetMouseButton(0))
        {
            Attack();
        }
    }

    private void Move()
    {
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        characterController.Move(move * moveSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    private void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void Attack()
    {
        if (!readyToAttack || attacking)
        {
            return;
        }

        readyToAttack = false;
        attacking = true;

        hammerAnim.SetTrigger("Attack");

        Invoke(nameof(ResetAttack), hammerSmash.length + 0.2f);
        Invoke(nameof(AttackRaycast), 0);
    }

    private void ResetAttack()
    {
        attacking = false;
        readyToAttack = true;
    }

    private void AttackRaycast()
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, attackDistance, attackLayer))
        {
            Debug.Log(hit.transform.name);
        }
    }
}