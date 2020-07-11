using Mirror;
using UnityEngine;

namespace Common.TrackingLinker.Integer {
	public class MaxPlayersLinker : IntegerLinkerBase {
		[Header("Module")]
		[SerializeField] private NetworkManager netManager;
		
		protected override void Start() {
			base.Start();
			OnValueChange(netManager.maxConnections);
		}

		protected override void RegisterHandlers() {}
		protected override void UnregisterHandlers() {}
	}
}