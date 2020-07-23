using Mirror;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game {
	public class Player : NetworkBehaviour {
		private GameManager gameManager;
		
		[Header("Module")]
		[SerializeField] private SO_Object gameManagerRef;
		
		[Server] public override void OnStartServer() {
			base.OnStartServer();
			InitializeServer();
			
			gameManager.RegisterPlayer(connectionToClient);
		}

		[Server] private void InitializeServer() {
			gameManager = (GameManager) gameManagerRef.Data;
			Assert.IsNotNull(gameManager);
		}
		
		[Server] public override void OnStopServer() {
			base.OnStopServer();
			
			gameManager.UnregisterPlayer(connectionToClient);
		}
	}
}