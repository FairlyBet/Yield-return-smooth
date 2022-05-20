using System.Collections.Generic;


namespace yield
{
    public readonly struct MovingMaxSearcher
    {
        private readonly int _windowWidth;
        private readonly LinkedList<DataPoint> _window;
        private readonly LinkedList<DataPoint> _potentialMax;


        public MovingMaxSearcher(int windowWidth)
        {
            _windowWidth = windowWidth;
            _window = new LinkedList<DataPoint>();
            _potentialMax = new LinkedList<DataPoint>();
        }


        public void Add(DataPoint point)
        {
            _window.AddLast(point);
            if (_window.Count > _windowWidth)
            {
                RemoveFirst();
            }
            AddToPotentialMax();
        }


        private void RemoveFirst()
        {
            if (_potentialMax.Count > 0
                && _window.First.Value == _potentialMax.First.Value)
            {
                _potentialMax.RemoveFirst();
            }
            _window.RemoveFirst();
        }


        private void AddToPotentialMax()
        {
            var last = _window.Last.Value;

            if (_potentialMax.Count == 0)
            {
                _potentialMax.AddLast(last);
                return;
            }

            var lastPotential = _potentialMax.Last.Value;
            if (last.OriginalY <= lastPotential.OriginalY)
            {
                _potentialMax.AddLast(last);
                return;
            }

            if (last.OriginalY > _potentialMax.First.Value.OriginalY)
            {
                _potentialMax.Clear();
                _potentialMax.AddLast(last);
                return;
            }

            do
            {
                _potentialMax.RemoveLast();
            } while (last.OriginalY > _potentialMax.Last.Value.OriginalY);

            _potentialMax.AddLast(last);
        }


        public DataPoint GetMax()
        {
            var max = _potentialMax.First.Value;
            return _window.Last.Value.WithMaxY(max.OriginalY);
        }
    }


    public static class MovingMaxTask
    {
        public static IEnumerable<DataPoint> MovingMax
            (this IEnumerable<DataPoint> data, int windowWidth)
        {
            var maxSearcher = new MovingMaxSearcher(windowWidth);
            foreach (var item in data)
            {
                maxSearcher.Add(item);
                yield return maxSearcher.GetMax();
            }
        }
    }
}
