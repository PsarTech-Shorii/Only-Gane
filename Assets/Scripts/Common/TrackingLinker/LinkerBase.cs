using UnityEngine;

namespace Common.TrackingLinker {
	public abstract class LinkerBase<T> : MonoBehaviour {
		protected virtual void Start() {
			RegisterHandlers();
		}

		protected virtual void OnDestroy() {
			UnregisterHandlers();
		}

		protected abstract void RegisterHandlers();
		protected abstract void UnregisterHandlers();
		protected abstract void OnValueChange(T newValue);
	}
}