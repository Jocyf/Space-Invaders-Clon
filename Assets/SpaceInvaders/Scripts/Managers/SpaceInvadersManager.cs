using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceInvadersManager : MonoBehaviour
{
    [Header("Main Player Data")]
    public int numlives = 3;
    public int score = 0;

    [Header("InGame UI")]
    public Text scoreText;
    public Text livesText;
    public Image[] liveImages;
    public Text gameOverText;
    [Space(10)]
    public GameObject gameContainer;
    public GameObject inGameUI;
    public GameObject mainMenuPanel;
    public GameObject pauseMenuPanel;

    [Header("Skill Improvement")]
    public float ratioTimeDecrease = 0.5f;
    public float ratioByDeadIncrease = 0.0005f;

    #region Singleton
    public static SpaceInvadersManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion


    public void StartPlay()
    {
        ResetPlayer();
        BarrierManager.Instance.StartPlaying();
        EnemiesManager.Instance.StartPlaying();
        SendMessage("StartPlaying");        // UFO Msg
    }

    public void AddScore(int _score)
    {
        score += _score;
        UpdateScoreInScreen();
    }

    public void AddExtraLive()
    {
        numlives++;
        UpdateLivesInScreen();
        // Falta el sonido de vida extra
    }

    public void Die()
    {
        numlives--;
        EnemiesManager.Instance.ratio = 1f;     // Reiniciamos el ratio.
        if (numlives <= 0)
        {
            Debug.Log("Game Over");
            GameOver();
        }
        else
        {
            Debug.Log("Player Died");
        }

        UpdateLivesInScreen();
    }

    public void LevelFinished()
    {
        if (numlives <= 0)
        {
            Debug.Log("Game Over");
            GameOver();
            return;      // Check we dont die crashing with the last enemy. This is also GameOver
        }

        Debug.Log("Player Finished Level");
        BarrierManager.Instance.Reset();
        EnemiesManager.Instance.Reset();
        SendMessage("Reset");   // send reset to UFO manager

        StartCoroutine("UpdateLevelTimed"); // Start Next Level.
    }

    // Se llama desde el menu de pausa para voolver al menu principal
    public void ExitGame()
    {
        EnemiesManager.Instance.Reset();    // Paramos los enemigos. Ya no se mueven mas.
        BarrierManager.Instance.Reset();
        EnemiesManager.Instance.DestroyAllEnemies();
        SendMessage("Reset");   // send reset to UFO manager
        //yield return new WaitForSeconds(1.0f);
        ResetPlayer();
        ResetUIPanels();
    }


    public void GameOver()
    {
        EnemiesManager.Instance.Reset();    // Paramos los enemigos. Ya no se mueven mas.
        gameOverText.enabled = true;
        StartCoroutine("_LoadMainMenuTimed");
    }


    private IEnumerator _LoadMainMenuTimed()
    {
        yield return new WaitForSeconds(5.0f);
        BarrierManager.Instance.Reset();
        EnemiesManager.Instance.DestroyAllEnemies();
        SendMessage("Reset");   // send reset to UFO manager
        yield return new WaitForSeconds(1.0f);
        gameOverText.enabled = false;
        ResetUIPanels();
    }

    private IEnumerator UpdateLevelTimed()
    {
        // Skill
        EnemiesManager.Instance.changeRatioTime -= ratioTimeDecrease;
        EnemiesManager.Instance.ratioByDeadMult += ratioByDeadIncrease;

        // Start Playing new level
        yield return new WaitForSeconds(1.0f);
        BarrierManager.Instance.StartPlaying();
        EnemiesManager.Instance.StartPlaying();
        SendMessage("StartPlaying");
    }


    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1;
    }

    void Start ()
    {
        ResetPlayer();
        ResetUIPanels();
    }
	
    private void ResetPlayer()
    {
        numlives = 3;
        score = 0;
        UpdateScoreInScreen();
        UpdateLivesInScreen();
        gameOverText.enabled = false;
    }

    private void ResetUIPanels()
    {
        gameContainer.SetActive(false);
        inGameUI.SetActive(false);
        mainMenuPanel.SetActive(true);
        pauseMenuPanel.SetActive(false);
    }

    private void UpdateScoreInScreen()
    {
        scoreText.text = score.ToString();
    }

    private void UpdateLivesInScreen()
    {
        for(int i = 0; i < 3; i++)
        {
            liveImages[i].enabled = i < numlives;
        }
    }
}
