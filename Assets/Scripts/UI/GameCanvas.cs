using Archon.SwissArmyLib.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCanvas : MonoBehaviour, IResettable
{
    public CanvasGroup CanvasGroup;
    public TMPro.TextMeshProUGUI WinnerText;

    public void OnReset()
    {
    }

    private void Start()
    {
        ServiceLocator.Resolve<SpyRunGameMode>().GameStateChanged += GameStateChanged;
    }

    void GameStateChanged(SkopjeGameState state)
    {
        if(state == SkopjeGameState.Playing)
        {
            CanvasGroup.alpha = 0;
        } else
        {
            CanvasGroup.alpha = 1;
            switch (ServiceLocator.Resolve<SpyRunGameMode>().Winner)
            {
                case SkopjeWinner.Human:
                    WinnerText.text = "Human wins";
                    break;
                case SkopjeWinner.Car:
                    WinnerText.text = "Car wins";
                    break;
            }
        }
    }
}
