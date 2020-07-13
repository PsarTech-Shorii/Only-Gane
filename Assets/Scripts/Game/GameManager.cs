using System.Linq;
using Mirror;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game {
	public class GameManager : NetworkBehaviour {
		private NetworkConnectionToClient _matchLeaderConn;

		[Header("Exposed")] 
		[SerializeField] private SO_Boolean isMatchLeader;

		#region Server

		[Server]
		public void SetMatchLeader() {
			var oldMatchLeader = _matchLeaderConn;
			_matchLeaderConn = NetworkServer.connections.Count > 0 ? NetworkServer.connections.First().Value : null;
			if (_matchLeaderConn == oldMatchLeader) {
				Debug.Log($"SetMatchLeader - Nothing to do");
				return;
			}

			if (oldMatchLeader != null) {
				Debug.Log($"SetMatchLeader - UnassignLeader : {oldMatchLeader}");
				TargetUnassignLeader(oldMatchLeader);
			}

			if (_matchLeaderConn != null) {
				Debug.Log($"SetMatchLeader - AssignLeader : {_matchLeaderConn}");
				TargetAssignLeader(_matchLeaderConn);
			}
		}

		#endregion

		#region Client

		[TargetRpc]
		private void TargetAssignLeader(NetworkConnection target) {
			Assert.IsFalse(isMatchLeader.Data);
			isMatchLeader.Data = true;
		}

		[TargetRpc]
		private void TargetUnassignLeader(NetworkConnection target) {
			Assert.IsTrue(isMatchLeader.Data);
			isMatchLeader.Data = true;
		}

		#endregion
	}
}