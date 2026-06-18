using UnityEngine;
using UnityEngine.InputSystem; 

public enum GameState
{
    None,
    Start,
    Play
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState state = GameState.Start;

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
        state = GameState.Start;

        ResetGame();
    }

    private void Update()
    {
        if(UIManager.Instance == null) return;

        switch(state)
        {
            case GameState.Start:
                UIManager.Instance.TitleActive(true);
                UIManager.Instance.ScoreActive(false);
                UIManager.Instance.ItemActive(false);

                break;

            case GameState.Play:
                UIManager.Instance.ScoreActive(true);
                UIManager.Instance.ItemActive(true);
                UIManager.Instance.TitleActive(false);

                break;
        }
    }

    // ゲームのスタート処理
    private void StartGame()
    {
        state = GameState.Play;
        Time.timeScale = 1f;
    }

    public void ResetGame()
    {
        // 壁削除
        foreach (var wall in GameObject.FindGameObjectsWithTag("Wall"))
        {
            Destroy(wall);
        }

        // プレイヤー位置
        PlayerController.Instance.ResetPlayer();

        // スコア
        ScoreManager.Instance.ResetScore();

        // アイテム
        ItemManager.Instance.Reset();
    }
}
