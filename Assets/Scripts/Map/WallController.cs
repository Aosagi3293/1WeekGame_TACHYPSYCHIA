using UnityEngine;

public class WallController : MonoBehaviour
{
    private float baseSpeed = 3f;

    private float playerPos;

    private void Start()
    {
        playerPos = PlayerController.Instance.ReturnPosition();
    }

    private void Update()
    {
        if(GameManager.Instance.state != GameState.Play) return;

        float speed = baseSpeed + Time.time * 0.1f;

        transform.Translate(Vector3.up * speed * Time.deltaTime);

        if(this.gameObject.transform.position.y > playerPos + 10f)
        {
            Destroy(gameObject);
        }
    }
}
