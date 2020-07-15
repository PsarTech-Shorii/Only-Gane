using ScriptableObjects;
using UnityEngine;

namespace Common.TrackingLinker.Integer {
	public abstract class IntegerLinkerBase : LinkerBase<int> {
		[SerializeField] protected SO_Integer target;

		protected override void OnValueChange(int _newValue) {
			target.Data = _newValue;
		}
	}
}