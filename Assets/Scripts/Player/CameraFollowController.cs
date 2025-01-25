using UnityEngine;

public class CameraFollowController : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    void FixedUpdate()
    {
        // Smoothly move the center of the camera to the target's position

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Look at the target
        transform.LookAt(target);
    }
}
