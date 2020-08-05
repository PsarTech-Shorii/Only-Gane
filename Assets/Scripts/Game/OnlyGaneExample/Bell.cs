using System;
using Mirror;
using ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.OnlyGaneExample {
	public class Bell : NetworkBehaviour {
		#region Server
		
		private const float VerticalBound = 9f;
		private const float HorizontalBound = 10f;

		// [SerializeField] private SO_Integer winnerNetId;
		
		/*private void OnCollisionEnter2D(Collision2D _other) {
			if(!isServer) return;

			if (_other.gameObject.CompareTag("Player")) {
				winnerNetId.Data = (int) _other.gameObject.GetComponent<NetworkIdentity>().netId;
			}
		}*/
		
		[Server] public override void OnStartServer() {
			base.OnStartServer();
			
			NetworkServer.RegisterHandler<MoveBellMsg>(_ => Move());
		}

		[Server] private void Move() {
			var position = new Vector2 {
				x = Random.Range(-HorizontalBound, HorizontalBound),
				y = Random.Range(0, VerticalBound)
			};

			transform.position = position;
			RpcMove(position);
		}

		#endregion

		#region Client

		[ClientRpc] private void RpcMove(Vector2 _position) {
			transform.position = _position;
		}

		#endregion
	}
}