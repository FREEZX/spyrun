using Archon.SwissArmyLib.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : MonoBehaviour, IResettable
{
    public string PickerTag;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(PickerTag))
        {
            ServiceLocator.Resolve<SpyRunGameMode>().ItemPicked(this);
            gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        ServiceLocator.Resolve<ResetManager>().RegisterResettable(this);
    }

    public void OnReset()
    {
        gameObject.SetActive(true);
    }
}
