using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float swipeThreshold = 50f; // タッチスワイプの最低移動距離

    // タッチスワイプの判定用変数
    private Vector2 startPos;
    private Vector2 currentPos;
    private bool isDragging = false;

    private Vector2 moveInput;

    public float moveSpeed = 5f;

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
        // X座標とZ座標に変換
        Vector3 dir = new Vector3(moveInput.x, 0, moveInput.y);
        transform.position += dir * moveSpeed * Time.deltaTime;

        moveInput = Vector2.zero; // 毎フレームリセット
    }

    // 接触検知
    private void OnCollisionEnter(Collision other)
    {
        // 接触したオブジェクトのタグが壁（Wall）なら死亡
        if(other.gameObject.CompareTag("Wall"))
        {
            GameManager.Instance.state = GameState.Start;
            Debug.Log("Playerが死亡しました");
        }
    }
}