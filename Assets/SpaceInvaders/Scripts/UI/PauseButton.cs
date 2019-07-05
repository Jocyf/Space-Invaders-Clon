using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{

	public void PressButton ()
    {
        SpaceInvadersManager.Instance.PauseGame();
        SpaceInvadersManager.Instance.inGameUI.SetActive(false);
        SpaceInvadersManager.Instance.pauseMenuPanel.SetActive(true);
    }
	
}
