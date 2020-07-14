using ScriptableObjects;
using UnityEngine;

namespace Common.Enabler {
	public abstract class EnablerBase : MonoBehaviour {
		[SerializeField] protected SO_Boolean enabler;
		[SerializeField] protected bool enable = true;

		private void Start() {
			enabler.AddListener(CheckEnable);
			CheckEnable(enabler.Data);
		}

		private void OnDestroy() {
			enabler.RemoveListener(CheckEnable);
		}

		protected abstract void CheckEnable(bool _newEnable);
	}
}