using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    private Rigidbody rb;

    public float swipeThreshold = 50f; // タッチスワイプの最低移動距離

    // タッチスワイプの判定用変数
    private Vector2 startPos;
    private Vector2 currentPos;
    private bool isDragging = false;

    private Vector2 moveInput;

    public float acceleration = 10f;
    public float damping = 0.1f;
    public float maxSpeed = 1f;

    private Vector3 velocity;

    private Vector3 currentVelocity;

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

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        MovePlayer();   // 移動用関数
    }

    private void Update()
    {
        if (GameManager.Instance == null) return;
        if (GameManager.Instance.state != GameState.Play) return;

        HandleTouch();  // タッチスワイプ
        HandleKeyboard();   // WASD
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
        if (GameManager.Instance == null) return;
        if (GameManager.Instance.state != GameState.Play) return;

        Vector3 inputDir = new Vector3(moveInput.x, 0, moveInput.y);

        if (inputDir.sqrMagnitude > 0.01f)
        {
            // 入力方向が変わったら完全停止
            if (currentVelocity.sqrMagnitude > 0.001f)
            {
                Vector3 currentDir = currentVelocity.normalized;
                Vector3 inputNorm = inputDir.normalized;

                if (Vector3.Dot(currentDir, inputNorm) < 0.5f)
                {
                    currentVelocity = Vector3.zero;
                }
            }

            // 加速
            currentVelocity += inputDir * acceleration * Time.fixedDeltaTime;

            // 最大速度制限
            currentVelocity = Vector3.ClampMagnitude(currentVelocity, maxSpeed);
        }
        else
        {
            // 入力なし → 完全停止
            currentVelocity = Vector3.zero;
        }

        rb.MovePosition(rb.position + currentVelocity * Time.fixedDeltaTime);
    }

    // 接触検知
    private void OnCollisionEnter(Collision other)
    {
        if(GameManager.Instance.state != GameState.Play) return;

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

    public void ResetPlayer()
    {
        // 速度値（変数）のリセット
        currentVelocity = Vector3.zero;
        moveInput = Vector2.zero; 

        // Rigidbody の物理的な勢いを完全にゼロにする
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            
            rb.angularVelocity = Vector3.zero; // 回転の勢いも止める
        }
        
        // 座標のリセット
        if (rb != null)
        {
            rb.position = Vector3.zero;
        }
        else
        {
            transform.position = Vector3.zero;
        }
    }

}