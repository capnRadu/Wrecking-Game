using UnityEngine;

public class PieceControl : MonoBehaviour
{
    private Vector3 startPosition;
    private Quaternion startRotation;
    private bool isLifted = false;
    private bool isReturning = false;
    private float liftHeight = 1.2f;
    private float returnSpeed = 3f;
    private float liftSpeed = 0.4f;
    public static bool timereset;
    private bool obs;

    private Rigidbody rb;
    private MeshCollider meshCollider;  // Add reference to MeshCollider

    void Start()
    {
        timereset = false;
        obs = false;

        startPosition = transform.position;
        startRotation = transform.rotation;

        rb = GetComponent<Rigidbody>();
        meshCollider = GetComponent<MeshCollider>(); // Get MeshCollider component
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isLifted && !isReturning && PlayerController.partsCollected && !obs)
        {
            StartCoroutine(LiftAndReturn());
            obs = true;
            SoundtrackScript.GameEnd = true;

        }

        if (isReturning)
        {
            ReturnToStart();
        }
    }

    public void Rewind()
    {
        if (!isLifted && !isReturning)
        {
            StartCoroutine(LiftAndReturn());
        }
    }

    private System.Collections.IEnumerator LiftAndReturn()
    {
        isLifted = true;

        if (rb != null)
        {
            rb.isKinematic = true;
        }

        // Disable the MeshCollider when lifting
        if (meshCollider != null)
        {
            meshCollider.enabled = false;
        }

        Vector3 targetPosition = startPosition + Vector3.up * liftHeight;
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, liftSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(1);

        isReturning = true;
        isLifted = false;
    }

    private void ReturnToStart()
    {
        transform.position = Vector3.MoveTowards(transform.position, startPosition, returnSpeed * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, startRotation, returnSpeed * Time.deltaTime * 10);

        // Keep the MeshCollider disabled during return
        if (meshCollider != null)
        {
            meshCollider.enabled = false;
        }

        if (Vector3.Distance(transform.position, startPosition) < 0.1f && Quaternion.Angle(transform.rotation, startRotation) < 1.0f)
        {
            isReturning = false;

            if (rb != null)
            {
                rb.isKinematic = false;
            }

            StartCoroutine(DelayedTimereset());
        }
    }

    private System.Collections.IEnumerator DelayedTimereset()
    {
        yield return new WaitForSeconds(2f);
        timereset = true;
    }
}
