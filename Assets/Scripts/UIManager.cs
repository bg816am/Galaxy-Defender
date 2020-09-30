using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameManager _gameManager;
    private Player _player;
    [SerializeField] private Text ammoCount = default;
    [SerializeField] private Text restartText = default;
    [SerializeField] private float flickerTime = 0.05f;
    [SerializeField] private Text gameOverText = default;
    [SerializeField] private Image livesImage = default;
    [SerializeField] Text scoreText = default;
    [SerializeField] private Sprite[] livesSprites = null;
    void Start()
    {
        restartText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _player = FindObjectOfType<Player>();
        scoreText.text = "Score: " + 0;
        //assign text to handle
        if (_gameManager == null)
        {
            Debug.LogError("Game Manager not loaded");
        }
    }
    
    void Update()
    {
        scoreText.text = "Score: " + _player.GetScore();
        ammoCount.text = "Ammo: " + _player.GetAmmoCount();
    }

    public void UpdateLives(int currentLives)
    {
        //display image sprite based on index
        if (currentLives < 0)
        {
            currentLives = 0;
        }
        livesImage.sprite = livesSprites[currentLives];
    }

    public void GameOver()
    {
        _gameManager.GameOver();
        StartCoroutine(FlickerEffect());
        restartText.gameObject.SetActive(true);
    }

    IEnumerator FlickerEffect()
    {
        for (int i = 0; i < 1000; i++)
        {
            gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(flickerTime);
            gameOverText.gameObject.SetActive(false); 
            yield return new WaitForSeconds(flickerTime);
        }
        
    }
    
}
