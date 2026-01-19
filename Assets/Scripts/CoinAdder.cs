using UnityEngine;
using TMPro;

public class CoinAdder : MonoBehaviour
{
    [SerializeField] private int coinBalance = 0;
    [SerializeField] private TextMeshProUGUI coinText;

    void Start()
    {
        UpdateUI();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            coinBalance++;
            UpdateUI();
            Destroy(gameObject);
        }
    }

    void UpdateUI()
    {
        if (coinText != null)
        {
            coinText.text = "Coins: " + coinBalance;
        }
    }
}