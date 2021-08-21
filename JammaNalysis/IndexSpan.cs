using System.Collections;
using System.Collections.Generic;

namespace JammaNalysis
{
    public class IndexSpan : IEnumerable<int>
    {
        public int Start;
        public int End;

        public int Size => End - Start;

        public IEnumerator<int> GetEnumerator()
        {
            for (var index = Start; index < End; index++)
                yield return index;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}