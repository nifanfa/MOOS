
namespace System {
	public abstract class Array {
#pragma warning disable 649
		// This field should be the first field in Array as the runtime/compilers depend on it
		internal int _numComponents;
#pragma warning restore

		public int Length {
			get {
				// NOTE: The compiler has assumptions about the implementation of this method.
				// Changing the implementation here (or even deleting this) will NOT have the desired impact
				return _numComponents;
			}
		}
	}

	public class Array<T> : Array { }
}