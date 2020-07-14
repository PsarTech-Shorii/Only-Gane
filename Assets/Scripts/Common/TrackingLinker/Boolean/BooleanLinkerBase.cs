using ScriptableObjects;
using UnityEngine;

namespace Common.TrackingLinker.Boolean {
	public abstract class BooleanLinkerBase : LinkerBase<bool> {
		[SerializeField] private SO_Boolean target;

		protected override void OnValueChange(bool _newValue) {
			target.Data = _newValue;
		}
	}
}