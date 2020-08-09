using System.Linq;
using Insight;
using Mirror;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game {
	public class StartGameMsg : MessageBase {}

	public class GameCoreManager : NetworkBehaviour {
		[Header("Output")] 
		[SerializeField] private SO_Boolean isMatchLeader;
		[SerializeField] private SO_Boolean isStartedGame;

		[Header("Module")] 
		[SerializeField] private SO_Object gameServerManagerRef;

		#region Common

		private void Awake() {
			isStartedGame.Data = false;
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

			isStartedGame.Data = true;
			RpcStartGame();
		}

		[Server] public void StopGame() {
			isStartedGame.Data = false;
			RpcStopGame();
			
			gameServerManager.StopGame();
		}

		#endregion

		#region Client

		[ClientRpc] private void RpcStartGame() {
			isStartedGame.Data = true;
		}

		[ClientRpc] private void RpcStopGame() {
			isStartedGame.Data = false;
		}

		[TargetRpc] private void TargetAssignLeader(NetworkConnection _target) {
			Debug.Log("[GameCoreManager] - TargetAssignLeader");
			Assert.IsFalse(isMatchLeader.Data);
			isMatchLeader.Data = true;
		}

		#endregion
	}
}