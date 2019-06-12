using UnityEngine;

public class CameraFollower: MonoBehaviour {
    public Transform target;

    public float smoothSpeed = 0.125f;
    public Vector3 offsetPosition;

    void LateUpdate () {
        Vector3 desiredPosition = target.position + offsetPosition;
        Vector3 smoothedPosition = Vector3.Lerp (transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
