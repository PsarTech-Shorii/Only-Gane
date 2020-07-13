using ScriptableObjects;
using UnityEngine;

namespace Common {
	public class RegisterObject : MonoBehaviour {
		[SerializeField] private SO_Object objRef;
		[SerializeField] private Object obj;

		private void Awake() {
			objRef.Data = obj;
		}

		private void OnDestroy() {
			objRef.Data = null;
		}
	}
}