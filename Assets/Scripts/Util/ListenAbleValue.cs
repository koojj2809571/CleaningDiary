namespace Util
{
    
    public class ListenAbleValue<T>
    {

        public delegate void Observe(T changeValue);
        private T _value;
        private Observe _observe;

        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                _observe(_value);
            }
        }

        public ListenAbleValue(ref T value)
        {
            _value = value;
        }

        public void SetObserve(Observe o)
        {
            _observe = o;
        }
        
    }
}