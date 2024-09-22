using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Ãß°¡

public class GameManager : MonoBehaviour
{
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    public int health = 3;
    public PlayerController player;
    [SerializeField] SoundManager soundManager;

    [SerializeField] TextMeshProUGUI gameOverText;
    [SerializeField] TextMeshProUGUI gameClearText;
    [SerializeField] TextMeshProUGUI scoreText;

    [SerializeField] private Image[] hearts;

    private bool reStart = false;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        UpdateScoreUI();
        UpdateHealthUI();
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
            UpdateHealthUI();
        }
        else
        {
            player.OnDie();
            soundManager.StopBGM();
            gameOverText.gameObject.SetActive(true);
            reStart = true;
        }
    }

    private void UpdateHealthUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].gameObject.SetActive(true);
            }
            else
            {
                hearts[i].gameObject.SetActive(false);
            }
        }
    }
}
