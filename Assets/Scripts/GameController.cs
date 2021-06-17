using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Text[] buttonList;

    void Awake()
    {
        SetGameReferenceOnButtonsController();
    }

    public void EndTurn()
    {
        Debug.Log("Endturn is not implemented");
    }

    public string GetPlayerSide()
    {
        return "?";
    }

    void SetGameReferenceOnButtonsController()
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
        }
    }
}
