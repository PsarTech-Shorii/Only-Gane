using Mirror;
using UnityEngine;

namespace Common.EnablerManager {
	public class SetEnablerConnecting : SetEnabler {
		[Header("Module")]
		[SerializeField] private Transport transport;

		protected override void Start() {
			base.Start();
			
			if(transport.ClientConnected()) Enable();
			else Disable();
		}

		protected override void RegisterHandlers() {
			transport.OnClientConnected.AddListener(Enable);
			transport.OnClientDisconnected.AddListener(Disable);
		}

		protected override void UnregisterHandlers() {
			transport.OnClientConnected.RemoveListener(Enable);
			transport.OnClientDisconnected.RemoveListener(Disable);
		}
	}
}