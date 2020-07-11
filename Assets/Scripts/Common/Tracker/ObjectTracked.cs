namespace Common.Tracker {
	public class ObjectTracked<T> {
		public delegate void TrackingEvent(T newValue);
		
		private T _data;
		private event TrackingEvent OnValueChanged;

		public T Data {
			get => _data;
			set {
				_data = value;
				OnValueChanged?.Invoke(_data);
			}
		}

		public void AddListener(TrackingEvent handler) {
			OnValueChanged += handler;
		}
		
		public void RemoveListener(TrackingEvent handler) {
			OnValueChanged -= handler;
		}
	}
}