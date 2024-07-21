using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RopeScript : MonoBehaviour
{
    [Header("Rope Settings")]
    [SerializeField] private Transform transPoint1; // The first point of the slingshot (the handle)
    [SerializeField] private Transform transPoint2; // The second point of the slingshot (the rubber band)

    [Header("Prefab")]
    [SerializeField] private Transform ballPrefab; // The ball prefab to be instantiated

    [Header("Settings")]
    public float dragBackOffset = 1.0f; // The offset distance to move the ball back from the slingshot

    private LineRenderer lineRenderer; // The line renderer for the rubber band
    private Transform currentBall; // The instance of the ball
    private Camera mainCam; // The main camera

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 3; // The line renderer will have three points
        mainCam = Camera.main; // Get the main camera
        lineRenderer.enabled = false; // Start with the line renderer disabled
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartDragging();
        }

        if (Input.GetMouseButton(0))
        {
            if (currentBall != null)
            {
                Dragging();
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (currentBall != null)
            {
                Release();
            }
        }

        UpdateRope();
    }

    void StartDragging()
    {
        if (ballPrefab != null)
        {
            currentBall = Instantiate(ballPrefab, transPoint1.position, Quaternion.identity);
            Rigidbody ballRb = currentBall.GetComponent<Rigidbody>();
            ballRb.isKinematic = true; // Set the ball to kinematic so it doesn't respond to physics yet
            lineRenderer.enabled = true; // Show the LineRenderer when dragging starts
        }
    }

    void Dragging()
    {
        if (currentBall != null)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = mainCam.WorldToScreenPoint(currentBall.position).z;
            Vector3 worldPos = mainCam.ScreenToWorldPoint(mousePos);

            Vector3 dragDirection = (worldPos - transPoint1.position).normalized;
            currentBall.position = transPoint1.position + dragDirection * dragBackOffset;

            lineRenderer.SetPosition(1, currentBall.position); // Set the second point of the line renderer to the ball's position
        }
    }

    void Release()
    {
        if (currentBall != null)
        {
            Rigidbody ballRb = currentBall.GetComponent<Rigidbody>();
            ballRb.isKinematic = false; // Enable physics

            Vector3 forceDirection = (transPoint1.position - currentBall.position).normalized;
            forceDirection = Quaternion.Euler(0, 0, 340) * forceDirection; // Rotate force direction

            ballRb.AddForce(forceDirection * 1000); // Apply force to the ball
            currentBall = null; // Clear the ball reference
            lineRenderer.enabled = false; // Hide the LineRenderer when dragging stops
        }
    }

    void UpdateRope()
    {
        if (transPoint1 != null && transPoint2 != null)
        {
            lineRenderer.SetPosition(0, transPoint1.position); // Set the first point of the line renderer to transPoint1
            lineRenderer.SetPosition(2, transPoint2.position); // Set the third point of the line renderer to transPoint2
            if (currentBall != null)
            {
                lineRenderer.SetPosition(1, currentBall.position); // Update the second point of the line renderer to the ball's position
            }
        }
    }
}
