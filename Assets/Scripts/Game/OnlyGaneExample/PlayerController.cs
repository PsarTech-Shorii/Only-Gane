using Mirror;
using UnityEngine;

namespace Game.OnlyGaneExample {
	public class PlayerController : NetworkBehaviour {
		private Rigidbody2D playerRb;

		private void Awake() {
			playerRb = GetComponent<Rigidbody2D>();
		}

		private void Update() {
			if(!hasAuthority) return;
			
		}
		
		private void FixedUpdate() {
			if(!isServer) return;
			
		}

		#region Server

		#endregion

		#region Client

		[Client] public override void OnStartClient() {
			base.OnStartClient();
			playerRb.isKinematic = true;
			enabled = false;
		}

		[Client] public override void OnStartAuthority() {
			base.OnStartAuthority();
			enabled = true;
		}

		#endregion
	}
}