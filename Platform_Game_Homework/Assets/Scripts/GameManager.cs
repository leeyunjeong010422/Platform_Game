using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    public int health = 3;
    public PlayerController player;

    [SerializeField] TextMeshProUGUI gameOverText;
    [SerializeField] TextMeshProUGUI gameClearText;
    [SerializeField] TextMeshProUGUI scoreText;

    private bool reStart = false;

    private void Start()
    {
        UpdateScoreUI();
    }

    private void Update()
    {
        if (reStart && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    private void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void GameClear()
    {
        gameClearText.gameObject.SetActive(true);
        reStart = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            HealthDown();

            collision.transform.position = new Vector3(-6, -1.56f, -1);
        }

    }
    public void AddScore(int points)
    {
        stagePoint += points;
        totalPoint += points;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        scoreText.text = "Score: " + stagePoint;
    }

    public void HealthDown()
    {
        if (health > 0)
        {
            health--;
        }
        else
        {
            player.OnDie();

            gameOverText.gameObject.SetActive(true);
            reStart = true;
        }
    }
}
