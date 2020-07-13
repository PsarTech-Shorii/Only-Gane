using Insight;
using UnityEngine;

namespace Common.TrackingLinker.Boolean {
	public class IngameLinker : BooleanLinkerBase {
		[Header("Module")]
		[SerializeField] private GameClientManager gameClientManager;
		
		protected override void Start() {
			base.Start();
			OnValueChange(gameClientManager.IsInGame);
		}

		protected override void RegisterHandlers() {
			gameClientManager.OnGoInGame += OnValueChange;
		}

		protected override void UnregisterHandlers() {
			gameClientManager.OnGoInGame -= OnValueChange;
		}
	}
}