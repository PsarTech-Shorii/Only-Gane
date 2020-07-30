using Mirror;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game {
	public class Player : NetworkBehaviour {
		#region Server

		private GameCoreManager gameCoreManager;

		[Header("Module")]
		[SerializeField] private SO_Object gameCoreManagerRef;

		[Server] private void Initilize() {
			gameCoreManager = (GameCoreManager) gameCoreManagerRef.Data;
			Assert.IsNotNull(gameCoreManager);
		}
		
		[Server] public override void OnStartServer() {
			base.OnStartServer();
			Initilize();
			
			gameCoreManager.RegisterPlayer(connectionToClient);
		}

		public override void OnStopServer() {
			base.OnStopServer();
			
			gameCoreManager.UnregisterPlayer(connectionToClient);
		}

		#endregion
	}
}