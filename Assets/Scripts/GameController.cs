using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Text[] buttonList;
    public GameObject gameOverPanel;
    public GameObject restartButton;
    public Text gameOverText;

    private string playerSide;
    private int movesCount;

    void Awake()
    {
        playerSide = "X";
        movesCount = 0;
        gameOverPanel.SetActive(false);
        restartButton.SetActive(false);
        SetGameReferenceOnButtonsController();
    }

    void ChangeSides()
    {
        playerSide = (playerSide == "X" ? "O" : "X");
    }

    public void EndTurn()
    {
        movesCount++;

        // Check winning conditions
        // Rows
        if (buttonList[0].text == playerSide && buttonList[1].text == playerSide && buttonList[2].text == playerSide)
        {
            GameOver(playerSide);
        }
        else if (buttonList[3].text == playerSide && buttonList[4].text == playerSide && buttonList[5].text == playerSide)
        {
            GameOver(playerSide);
        }
        else if (buttonList[6].text == playerSide && buttonList[7].text == playerSide && buttonList[8].text == playerSide)
        {
            GameOver(playerSide);
        }
        // Columns
        else if (buttonList[0].text == playerSide && buttonList[3].text == playerSide && buttonList[6].text == playerSide)
        {
            GameOver(playerSide);
        }
        else if (buttonList[1].text == playerSide && buttonList[4].text == playerSide && buttonList[7].text == playerSide)
        {
            GameOver(playerSide);
        }
        else if (buttonList[2].text == playerSide && buttonList[5].text == playerSide && buttonList[8].text == playerSide)
        {
            GameOver(playerSide);
        }
        // Diagonals
        else if (buttonList[0].text == playerSide && buttonList[4].text == playerSide && buttonList[8].text == playerSide)
        {
            GameOver(playerSide);
        }
        else if (buttonList[6].text == playerSide && buttonList[4].text == playerSide && buttonList[2].text == playerSide)
        {
            GameOver(playerSide);
        }

        if (movesCount >= 9)
        {
            GameOver("draw");
        }

        // Change sides
        ChangeSides();
    }

    void GameOver(string winner)
    {
        SetBoardInteractable(false);
        restartButton.SetActive(true);

        if (winner == "draw")
        {
            SetGameOverText("It's a draw!");
        }
        else
        {
            SetGameOverText(playerSide + " Wins!");
        }
    }

    public string GetPlayerSide()
    {
        return playerSide;
    }

    public void RestartGame()
    {
        playerSide = "X";
        movesCount = 0;
        gameOverPanel.SetActive(false);
        restartButton.SetActive(false);

        SetBoardInteractable(true);
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].text = "";
        }
    }

    void SetBoardInteractable(bool toggle)
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<Button>().interactable = toggle;
        }
    }

    void SetGameOverText(string text)
    {
        gameOverPanel.SetActive(true);
        gameOverText.text = text;
    }

    void SetGameReferenceOnButtonsController()
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
        }
    }
}
