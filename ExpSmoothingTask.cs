using System.Collections.Generic;


namespace yield
{
    public static class ExpSmoothingTask
    {
        public static IEnumerable<DataPoint> SmoothExponentialy(this IEnumerable<DataPoint> data, double alpha)
        {
            var enumerator = data.GetEnumerator();
            DataPoint previous = null;
            if (enumerator.MoveNext())
            {
                previous = enumerator.Current;
                yield return previous = previous.WithExpSmoothedY(previous.OriginalY);
            }
            while (enumerator.MoveNext())
            {
                var smooth = alpha * enumerator.Current.OriginalY + (1 - alpha) * previous.ExpSmoothedY;
                yield return previous = enumerator.Current.WithExpSmoothedY(smooth);
            }
        }
    }
}
