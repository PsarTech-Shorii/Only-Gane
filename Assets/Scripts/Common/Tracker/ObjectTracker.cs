namespace Common.Tracker {
	public class ObjectTracker<T> {
		public delegate void TrackingEvent(T _newValue);
		
		private T data;
		private event TrackingEvent OnValueChanged;

		public T Data {
			get => data;
			set {
				data = value;
				OnValueChanged?.Invoke(data);
			}
		}

		public void AddListener(TrackingEvent _handler) {
			OnValueChanged += _handler;
		}
		
		public void RemoveListener(TrackingEvent _handler) {
			OnValueChanged -= _handler;
		}
	}
}