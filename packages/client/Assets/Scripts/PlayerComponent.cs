using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using mudworld;

public class PlayerComponent : MUDComponent
{
    public static PlayerComponent LocalPlayer;
    public bool IsLocalPlayer;

    protected override void PostInit() {
        base.PostInit();

        // add to CameraControl's Targets array
        var cameraControl = GameObject.Find("CameraRig").GetComponent<CameraControl>();
        cameraControl.m_Targets.Add(transform);

    }
    
    protected override void UpdateComponent(MUDTable table, UpdateInfo updateInfo) {

        if(NetworkManager.LocalKey == Entity.Key) {
            IsLocalPlayer = true;
            LocalPlayer = this;
        }

        if(Loaded) {

        } else {

    
        }
    }

}
