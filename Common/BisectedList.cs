using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Common
{
    public class BisectedList<T> : IEnumerable<T>
    {
        public T[] FirstSegment { get; }
        public T[] SecondSegment { get; }

        public BisectedList(T[] firstSegment, T[] secondSegment)
        {
            FirstSegment = firstSegment;
            SecondSegment = secondSegment;
        }

        public IEnumerator<T> GetEnumerator() => FirstSegment.Concat(SecondSegment).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
