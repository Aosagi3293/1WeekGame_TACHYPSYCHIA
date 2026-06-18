using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using System.Collections;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }

    private bool canUseItem = true;

    // クールダウン（60秒）
    private float coolDownTime = 60f;
    private float coolDownTimer = 0f;

    // スローにする時間
    private float slowlyTime = 3f;

    // スロー倍率
    private float slowlyMultiplication = 0.5f;

    private VisualElement root;

    private Button button;

    [SerializeField] private Texture2D itemTexture;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        button = root.Q<Button>("Item");

        if (button != null)
        {
            button.clicked += () => TryUseItem();
        }

        // クリックできるようにする
        root.pickingMode = PickingMode.Position;
    }

    private void OnDisable()
    {
        // クリックできなくする
        root.pickingMode = PickingMode.Ignore;
    }

    private void Update()
    {
        // アイテム使用後のクールダウン処理
        if (!canUseItem)
        {
            coolDownTimer += Time.unscaledDeltaTime;

            if (coolDownTime <= coolDownTimer)
            {
                canUseItem = true;
            }

            button.style.backgroundImage = new StyleBackground();
        }
        else
        {
            button.style.backgroundImage = Background.FromTexture2D(itemTexture);
        }

        // スペースキーが押されたらアイテムを使う
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            TryUseItem();
        }
    }

    private void TryUseItem()
    {
        if(GameManager.Instance == null) return;
        if(GameManager.Instance.state != GameState.Play) return;

        Debug.Log("アイテムの使用が検知されました");

        // アイテムが使える状態なら使う
        if (canUseItem)
        {
            StartCoroutine(ActivateSlowMotion());
        }
    }

    // スローモーションと時間を元に戻す処理を管理するコルーチン
    private IEnumerator ActivateSlowMotion()
    {
        canUseItem = false;
        coolDownTimer = 0f;

        // ゲーム内時間の速度変化
        Time.timeScale = slowlyMultiplication;

        // スロー時間を経過させる
        float elapsed = 0f;
        while (elapsed < slowlyTime)
        {
            elapsed += Time.unscaledDeltaTime;
            yield return null; // 次のフレームまで待機
        }

        // ゲーム内時間を元に戻す
        Time.timeScale = 1f;
    }

    public void Reset()
    {
        coolDownTimer = 0;

        canUseItem = true;
    }
}
