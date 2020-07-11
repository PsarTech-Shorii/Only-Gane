using Insight;
using UnityEngine;

namespace Common.EnablerManager {
	public class SetEnablerIngame : SetEnabler {
		[Header("Module")]
		[SerializeField] private ClientGameManager clientGameManager;

		protected override void Start() {
			base.Start();
			
			if(clientGameManager.IsInGame) Enable();
			else Disable();
		}

		protected override void RegisterHandlers() {
			clientGameManager.OnGoInGame += OnGoInGame;
		}

		protected override void UnregisterHandlers() {
			clientGameManager.OnGoInGame -= OnGoInGame;
		}

		private void OnGoInGame(bool enable) {
			if(enable) Enable();
			else Disable();
		}
	}
}