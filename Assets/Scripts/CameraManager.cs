using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform target;

    private Vector3 offset = new Vector3(0, 3f, -3f);

    private void LateUpdate()
    {
        transform.position = target.position + offset;

        transform.rotation = Quaternion.Euler(50, 0, 0);
    }
}
