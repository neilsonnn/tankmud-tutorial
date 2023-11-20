#nullable enable
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using IWorld.ContractDefinition;
using mud;
using UniRx;
using UnityEngine;
using ObservableExtensions = UniRx.ObservableExtensions;

public class PlayerController : MonoBehaviour
{
    private Camera _camera;
    private Vector3 destination;
    private PositionComponent posComponent;

    public GameObject destinationMarker;

    private bool _hasDestination;
    private IDisposable? _disposer;
    private TankShooting _target;
    private PlayerSync _player;

    void Start()
    {
        _camera = Camera.main;
        var target = FindObjectOfType<TankShooting>();
        if (target == null) return;
        _target = target;

        var ds = NetworkManager.Instance.ds;

        _player = GetComponent<PlayerSync>();

        posComponent.OnUpdated += UpdatePosition;

    }


    void UpdatePosition() {
        destination = posComponent.position;   
    }

    // TODO: Send tx
    private async UniTaskVoid SendMoveTxAsync(int x, int y)
    {
        try {
            await TxManager.SendDirect<MoveFunction>(Convert.ToInt32(x), Convert.ToInt32(y));
        }
        catch (Exception ex) {
            // Handle your exception here
            Debug.LogException(ex);
        }
    }

    void Update()
    {
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

        // TODO: Early return if not local player 
        if (!_player.IsLocalPlayer() || _target.RangeVisible) return;

        if (Input.GetMouseButtonDown(0))
        {
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

    private void OnDestroy()
    {
        _disposer?.Dispose();
    }
}
