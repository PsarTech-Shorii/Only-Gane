using UnityEngine;

namespace Common.Tracker {
	public abstract class SO_TrackedBase<T> : ScriptableObject {
		private readonly ObjectTracked<T> _object = new ObjectTracked<T>();

		public T Data {
			get => _object.Data;
			set => _object.Data = value;
		}

		public void AddListener(ObjectTracked<T>.TrackingEvent handler) {
			_object.AddListener(handler);
		}
		
		public void RemoveListener(ObjectTracked<T>.TrackingEvent handler) {
			_object.RemoveListener(handler);
		}
	}
}