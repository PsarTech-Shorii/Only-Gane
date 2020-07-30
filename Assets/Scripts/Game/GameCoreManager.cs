using System.Linq;
using Insight;
using Mirror;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game {
	public delegate void StartGameDelegate();

	public class GameCoreManager : NetworkBehaviour {
		[SyncEvent] private event StartGameDelegate EventOnStartGame;

		[Header("Output")] 
		[SerializeField] private SO_Boolean isMatchLeader;
		[SerializeField] private SO_Boolean hasStartedGame;

		[Header("Module")] 
		[SerializeField] private SO_Object gameServerManagerRef;

		#region Common

		private void Awake() {
			hasStartedGame.Data = false;
		}

		private void OnStartGame() {
			Debug.Log($"[GameCoreManager] - OnStartGame");
			hasStartedGame.Data = true;
		}

		#endregion
		
		#region Server

		private GameServerManager gameServerManager;
		private NetworkConnection matchLeaderConn;
		
		
		[Server] public override void OnStartServer() {
			base.OnStartServer();
			Debug.Log("[GameCoreManager] - OnStartServer");
			
			gameServerManager = (GameServerManager) gameServerManagerRef.Data;
			Assert.IsNotNull(gameServerManager);
			
			NetworkServer.RegisterHandler<StartGameMsg>(_ => StartGame());
		}

		[Server] public void RegisterPlayer(NetworkConnection _connection) {
			Debug.Log("[GameCoreManager] - RegisterPlayer");
			AssignLeader(_connection);
		}

		[Server] public void UnregisterPlayer(NetworkConnection _connection) {
			Debug.Log("[GameCoreManager] - UnregisterPlayer");
			UnassignLeader(_connection);
		}

		[Server] private void AssignLeader(NetworkConnection _connection) {
			if (matchLeaderConn != null) return;

			Debug.Log("[GameCoreManager] - AssignLeader");
			TargetAssignLeader(_connection);
			matchLeaderConn = _connection;
		}

		[Server] private void UnassignLeader(NetworkConnection _connection) {
			if (matchLeaderConn != _connection) return;
			
			Debug.Log("[GameCoreManager] - UnassignLeader");
			matchLeaderConn = null;
			if (NetworkServer.connections.Count > 0) AssignLeader(NetworkServer.connections.First().Value);
		}

		[Server] private void StartGame() {
			if(!gameServerManager.StartGame()) return;
			
			EventOnStartGame?.Invoke();
			OnStartGame();
		}

		#endregion

		#region Client

		[Client] public override void OnStartClient() {
			base.OnStartClient();
			
			EventOnStartGame = OnStartGame;
		}

		[TargetRpc] private void TargetAssignLeader(NetworkConnection _target) {
			Debug.Log("[GameCoreManager] - TargetAssignLeader");
			Assert.IsFalse(isMatchLeader.Data);
			isMatchLeader.Data = true;
		}

		#endregion
	}
}