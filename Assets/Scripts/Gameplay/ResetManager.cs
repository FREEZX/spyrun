using Archon.SwissArmyLib.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetManager : MonoBehaviour
{
    private List<IResettable> resettables = new List<IResettable>();

    private void Awake()
    {
        ServiceLocator.RegisterSingleton(this);
    }

    public void DoReset()
    {
        for(int i=0; i<resettables.Count; ++i)
        {
            resettables[i].OnReset();
        }
    }

    public void RegisterResettable(IResettable resettable)
    {
        resettables.Add(resettable);
    }

    public void UnregisterResettable(IResettable resettable)
    {
        resettables.Remove(resettable);
    }
}
