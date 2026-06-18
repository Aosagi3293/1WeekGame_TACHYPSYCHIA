using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform target;

    private Vector3 offset = new Vector3(0, 5f, 0);

    private void LateUpdate()
    {
        transform.position = target.position + offset;

        transform.rotation = Quaternion.Euler(90, 0, 0);
    }
}
