using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    private AudioSource footstepSound;

    private bool isActive = true;
    public bool IsActive
    {
        get { return isActive; }
        set { isActive = value; }
    }


    // Artefact Pieces

    [SerializeField] private GameObject part1;
    [SerializeField] private GameObject part2;
    [SerializeField] private GameObject part3;

    public static bool partsCollected;
    [SerializeField] private GameObject finalArtefact;
    [SerializeField] private GameObject finalArtefactPos;


    // Movement
    private float moveSpeed = 6f;
    private float gravity = -9.8f;
    private float jumpHeight = 1.2f;
    private bool isGrounded;
    private bool isMoving;
    private bool isCrouching;
    private Vector3 velocity;

    // Look
    public Camera cam;
    private float sensitivity = 2.5f;
    private float xRotation = 0f;

    // Attack
    private float attackDistance = 1.2f;
    public LayerMask attackLayer;
    private bool attacking = false;
    private bool readyToAttack = true;

    // Hammer
    public GameObject hammer;
    private Animator hammerAnim;
    private AnimationClip currentHammerAnimation;
    private AudioSource hammerSound;

    private bool isCoroutineRunning = false;

    [SerializeField] private ManagerTimeReset managerTimeReset;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        hammerAnim = hammer.GetComponent<Animator>();
        hammerSound = hammer.GetComponent<AudioSource>();
        footstepSound = GetComponent<AudioSource>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        partsCollected = false;
    }

    private void Update()
    {
        if (!part1.activeSelf && !part2.activeSelf && !part3.activeSelf && !partsCollected)
        {
            partsCollected = true;
            GameObject artefact = Instantiate(finalArtefact, finalArtefactPos.transform.position, Quaternion.identity);
            artefact.transform.SetParent(finalArtefactPos.transform, true);
            hammer.SetActive(false);
        }

        isGrounded = characterController.isGrounded;

        if (isActive)
        {
            Move();
            Look();
            Crouch();
            Footsteps();

            if (Input.GetMouseButton(0))
            {
                if (!partsCollected)
                {
                    Attack();
                }
            }
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

        if (x != 0 || z != 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

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

    private void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            cam.transform.localPosition = new Vector3(0, 0.25f, 0);
            characterController.height = 1;
            moveSpeed = 2f;
            isCrouching = true;

        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            cam.transform.localPosition = new Vector3(0, 0.53f, 0);
            characterController.height = 2;
            moveSpeed = 6f;
            isCrouching = false;
        }
    }

    private void Footsteps()
    {
        if (isGrounded && isMoving)
        {
            if (!footstepSound.isPlaying)
            {
                if (!isCrouching)
                {
                    StartCoroutine(FootstepSound(0f));
                }
                else if (!isCoroutineRunning)
                {
                    StartCoroutine(FootstepSound(0.3f));
                }
            }
        }
    }

    private void Attack()
    {
        if (!readyToAttack || attacking)
        {
            return;
        }

        readyToAttack = false;
        attacking = true;

        int randomAttack = Random.Range(0, 2);
        string attackName = randomAttack == 0 ? "Attack" : "Attack2";
        currentHammerAnimation = randomAttack == 0 ? hammerAnim.runtimeAnimatorController.animationClips[0] : hammerAnim.runtimeAnimatorController.animationClips[1];

        hammerAnim.SetTrigger(attackName);
        hammerSound.Play();

        Invoke(nameof(ResetAttack), currentHammerAnimation.length + 0.3f);
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

            if (hit.transform.TryGetComponent(out Breaking breaking))
            {
                breaking.Break();
            }
        }
    }

    private IEnumerator FootstepSound(float time)
    {
        isCoroutineRunning = true;
        yield return new WaitForSeconds(time);
        footstepSound.pitch = Random.Range(0.8f, 1.1f);
        footstepSound.Play();
        isCoroutineRunning = false;
    }
}