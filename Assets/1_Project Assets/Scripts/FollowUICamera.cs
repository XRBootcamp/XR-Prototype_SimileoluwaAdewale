using UnityEngine;

public class FollowUICamera : MonoBehaviour
{
    public Transform cameraTransform; // Reference to main camera
    public Vector3 offset = new Vector3(0, -0.2f, 1.5f); // Position in front and slightly below eye level
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (cameraTransform == null)
            return;

        // Target position relative to camera
        Vector3 targetPosition = cameraTransform.position + cameraTransform.forward * offset.z
                                                       + cameraTransform.up * offset.y
                                                       + cameraTransform.right * offset.x;

        // Smoothly interpolate to that position
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

        // Optional: Look at camera smoothly
        Quaternion targetRotation = Quaternion.LookRotation(transform.position - cameraTransform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothSpeed * Time.deltaTime);
    }
}
