using UnityEngine;

namespace Common.EnablerManager {
	public class InteractionEnabler : Enabler {
		[SerializeField] private CanvasGroup canvasGroup;

		protected override void CheckEnable(bool newEnable) {
			canvasGroup.interactable = enable ? newEnable : !newEnable;
		}
	}
}