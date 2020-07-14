using UnityEngine;

namespace Common.Tracker {
	public abstract class SO_TrackerBase<T> : ScriptableObject {
		private readonly ObjectTracker<T> objTracker = new ObjectTracker<T>();

		public T Data {
			get => objTracker.Data;
			set => objTracker.Data = value;
		}

		public void AddListener(ObjectTracker<T>.TrackingEvent _handler) {
			objTracker.AddListener(_handler);
		}
		
		public void RemoveListener(ObjectTracker<T>.TrackingEvent _handler) {
			objTracker.RemoveListener(_handler);
		}
	}
}