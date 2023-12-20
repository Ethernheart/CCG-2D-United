using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private int battleIndex = -1;

    [SerializeField] private Animator barAnimator;

    [SerializeField] private GameObject[] infoTextsList;
    [SerializeField] private GameObject helpPanel;

    [SerializeField] private Button[] buttons;
	private void Start()
    {
        buttons[0].interactable = true;
        for (int i = 1; i < buttons.Length; i++)
        {
            if(PlayerPrefs.GetInt("IsFightWon" + i) == 1)
                buttons[i].interactable = true;
            else
                buttons[i].interactable = false;
        }
    }
    private void TextChanger(int index)
    {
        if (infoTextsList[index] != null)
            infoTextsList[index].SetActive(true);
        for (int i = 0; i < infoTextsList.Length; i++)
        {
            if(i != index)
                infoTextsList[i].SetActive(false);
        }
    }
    public void BattleChose(int battleIndex)
    {
        if (battleIndex != this.battleIndex)
        {
            barAnimator.SetTrigger("ButtonPressed");
            this.battleIndex = battleIndex;
            TextChanger(battleIndex);
        }
    }
    public void FightStart()
    {
        int sceneIndex = battleIndex+3;
        SceneManager.LoadScene(sceneIndex);
    }
    public void HelpPanelManager(bool isOpened)
    {
        helpPanel.SetActive(isOpened);
    }
    public void Quit()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
