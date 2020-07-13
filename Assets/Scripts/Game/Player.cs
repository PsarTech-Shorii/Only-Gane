using Mirror;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game {
	public class Player : NetworkBehaviour {
		private GameManager _gameManager;
		
		[Header("Module")]
		[SerializeField] private SO_Object gameManagerRef;
		
		[Server] public override void OnStartServer() {
			base.OnStartServer();
			InitializeServer();
			
			_gameManager.SetMatchLeader();
		}

		[Server] private void InitializeServer() {
			_gameManager = (GameManager) gameManagerRef.Data;
			Assert.IsNotNull(_gameManager);
		}
		
		[Server] public override void OnStopServer() {
			base.OnStopServer();
			
			_gameManager.SetMatchLeader();
		}
	}
}