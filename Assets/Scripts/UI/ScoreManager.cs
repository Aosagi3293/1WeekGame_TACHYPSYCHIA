using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    private float timer = 0f;
    public float score = 0;

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
        if(GameManager.Instance == null) return;
        if(GameManager.Instance.state != GameState.Play) return;

        timer += Time.deltaTime;

        if(timer >= 0.1f)
        {
            score += 1;

            timer -= 0.1f;
        }
    }

    public void ResetScore()
    {
        TitleManager.Instance._score = score;

        score = 0;
    }
}
