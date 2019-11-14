using Archon.SwissArmyLib.Utils;
using Rewired;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WheelConfiguration
{
    public WheelCollider WheelCollider;
    public float HandlingMult;
    public float BrakeMult;
    public float TorqueMult;
}

public class VehicleController : MonoBehaviour, IResettable
{
    public WheelConfiguration[] WheelConfigurations;
    public float Acceleration;
    public float Breaking;
    public float Handling;
    public float TopSpeed;

    public float DriftStartForce;
    public float DriftEndForce;

    public Rigidbody VehicleRigid;

    protected Vector3 currentSpeed;
    protected Vector3 direction;

    protected bool isDrifting;

    protected Vector3 startPosition;
    protected Quaternion startRotation;

    public void OnReset()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;
        for (int i=0; i< WheelConfigurations.Length; ++i)
        {
            WheelConfigurations[i].WheelCollider.motorTorque = 0;
        }
        VehicleRigid.velocity = Vector3.zero;
        VehicleRigid.angularVelocity = Vector3.zero;
    }

    private void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        ServiceLocator.Resolve<ResetManager>().RegisterResettable(this);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Handle input
        Vector2 movementInput = ReInput.players.GetPlayer(RewiredConsts.Player.VehiclePlayer).GetAxis2D(RewiredConsts.Action.Horizontal, RewiredConsts.Action.Vertical);

        for(int i=0; i< WheelConfigurations.Length; ++i)
        {
            WheelConfigurations[i].WheelCollider.steerAngle = movementInput.x * WheelConfigurations[i].HandlingMult;

            float angle = Vector3.SignedAngle(VehicleRigid.velocity, VehicleRigid.transform.forward, Vector3.up);
            // Handling breaking and reverse differently. If moving close enough to forward, brake. Otherwise, move backwards
            WheelConfigurations[i].WheelCollider.motorTorque = movementInput.y * WheelConfigurations[i].TorqueMult;
        }
    }
}
