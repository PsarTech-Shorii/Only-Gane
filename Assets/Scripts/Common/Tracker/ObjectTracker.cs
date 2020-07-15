namespace Common.Tracker {
	public class ObjectTracker<T> {
		public delegate void Tracker(T _newValue);
		
		private T data;
		private event Tracker OnValueChanged;

		public T Data {
			get => data;
			set {
				data = value;
				OnValueChanged?.Invoke(data);
			}
		}

		public void AddListener(Tracker _handler) {
			OnValueChanged += _handler;
		}
		
		public void RemoveListener(Tracker _handler) {
			OnValueChanged -= _handler;
		}
	}
}