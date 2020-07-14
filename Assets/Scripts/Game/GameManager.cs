using System.Linq;
using Mirror;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game {
	public class GameManager : NetworkBehaviour {
		private NetworkConnectionToClient matchLeaderConn;

		[Header("Exposed")] 
		[SerializeField] private SO_Boolean isMatchLeader;

		#region Server

		[Server]
		public void SetMatchLeader() {
			var oldMatchLeader = matchLeaderConn;
			matchLeaderConn = NetworkServer.connections.Count > 0 ? NetworkServer.connections.First().Value : null;
			if (matchLeaderConn == oldMatchLeader) {
				Debug.Log($"SetMatchLeader - Nothing to do");
				return;
			}

			if (oldMatchLeader != null) {
				Debug.Log($"SetMatchLeader - UnassignLeader : {oldMatchLeader}");
				TargetUnassignLeader(oldMatchLeader);
			}

			if (matchLeaderConn != null) {
				Debug.Log($"SetMatchLeader - AssignLeader : {matchLeaderConn}");
				TargetAssignLeader(matchLeaderConn);
			}
		}

		#endregion

		#region Client

		[TargetRpc]
		private void TargetAssignLeader(NetworkConnection _target) {
			Assert.IsFalse(isMatchLeader.Data);
			isMatchLeader.Data = true;
		}

		[TargetRpc]
		private void TargetUnassignLeader(NetworkConnection _target) {
			Assert.IsTrue(isMatchLeader.Data);
			isMatchLeader.Data = true;
		}

		#endregion
	}
}