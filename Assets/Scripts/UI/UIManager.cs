using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private GameObject score;
    [SerializeField] private GameObject title; 
    [SerializeField] private GameObject item; 

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

    public void ScoreActive(bool mode)
    {
        score.SetActive(mode);
    }

    public void TitleActive(bool mode)
    {
        title.SetActive(mode);
    }

    public void ItemActive(bool mode)
    {
        item.SetActive(mode);
    }
}
