using UnityEngine;

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

    private void Update()
    {
        switch(state)
        {
            case GameState.Start:

                break;

            case GameState.Play:

                break;
        }
    }
}
