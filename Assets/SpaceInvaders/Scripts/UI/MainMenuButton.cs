using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButton : MonoBehaviour
{

	public void PressButton ()
    {
        SpaceInvadersManager.Instance.gameContainer.SetActive(true);
        SpaceInvadersManager.Instance.inGameUI.SetActive(true);
        SpaceInvadersManager.Instance.StartPlay();
        SpaceInvadersManager.Instance.mainMenuPanel.SetActive(false);
        SpaceInvadersManager.Instance.pauseMenuPanel.SetActive(false);
    }
	
}
