using System.Collections.Generic;

namespace Common.Bases
{
    public abstract class PassThroughList<T, TObj> : PassThroughEnumerable<T, TObj>, IReadOnlyList<TObj>
        where T : IReadOnlyList<TObj>
    {
        protected PassThroughList(T value) : base(value) { }

        public int Count => Value.Count;
        public int Length => Count;

        public TObj this[int index] => Value[index];
    }
}