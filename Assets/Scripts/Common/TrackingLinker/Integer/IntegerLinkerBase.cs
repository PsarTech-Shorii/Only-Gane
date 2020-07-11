using ScriptableObjects;
using UnityEngine;

namespace Common.TrackingLinker.Integer {
	public abstract class IntegerLinkerBase : LinkerBase<int> {
		[SerializeField] private SO_Integer target;

		protected override void OnValueChange(int newValue) {
			target.Data = newValue;
		}
	}
}