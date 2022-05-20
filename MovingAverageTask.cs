using System.Collections.Generic;


namespace yield
{
    public static class MovingAverageTask
    {
        public static IEnumerable<DataPoint> MovingAverage
            (this IEnumerable<DataPoint> data, int windowWidth)
        {
            var points = new Queue<DataPoint>();
            var average = 0d;
            foreach (var item in data)
            {
                if (points.Count == windowWidth)
                {
                    average += 1d
                        / windowWidth
                        * (item.OriginalY - points.Dequeue().OriginalY);
                    points.Enqueue(item);
                    yield return item.WithAvgSmoothedY(average);
                    continue;
                }
                average += (item.OriginalY - average) / (points.Count + 1);
                points.Enqueue(item);
                yield return item.WithAvgSmoothedY(average);
            }
        }
    }
}
