using Archon.SwissArmyLib.Utils;
using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkopjeGameState
{
    Playing,
    Over,
}

public enum SkopjeWinner
{
    Human,
    Car,
}

public class SpyRunGameMode : MonoBehaviour, IResettable
{
    public PickableItem[] PickableItems;
    public System.Action<SkopjeGameState> GameStateChanged;
    public SkopjeGameState SkopjeGameState
    {
        get
        {
            return skopjeGameState;
        } set
        {
            skopjeGameState = value;
            GameStateChanged.Invoke(skopjeGameState);
        }
    }
    public SkopjeWinner Winner
    {
        get;
        protected set;
    }

    private SkopjeGameState skopjeGameState = SkopjeGameState.Playing;
    private int pickedItemCount;

    private void Awake()
    {
        ServiceLocator.RegisterSingleton(this);
    }

    private void Update()
    {
        if(skopjeGameState == SkopjeGameState.Over)
        {
            bool startPressed = false;
            for (int i = 0; i < ReInput.players.Players.Count; ++i)
            {
                if (ReInput.players.Players[i].GetButtonDown(RewiredConsts.Action.Start))
                {
                    startPressed = true;
                    break;
                }
            }
            if(startPressed)
            {
                ServiceLocator.Resolve<ResetManager>().DoReset();
            }
        }
    }

    private void Start()
    {
        ServiceLocator.Resolve<ResetManager>().RegisterResettable(this);
    }

    public void ItemPicked(PickableItem item)
    {
        ++pickedItemCount;

        if(PickableItems.Length == pickedItemCount)
        {
            Winner = SkopjeWinner.Human;
            SkopjeGameState = SkopjeGameState.Over;
        }
    }

    public void OnReset()
    {
        pickedItemCount = 0;
        SkopjeGameState = SkopjeGameState.Playing;
        for(int i=0; i< PickableItems.Length; ++i)
        {
            PickableItems[i].gameObject.SetActive(true);
        }
    }

    public void HumanKilled()
    {
        Winner = SkopjeWinner.Car;
        SkopjeGameState = SkopjeGameState.Over;
    }
}
