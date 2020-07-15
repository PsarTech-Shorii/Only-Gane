using Mirror;
using UnityEngine;

namespace Common.TrackingLinker.Integer {
	public class MaxPlayersLinker : IntegerLinkerBase {
		[Header("Module")]
		[SerializeField] private NetworkManager netManager;
		
		protected override void Start() {
			base.Start();
			target.Data = netManager.maxConnections;
		}

		protected override void RegisterHandlers() {
			target.AddListener(_playerCount => netManager.maxConnections = _playerCount);
		}

		protected override void UnregisterHandlers() {
			target.RemoveListener(_playerCount => netManager.maxConnections = _playerCount);
		}
	}
}