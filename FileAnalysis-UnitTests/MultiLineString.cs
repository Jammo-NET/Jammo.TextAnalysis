using System.Collections;
using System.Collections.Generic;

namespace JammaNalysis_UnitTests
{
    public class MultiLineString : IEnumerable<char>
    {
        private string value;

        public MultiLineString(string value = "")
        {
            this.value = value;
        }

        public void Concat(string str) => value += str;

        public static implicit operator string(MultiLineString multiLine) => multiLine.ToString();
        public static implicit operator MultiLineString(string str) => new(str);
        
        public static MultiLineString operator+(MultiLineString multiLine, string str)
        {
            return new MultiLineString(multiLine.value + (string.IsNullOrEmpty(multiLine.value) ? "" : "\n") + str);
        }

        public IEnumerator<char> GetEnumerator()
        {
            return value.GetEnumerator();
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return value;
        }
    }
}