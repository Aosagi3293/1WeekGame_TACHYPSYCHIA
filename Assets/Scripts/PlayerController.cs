using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    public float swipeThreshold = 50f; // タッチスワイプの最低移動距離

    // タッチスワイプの判定用変数
    private Vector2 startPos;
    private Vector2 currentPos;
    private bool isDragging = false;

    private Vector2 moveInput;

    public float acceleration = 10f;
    public float damping = 0.92f;
    public float maxSpeed = 1f;

    private Vector3 velocity;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        HandleTouch();  // タッチスワイプ
        HandleKeyboard();   // WASD

        MovePlayer();   // 移動用関数
    }

    // タッチスワイプ
    private void HandleTouch()
    {
        if (Touchscreen.current == null) return;

        var touch = Touchscreen.current.primaryTouch;

        // 指が触れた瞬間基準点を決める
        if (touch.press.wasPressedThisFrame)
        {
            // 現在地を取得
            startPos = touch.position.ReadValue();
            isDragging = true;
        }

        // 指を動かしている最中にリアルタイムに方向を計算して動かす
        if (touch.press.isPressed && isDragging)
        {
            currentPos = touch.position.ReadValue();
            
            // 触れた場所から、今の指の位置までの距離と方向を計算
            Vector2 delta = currentPos - startPos;

            // 指を一定以上動かしたら、その方向に入力を入れる
            if (delta.magnitude > swipeThreshold)
            {
                moveInput = delta.normalized;
            }
            else
            {
                // 動かした量が少なければ止まる
                moveInput = Vector2.zero; 
            }
        }

        // 指が離れた瞬間移動をストップする
        if (touch.press.wasReleasedThisFrame && isDragging)
        {
            moveInput = Vector2.zero; // 指が離れたら入力をゼロにする
            isDragging = false;
        }
    }

    // WASD
    private void HandleKeyboard()
    {
        Vector2 keyInput = Vector2.zero;

        if (Keyboard.current.wKey.isPressed) keyInput.y += 1;
        if (Keyboard.current.sKey.isPressed) keyInput.y -= 1;
        if (Keyboard.current.aKey.isPressed) keyInput.x -= 1;
        if (Keyboard.current.dKey.isPressed) keyInput.x += 1;

        moveInput += keyInput;
    }

    // 移動用関数
    private void MovePlayer()
    {
        if(GameManager.Instance.state != GameState.Play) return;

        Vector3 inputDir = new Vector3(moveInput.x, 0, moveInput.y);

        // 加速
        velocity += inputDir * acceleration * Time.deltaTime;

        // 減速
        if (inputDir.magnitude < 0.1f)
        {
            velocity *= 0.8f; // 入力なし時は強く減速
        }
        else
        {
            velocity *= damping;
        }

        // 最大速度制限
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        // 移動
        transform.position += velocity * Time.deltaTime;
    }

    // 接触検知
    private void OnCollisionEnter(Collision other)
    {
        // 接触したオブジェクトのタグが壁（Wall）なら死亡
        if(other.gameObject.CompareTag("Wall"))
        {
            GameManager.Instance.state = GameState.Start;
            Debug.Log("Playerが死亡しました");

            var score = ScoreManager.Instance.score;
            Debug.Log($"今回のスコア： {score}");

            GameManager.Instance.ResetGame();
        }
    }

    public float ReturnPosition()
    {
        var posY = this.gameObject.transform.position.y;

        return posY;
    }
}