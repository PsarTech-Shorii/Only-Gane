using UnityEngine;

namespace Common.Enabler {
	public class InteractionEnabler : EnablerBase {
		[SerializeField] private CanvasGroup canvasGroup;

		protected override void CheckEnable(bool newEnable) {
			canvasGroup.interactable = enable ? newEnable : !newEnable;
		}
	}
}