using System.Collections;
using System.Collections.Generic;

namespace Common.Bases
{
    public abstract class PassThroughEnumerable<T> : PassThrough<T>, IEnumerable 
        where T : IEnumerable
    {
        protected PassThroughEnumerable(T value) : base(value) { }

        IEnumerator IEnumerable.GetEnumerator() => Value.GetEnumerator();
    }

    public abstract class PassThroughEnumerable<T, TObj> : PassThroughEnumerable<T>, IEnumerable<TObj>
        where T : IEnumerable<TObj>
    {
        protected PassThroughEnumerable(T value) : base(value) { }

        public IEnumerator<TObj> GetEnumerator() => Value.GetEnumerator();
    }
}