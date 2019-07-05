using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuButton : MonoBehaviour
{

    public enum PauseMenuType { CONTINUE = 0, MAINMENU = 1 }

    public PauseMenuType pauseMenuTypeButton = PauseMenuType.CONTINUE;


    public void ButtonPressed()
    {
        switch (pauseMenuTypeButton)
        {
            case PauseMenuType.CONTINUE:
                SpaceInvadersManager.Instance.gameContainer.SetActive(true);
                SpaceInvadersManager.Instance.inGameUI.SetActive(true);
                SpaceInvadersManager.Instance.mainMenuPanel.SetActive(false);
                SpaceInvadersManager.Instance.pauseMenuPanel.SetActive(false);
                SpaceInvadersManager.Instance.UnPauseGame();
                break;
            case PauseMenuType.MAINMENU:
                SpaceInvadersManager.Instance.gameContainer.SetActive(false);
                SpaceInvadersManager.Instance.inGameUI.SetActive(false);
                SpaceInvadersManager.Instance.mainMenuPanel.SetActive(true);
                SpaceInvadersManager.Instance.pauseMenuPanel.SetActive(false);
                Time.timeScale = 1;
                SpaceInvadersManager.Instance.ExitGame();
                break;
        }
    }


    /*IEnumerator _ExitGameTimed()
    {
        Time.timeScale = 1;
        SpaceInvadersManager.Instance.ExitGame();
    }*/
}
