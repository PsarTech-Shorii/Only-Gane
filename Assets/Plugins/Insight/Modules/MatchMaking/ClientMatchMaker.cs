using UnityEngine;
using UnityEngine.Assertions;

namespace Insight {
	public class ClientMatchMaker : InsightModule {
		private InsightClient _client;
		private GameClientManager _gameModule;

		private void Awake() {
			AddDependency<GameClientManager>();
		}
		
		public override void Initialize(InsightClient client, ModuleManager manager) {
			_client = client;
			
			_gameModule = manager.GetModule<GameClientManager>();

			RegisterHandlers();
		}

		private void RegisterHandlers() {}

		public void MatchGame() {
			Assert.IsTrue(_client.IsConnected);
			Debug.Log("[Client - MatchMaker] - Match game"); 
			
			_client.NetworkSend(new MatchGameMsg {uniqueId = _gameModule.uniqueId}, 
				callbackMsg => _client.InternalSend(callbackMsg.message));
		}
	}
}