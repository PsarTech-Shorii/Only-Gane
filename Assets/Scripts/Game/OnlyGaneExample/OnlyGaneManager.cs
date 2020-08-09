using Mirror;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game.OnlyGaneExample {
	public abstract class ControllMsg : MessageBase {}
	public class MoveBellMsg : ControllMsg {}

	public class OnlyGaneManager : NetworkBehaviour {
		[Header("Prefabs")]
		[SerializeField] private GameObject playerAvatarPrefab;

		[Header("Input")]
		[SerializeField] private SO_Boolean isStartedGame;
		
		[Header("Output")]
		[SerializeField] private SO_Boolean isMatchWinner;

		[Header("Module")]
		[SerializeField] private SO_Object gameCoreManagerRef;

		#region Server

		private GameCoreManager gameCoreManager;
		
		[Server] public override void OnStartServer() {
			base.OnStartServer();
			
			gameCoreManager = (GameCoreManager) gameCoreManagerRef.Data;
			Assert.IsNotNull(gameCoreManager);
			
			isStartedGame.AddListener(_newValue => {
				if(_newValue) StartGame();
			});
		}

		[Server] private void StartGame() {
			RpcStartGame();
			
			SpawnPlayers();
		}

		[Server] private void SpawnPlayers() {
			var numPlayer = NetworkServer.connections.Count;
			var playerPositionY = playerAvatarPrefab.transform.position.y;
			var i = 0;

			foreach (var connection in NetworkServer.connections.Values) {
				var position = new Vector2 {
					x = (i / (float) (numPlayer - 1)) * 10 - 5,
					y = playerPositionY
				};
				var playerAvatarObj = Instantiate(playerAvatarPrefab, position, Quaternion.identity);
				
				NetworkServer.Spawn(playerAvatarObj);
				playerAvatarObj.GetComponent<NetworkIdentity>().AssignClientAuthority(connection);
				
				i++;
			}
		}

		[Server] public void FinishGame(NetworkConnection _winnerConn) {
			TargetAssignWinner(_winnerConn);
			gameCoreManager.StopGame();
		}

		#endregion

		#region Client

		[ClientRpc] private void RpcStartGame() {
			isMatchWinner.Data = false;
		}
		
		[TargetRpc] private void TargetAssignWinner(NetworkConnection _target) {
			isMatchWinner.Data = true;
		}

		#endregion
	}
}