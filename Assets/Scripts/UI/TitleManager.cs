using UnityEngine;
using UnityEngine.UIElements;

public class TitleManager : MonoBehaviour
{
    public static TitleManager Instance { get; private set; }

    private Button startButton;
    private Label score;

    public float _score;

    private VisualElement root;

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

    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        startButton = root.Q<Button>("StartButton");
        score = root.Q<Label>("Result");

        startButton.clicked += () => GameStart();

        // クリックできるようにする
        root.pickingMode = PickingMode.Position;

        score.text = $"最終スコア： {_score}";
    }

    private void OnDisable()
    {
        // クリックできなくする
        root.pickingMode = PickingMode.Ignore;
    }

    private void GameStart()
    {
        GameManager.Instance.state = GameState.Play;
    }
}
