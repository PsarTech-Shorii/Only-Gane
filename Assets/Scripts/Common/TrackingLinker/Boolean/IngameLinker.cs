using Insight;
using UnityEngine;

namespace Common.TrackingLinker.Boolean {
	public class IngameLinker : BooleanLinkerBase {
		[Header("Module")]
		[SerializeField] private ClientGameManager clientGameManager;
		
		protected override void Start() {
			base.Start();
			OnValueChange(clientGameManager.IsInGame);
		}

		protected override void RegisterHandlers() {
			clientGameManager.OnGoInGame += OnValueChange;
		}

		protected override void UnregisterHandlers() {
			clientGameManager.OnGoInGame -= OnValueChange;
		}
	}
}