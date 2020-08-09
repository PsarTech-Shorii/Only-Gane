using Mirror;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace Game.OnlyGaneExample {
	public class Bell : NetworkBehaviour {
		[Header("Module")]
		[SerializeField] private SO_Object onlyGaneManagerRef;

		#region Server

		private OnlyGaneManager onlyGaneManager;
		
		private const float VerticalBound = 9f;
		private const float HorizontalBound = 10f;

		private void OnCollisionEnter2D(Collision2D _other) {
			if(!isServer) return;

			if (_other.gameObject.CompareTag("Player")) {
				Debug.Log("[Bell] - FinishGame !");
				
				var winnerConn = _other.gameObject.GetComponent<NetworkIdentity>().connectionToClient;
				onlyGaneManager.FinishGame(winnerConn);
			}
		}
		
		[Server] public override void OnStartServer() {
			base.OnStartServer();
			
			onlyGaneManager = (OnlyGaneManager) onlyGaneManagerRef.Data;
			Assert.IsNotNull(onlyGaneManager);
			
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