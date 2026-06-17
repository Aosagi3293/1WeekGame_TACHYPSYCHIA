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
        ResetGame();
    }

    private void Update()
    {
        switch(state)
        {
            case GameState.Start:
                // UIToolKitを使うものとして仮置きスタート用
                if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
                {
                    StartGame();
                }

                break;

            case GameState.Play:

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
        state = GameState.Start;

        // 壁削除
        foreach (var wall in GameObject.FindGameObjectsWithTag("Wall"))
        {
            Destroy(wall);
        }

        // プレイヤー位置
        PlayerController.Instance.transform.position = Vector3.zero;

        // スコア
        ScoreManager.Instance.ResetScore();
    }
}
