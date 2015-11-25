using UnityEngine;
using System.Collections;

public class LoadOnClick : MonoBehaviour {

    GameObject optionsPanel;
    GameObject highScoresPanel;
    GameObject selectDifficultyPanel;
    
    void Start()
    {
        hidePassiveMenus();
        optionsPanel.SetActive(false);
        highScoresPanel.SetActive(false);
        selectDifficultyPanel.SetActive(false);
    }

    public void hidePassiveMenus()
    {
        optionsPanel = GameObject.Find("OptionsPanel");
        highScoresPanel = GameObject.Find("HighScoresPanel");
        selectDifficultyPanel = GameObject.Find("SelectDifficultyPanel");
    }

	public void LoadScene(int level)
    {
        Application.LoadLevel(level);
    }

    public void OpenChildMenu(string panel)
    {
        if (panel.Equals("OptionsPanel"))
        {
            optionsPanel.SetActive(true);
        }
        else if (panel.Equals("HighScoresPanel"))
        {
            highScoresPanel.SetActive(true);
        }
        else if (panel.Equals("SelectDifficultyPanel"))
        {
            selectDifficultyPanel.SetActive(true);
        }

    }
    public void BackToMainMenu(string panel)
    {
        if (panel.Equals("OptionsPanel"))
        {
            optionsPanel.SetActive(false);
        }
        else if (panel.Equals("HighScoresPanel"))
        {
            highScoresPanel.SetActive(false);
        }
        else if (panel.Equals("SelectDifficultyPanel"))
        {
            selectDifficultyPanel.SetActive(false);
        }
    }
  
}
