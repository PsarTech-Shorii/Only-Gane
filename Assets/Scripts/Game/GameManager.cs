using System.Linq;
using Insight;
using Mirror;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game {
	public delegate void StartGameDelegate(bool _newValue);
	
	public class GameManager : NetworkBehaviour {
		private GameServerManager gameServerManager;
		private NetworkConnection matchLeaderConn;
		
		[SyncEvent] private event StartGameDelegate EventOnStartGame;

		[Header("Output")] 
		[SerializeField] private SO_Boolean isMatchLeader;
		[SerializeField] private SO_Boolean hasStartedGame;

		[Header("Module")]
		[SerializeField] private SO_Object gameServerManagerRef;

		#region Server

		public override void OnStartServer() {
			base.OnStartServer();
			gameServerManager = (GameServerManager) gameServerManagerRef.Data;
			Assert.IsNotNull(gameServerManager);
		}

		[Server] public void RegisterPlayer(NetworkConnection _connection) {
			AssignLeader(_connection);
		}
		
		[Server] public void UnregisterPlayer(NetworkConnection _connection) {
			if(matchLeaderConn != _connection) return;
			UnassignLeader(_connection);
		}
		
		[Server] private void AssignLeader(NetworkConnection _connection) {
			if (matchLeaderConn != null) return;
			
			TargetAssignLeader(_connection);
			matchLeaderConn = _connection;
		}
		
		[Server] private void UnassignLeader(NetworkConnection _connection) {
			TargetUnassignLeader(_connection);
			matchLeaderConn = null;
			if(NetworkServer.connections.Count > 0) AssignLeader(NetworkServer.connections.First().Value);
		}

		[Command(ignoreAuthority = true)] private void CmdStartGame() {
			EventOnStartGame?.Invoke(gameServerManager.StartGame());
		}

		#endregion

		#region Client

		[Client] public override void OnStartClient() {
			base.OnStartClient();
			EventOnStartGame = _newValue => {
				Debug.Log($"[GameManager] - EventOnStartGame : {_newValue}");
				if(hasStartedGame.Data != _newValue) hasStartedGame.Data = _newValue;
			};
		}
		
		[Client] public void StartGame() => CmdStartGame();

		[TargetRpc] private void TargetAssignLeader(NetworkConnection _target) {
			Debug.Log("[GameManager] - TargetAssignLeader");
			Assert.IsFalse(isMatchLeader.Data);
			isMatchLeader.Data = true;
		}

		[TargetRpc] private void TargetUnassignLeader(NetworkConnection _target) {
			Debug.Log("[GameManager] - TargetUnassignLeader");
			Assert.IsTrue(isMatchLeader.Data);
			isMatchLeader.Data = false;
		}

		#endregion
	}
}