using System.Collections;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.Text;

namespace Jammo.CsAnalysis.Compilation
{
    public readonly struct IndexSpan : IEnumerable<int>
    {
        public readonly int Start;
        public readonly int End;
        
        public int Size => End - Start;
        
        public IndexSpan(int start, int end)
        {
            Start = start;
            End = end;
        }

        public static implicit operator IndexSpan(TextSpan span) => new(span.Start, span.End);

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