using UnityEngine;

public class PetController : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] public SoundManager soundManager;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private float followDistance = 1f;
    [SerializeField] private float coinPickupDistance = 5f;

    public bool isActive = true;

    private void Update()
    {
        if (isActive)
        {
            FollowPlayer();
            CheckForCoinPickup();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            soundManager.PlayPetGetSound();
            ActivatePet();
        }
    }

    private void ActivatePet()
    {
        isActive = true;
        gameObject.SetActive(true);
    }

    private void FollowPlayer()
    {
        Vector3 targetPosition = player.transform.position - player.transform.right * followDistance;
        transform.position = targetPosition;
    }

    private void CheckForCoinPickup()
    {
        GameObject[] coins = GameObject.FindGameObjectsWithTag("Item");

        foreach (GameObject coin in coins)
        {
            if (Vector2.Distance(transform.position, coin.transform.position) < coinPickupDistance)
            {
                PickupCoin(coin);
                break;
            }
        }
    }

    private void PickupCoin(GameObject coin)
    {
        int points = 0;
        bool isBronze = coin.name.Contains("Bronze");
        bool isSilver = coin.name.Contains("Silver");
        bool isGold = coin.name.Contains("Gold");

        if (isBronze)
        {
            points = 50;
        }
        else if (isSilver)
        {
            points = 70;
        }
        else if (isGold)
        {
            points = 100;
        }

        soundManager.PlayCoinSound();
        gameManager.AddScore(points);

        coin.SetActive(false);
    }

    public void TakeDamage()
    {
        soundManager.PlayPetDieSound();
        isActive = false;
        gameObject.SetActive(false);
        Debug.Log("펫이 대신 피격되었습니다.");
    }
}
