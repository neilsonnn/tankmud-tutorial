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

    public GameObject destinationMarker;
    private TankShooting _target;

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

        if (!player.Loaded || !player.IsLocalPlayer || _target.RangeVisible) return;

        if (Input.GetMouseButtonDown(0)) {
            destinationMarker.SetActive(true);

            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit)) return;
            if (hit.collider.name != "floor-large") return;

            var dest = hit.point;
            dest.x = Mathf.Floor(dest.x);
            dest.y = Mathf.Floor(dest.y);
            dest.z = Mathf.Floor(dest.z);
            destination = dest;
            SendMoveTxAsync(Convert.ToInt32(dest.x), Convert.ToInt32(dest.z)).Forget();
        }
    }

    void UpdateTank() {

        var pos = transform.position;
        if (Vector3.Distance(pos, destination) > 0.5) {

            var newPosition = Vector3.Lerp(transform.position, destination, Time.deltaTime);
            transform.position = newPosition;

            // Determine the new rotation
            var lookRotation = Quaternion.LookRotation(destination - transform.position);
            var newRotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime);
            transform.rotation = newRotation;
    
        } else {
            destinationMarker.SetActive(false);
        }

    }

}
