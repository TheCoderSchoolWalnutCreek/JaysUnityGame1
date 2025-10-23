using UnityEngine;

public class camController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] float smoothSpeed = 5f;
    [SerializeField] Vector3 offset;
    [SerializeField] bool enableLookAhead;
    [SerializeField] float lookAheadDistance = 2f;
    [SerializeField] float lookAheadSmooth = 2f;

    private Vector3 currentVelocity;
    private Vector3 lookAheadPosition;
    private Vector3 lastTargetPos;
    private float lookDirection;
    void Start()
    {
        lastTargetPos = player.position;
    }
    void LateUpdate()
    {
        Vector3 delta = player.position - lastTargetPos;
        lastTargetPos = player.position;

        if (enableLookAhead)
        {
            float moveInput = Input.GetAxisRaw("Horizontal");
            if (Mathf.Abs(moveInput) > 0)
            {
                lookDirection = Mathf.Sign(moveInput);
                Vector3 targetLookAhead = new Vector3(lookDirection * lookAheadDistance, 0, 0);
                lookAheadPosition = Vector3.Lerp(lookAheadPosition, targetLookAhead, lookAheadSmooth*Time.deltaTime);
            }
        }
        else
        {
            lookAheadPosition = Vector3.zero;
        }
        Vector3 desiredPos = player.position + offset + lookAheadPosition;
        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPos;
    }
}