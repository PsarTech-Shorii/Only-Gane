using Mirror;
using ScriptableObjects;
using UnityEngine;

namespace Game.OnlyGaneExample {
	public class OnlyGaneManager : NetworkBehaviour {
		[Header("Prefabs")]
		[SerializeField] private GameObject playerAvatarPrefab;

		[Header("Module")]
		[SerializeField] private SO_Boolean hasStartedGame;

		#region Server

		[Server] public override void OnStartServer() {
			base.OnStartServer();

			hasStartedGame.AddListener(_newValue => {
				if(_newValue) StartGame();
			});
		}

		[Server] private void StartGame() {
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

		#endregion
	}
}