using UnityEngine;
using System.Collections;

public class RewindNotBrakable : MonoBehaviour
{
    private Vector3 startPosition;
    private Quaternion startRotation;
    private bool isLifted = false;
    private bool isReturning = false;

    private float liftHeight = 2f;
    private float returnSpeed = 0.5f;
    private float liftSpeed = 0.2f;
    private float returnOffset = 0.05f; // Small offset to stop slightly above

    private Rigidbody rb;
    private BoxCollider boxCollider;

    public static bool rewindTrigger = false;
    private bool alreadyLifted = false;

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;

        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        if (rewindTrigger && !isLifted && !isReturning && !alreadyLifted)
        {
            StartCoroutine(LiftAndReturn());
            alreadyLifted = true;
        }

        if (isReturning)
        {
            ReturnToStart();
        }
    }

    private IEnumerator LiftAndReturn()
    {
        isLifted = true;

        if (rb != null)
            rb.isKinematic = true;

        if (boxCollider != null)
            boxCollider.enabled = false;

        Vector3 targetPosition = startPosition + Vector3.up * liftHeight;

        while (Vector3.Distance(transform.position, targetPosition) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, liftSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(3);

        isReturning = true;
        isLifted = false;
    }

    private void ReturnToStart()
    {
        // Set return target slightly above original position
        Vector3 returnTarget = startPosition + Vector3.up * returnOffset;

        transform.position = Vector3.MoveTowards(transform.position, returnTarget, returnSpeed * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, startRotation, returnSpeed * Time.deltaTime * 10);

        if (Vector3.Distance(transform.position, returnTarget) < 0.05f && Quaternion.Angle(transform.rotation, startRotation) < 1.0f)
        {
            isReturning = false;

            if (rb != null)
                rb.isKinematic = false;

            if (boxCollider != null)
                boxCollider.enabled = true;

            StartCoroutine(DelayedRewindReset());
        }
    }

    private IEnumerator DelayedRewindReset()
    {
        yield return new WaitForSeconds(4f);
        rewindTrigger = false;
        alreadyLifted = false;
    }
}
