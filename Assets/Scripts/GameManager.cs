using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
  bool _isGameOver = false;

  public void Update()
  {
    if (Input.GetKeyDown(KeyCode.R) && _isGameOver)
    {
      SceneManager.LoadScene(1);
    }

    if (Input.GetKeyDown(KeyCode.Escape))
    {
      Application.Quit();
    }
  }

  public void GameOver()
  {
    _isGameOver = true;
  }
}
