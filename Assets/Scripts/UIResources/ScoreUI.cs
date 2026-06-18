using UnityEngine;
using UnityEngine.UIElements;

public class ScoreUI : MonoBehaviour
{
    public static ScoreUI Instance { get; private set; }

    private Label score;

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
        var root = GetComponent<UIDocument>().rootVisualElement;

        score = root.Q<Label>("Score");
    }

    private void Update()
    {
        score.text = $"Score : {ScoreManager.Instance.score}";
    }
}
