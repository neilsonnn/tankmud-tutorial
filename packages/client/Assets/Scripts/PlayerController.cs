#nullable enable
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using IWorld.ContractDefinition;
using mud;
using UniRx;
using UnityEngine;
using ObservableExtensions = UniRx.ObservableExtensions;

public class PlayerController : MonoBehaviour
{
    public PlayerComponent player;

    private Camera _camera;
    private Vector3 destination;
    float distance;

    public GameObject destinationMarker;
    private TankShooting _target;
    Quaternion rotation;

    void Start() {

        player.position.OnUpdated += UpdatePosition;

        _target = GetComponentInChildren<TankShooting>(true);
        _camera = Camera.main;

    }

    private void OnDestroy() {
        if(player.position) player.position.OnUpdated -= UpdatePosition;

    }

    void UpdatePosition() {
        destination = player.position.position;   
    }

    // TODO: Send tx
    private async UniTaskVoid SendMoveTxAsync(int x, int y) {
        try {
            await TxManager.SendDirect<MoveFunction>(Convert.ToInt32(x), Convert.ToInt32(y));
        }
        catch (Exception ex) {
            // Handle your exception here
            Debug.LogException(ex);
        }
    }

    void Update() {
        UpdateInput();
        UpdateTank();
    }

    void UpdateInput() {

        if (!player.Loaded || !player.IsLocalPlayer) return;

        if (Input.GetMouseButtonDown(0)) {

            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit)) return;

            var dest = hit.point;
            dest.x = Mathf.Floor(dest.x);
            dest.y = Mathf.Floor(0f);
            dest.z = Mathf.Floor(dest.z);

            if(dest == transform.position) {return;}

            // Determine the new rotation
            distance = Vector3.Distance(transform.position, destination);
            destination = dest;
            rotation = Quaternion.LookRotation(destination - transform.position);

            destinationMarker.SetActive(true);
            destinationMarker.transform.position = dest;

            //send tx
            SendMoveTxAsync(Convert.ToInt32(dest.x), Convert.ToInt32(dest.z)).Forget();

        }
    }

    void UpdateTank() {

        var pos = transform.position;

        if (Vector3.Distance(pos, destination) > 0.1) {
            transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * distance);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 25f);
    
        } else {
            destinationMarker.SetActive(false);
        }

    }

}
