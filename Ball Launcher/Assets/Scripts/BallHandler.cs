using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Rigidbody2D pivot;
	[SerializeField] private float detachDelay = 0.2f;
	[SerializeField] private float respawnDelay = 1f;

	private Rigidbody2D currentBallRigitbody;
    private SpringJoint2D currentBallSpringJoint;
    private Camera mainCamera;
    private bool isDragging = false;

    // Start is called before the first frame update
    private void Start()
    {
        mainCamera = Camera.main;
        SpawnNewBall();
    }

    // Update is called once per frame
    private void Update()
    {
        if (currentBallRigitbody == null || currentBallSpringJoint == null)
            return;

        if (!Touchscreen.current.primaryTouch.press.isPressed)
        {
            if (isDragging)
            {
                LaunchBall();
            }
            isDragging = false;
            return;
        }

        isDragging = true;
        currentBallRigitbody.isKinematic = true;
        Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);
        currentBallRigitbody.position = worldPosition;
    }

    private void SpawnNewBall()
    {
        GameObject ballInstance = Instantiate(ballPrefab, pivot.position, Quaternion.identity);

        currentBallRigitbody = ballInstance.GetComponent<Rigidbody2D>();
        currentBallSpringJoint = ballInstance.GetComponent<SpringJoint2D>();
        currentBallSpringJoint.connectedBody = pivot;

	}

    private void LaunchBall()
    {

		currentBallRigitbody.isKinematic = false;
        currentBallRigitbody = null;

        Invoke(nameof(DetachBall), detachDelay);
        

	}

    private void DetachBall()
    {
        currentBallSpringJoint.enabled = false;
        currentBallSpringJoint = null;

        Invoke(nameof(SpawnNewBall), respawnDelay);
	}

}
