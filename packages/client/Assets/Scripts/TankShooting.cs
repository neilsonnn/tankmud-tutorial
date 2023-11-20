using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using IWorld.ContractDefinition;
using mud;
using UnityEngine;

public class TankShooting : MonoBehaviour
{
	public Transform _tankTurret;
	private bool _rangeVis;
	public bool RangeVisible => _rangeVis;
	private Camera _camera;
	private Renderer _renderer;
	private bool _fired;

	// Start is called before the first frame update
	void Start() {
		_camera = Camera.main;
		_renderer = GetComponentInChildren<Renderer>(true);
		_renderer.enabled = false;
	}

	// Update is called once per frame
	void Update() {

		var ray = _camera.ScreenPointToRay(Input.mousePosition);
		if (!Physics.Raycast(ray, out var hit)) return;

		Vector3 dest = hit.point;
		dest.x = Mathf.RoundToInt(dest.x);
		dest.y = Mathf.RoundToInt(dest.y);
		dest.z = Mathf.RoundToInt(dest.z);

		transform.position = dest;

		//Turret rotation
        var turretRotation = Quaternion.LookRotation(transform.position - _tankTurret.position);
        var newTurretRotation = Quaternion.Lerp(transform.rotation, turretRotation, Time.deltaTime);
        _tankTurret.rotation = newTurretRotation;

		if (Input.GetKey(KeyCode.E)) {
			if (!_rangeVis) {
				_renderer.enabled = true;
				_rangeVis = true;
			}
		} else {
			if (!_rangeVis) return;
			_renderer.enabled = false;
			_rangeVis = false;
		}

		if (Input.GetMouseButtonDown(0) && !_fired) {
			_fired = true;
			SendFireTxAsync((int)dest.x, (int)dest.z).Forget();
			_fired = false;
		}
	}

	private async UniTaskVoid SendFireTxAsync(int x, int y) {
		try {
			await TxManager.SendDirect<AttackFunction>(System.Convert.ToInt32(x), System.Convert.ToInt32(y));
		}
		catch (System.Exception ex)
		{
			Debug.LogException(ex);
		}
	}
}
