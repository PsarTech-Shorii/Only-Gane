using UnityEngine;

namespace Common.Enabler {
	public class InteractionEnabler : EnablerBase {
		[SerializeField] private CanvasGroup canvasGroup;

		protected override void CheckEnable(bool _newEnable) {
			canvasGroup.interactable = enable ? _newEnable : !_newEnable;
		}
	}
}