using Mirror;
using UnityEngine;

namespace Common.TrackingLinker.Boolean {
	public class ConnectingLinker : BooleanLinkerBase {
		[Header("Module")]
		[SerializeField] private Transport transport;
		
		protected override void Start() {
			base.Start();
			target.Data = transport.ClientConnected();
		}

		protected override void RegisterHandlers() {
			transport.OnClientConnected.AddListener(() => {OnValueChange(true);});
			transport.OnClientDisconnected.AddListener(() => {OnValueChange(false);});
		}

		protected override void UnregisterHandlers() {
			transport.OnClientConnected.RemoveListener(() => {OnValueChange(true);});
			transport.OnClientDisconnected.RemoveListener(() => {OnValueChange(false);});
		}
	}
}