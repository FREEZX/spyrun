using Archon.SwissArmyLib.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPlayer : MonoBehaviour, IResettable
{
    private Vector3 startPosition;
    private Quaternion startRotation;

    public void OnReset()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;
    }

    private void Awake()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    private void Start()
    {
        ServiceLocator.Resolve<ResetManager>().RegisterResettable(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Vehicle"))
        {
            SpyRunGameMode gameMode = ServiceLocator.Resolve<SpyRunGameMode>();
            gameMode.HumanKilled();
        }
    }
}
