using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerColor
{
    public Color panelColor;
    public Color textColor;
}

[System.Serializable]
public class Player
{
    public Image panel;
    public Text text;
    public Button button;
}

public class GameController : MonoBehaviour
{
    public Text[] buttonList;
    public GameObject gameOverPanel;
    public GameObject restartButton;
    public Text gameOverText;
    public GameObject gameStartPanel;

    public Player playerX;
    public Player playerO;
    public PlayerColor activePlayerColor;
    public PlayerColor inactivePlayerColor;

    private string player1Side;  // Player1 is the human in AI Game
    private string player2Side;  // Player2 is the computer in AI Game
    private string playerSide;
    private int movesCount;

    void Awake()
    {
        SetGameReferenceOnButtonsController();
        InitGame();
    }

    void PlayAIMove()
    {
        // Random selector AI
        /*do
        {
            index = Random.Range(0, buttonList.Length);
        } while (buttonList[index].text != "");*/

        // Mini-Max Selector
        // AI player is the maximizer here
        int index = GetMiniMaxScore(true).x;
        Debug.Log(index);
        buttonList[index].GetComponentInParent<GridSpace>().SetSpace();
    }

    Vector2Int GetMiniMaxScore(bool maximizer)
    {
        // Base Case
        if (movesCount >= 9)
        {
            return new Vector2Int(-1, 0);
        }
        else if (IsWin(player2Side))
        {
            return new Vector2Int(-1, 1);
        }
        else if (IsWin(player1Side))
        {
            return new Vector2Int(-1, -1);
        }

        Vector2Int score = new Vector2Int(-1, -1);
        if (maximizer)
        {
            score.y = -1;
        }
        else
        {
            score.y = 1;
        }
        for (int i = 0; i < buttonList.Length; i++)
        {
            if (buttonList[i].text == "")
            {
                if (maximizer)
                {
                    buttonList[i].text = player2Side;
                }
                else
                {
                    buttonList[i].text = player1Side;
                }
                movesCount++;

                Vector2Int tempScore = GetMiniMaxScore(!maximizer);

                if (maximizer && tempScore.y > score.y)
                {
                    score.y = tempScore.y;
                    score.x = i;
                }
                else if (!maximizer && tempScore.y < score.y)
                {
                    score.y = tempScore.y;
                    score.x = -1;
                }

                buttonList[i].text = "";
                movesCount--;
            }
        }
        return score;
    }

    void ChangeSides()
    {
        playerSide = (playerSide == "X") ? "O" : "X";
        if (playerSide == "X")
        {
            SetPlayerColors(playerX, playerO);
        }
        else
        {
            SetPlayerColors(playerO, playerX);
        }

        bool shouldAIPlay = (playerSide == player2Side);
        if (shouldAIPlay)
        {
            PlayAIMove();
        }
    }

    public void EndTurn()
    {
        movesCount++;

        if (IsWin(playerSide))
        {
            GameOver(playerSide);
        }
        else if (movesCount >= 9)
        {
            GameOver("draw");
        }
        else
        {
            // Change sides
            ChangeSides();
        }
    }

    void GameOver(string winner)
    {
        SetBoardInteractable(false);
        restartButton.SetActive(true);

        if (winner == "draw")
        {
            SetGameOverText("It's a draw!");
            ResetPlayerColors();
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

    void InitGame()
    {
        movesCount = 0;
        gameStartPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        restartButton.SetActive(false);
        SetBoardInteractable(false);
    }

    private bool IsWin(string player)
    {
        // Check winning conditions
        // Rows
        if (buttonList[0].text == player && buttonList[1].text == player && buttonList[2].text == player)
        {
            return true;
        }
        else if (buttonList[3].text == player && buttonList[4].text == player && buttonList[5].text == player)
        {
            return true;
        }
        else if (buttonList[6].text == player && buttonList[7].text == player && buttonList[8].text == player)
        {
            return true;
        }
        // Columns
        else if (buttonList[0].text == player && buttonList[3].text == player && buttonList[6].text == player)
        {
            return true;
        }
        else if (buttonList[1].text == player && buttonList[4].text == player && buttonList[7].text == player)
        {
            return true;
        }
        else if (buttonList[2].text == player && buttonList[5].text == player && buttonList[8].text == player)
        {
            return true;
        }
        // Diagonals
        else if (buttonList[0].text == player && buttonList[4].text == player && buttonList[8].text == player)
        {
            return true;
        }
        else if (buttonList[6].text == player && buttonList[4].text == player && buttonList[2].text == player)
        {
            return true;
        }
        return false;
    }

    void ResetPlayerColors()
    {
        playerX.panel.color = inactivePlayerColor.panelColor;
        playerX.text.color = inactivePlayerColor.textColor;
        playerO.panel.color = inactivePlayerColor.panelColor;
        playerO.text.color = inactivePlayerColor.textColor;
    }

    public void RestartGame()
    {
        movesCount = 0;
        gameStartPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        restartButton.SetActive(false);

        ResetPlayerColors();
        SetPlayerButtons(true);

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

    void SetPlayerButtons(bool toggle)
    {
        playerX.button.interactable = toggle;
        playerO.button.interactable = toggle;
    }

    void SetPlayerColors(Player newPlayer, Player oldPlayer)
    {
        newPlayer.panel.color = activePlayerColor.panelColor;
        newPlayer.text.color = activePlayerColor.textColor;
        oldPlayer.panel.color = inactivePlayerColor.panelColor;
        oldPlayer.text.color = inactivePlayerColor.textColor;
    }

    public void SetStartingSide(string startingSide)
    {
        player1Side = playerSide = startingSide;
        if (playerSide == "X")
        {
            SetPlayerColors(playerX, playerO);
            player2Side = "O";
        }
        else
        {
            SetPlayerColors(playerO, playerX);
            player2Side = "X";
        }
        SetPlayerButtons(false);
        StartGame();
    }

    void StartGame()
    {
        SetBoardInteractable(true);
        gameStartPanel.SetActive(false);
    }
}
