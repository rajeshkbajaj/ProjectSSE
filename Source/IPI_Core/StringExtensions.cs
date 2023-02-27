using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IPI_Core
{
    public static class StringExtensions
    {
        public static string StripChars( this string str, IEnumerable<char> toExclude )
        {
            StringBuilder sb = new StringBuilder();
            for( int i = 0; i < str.Length; i++ )
            {
                char c = str[i];
                if  ( !toExclude.Contains(c) )
                    sb.Append( c ) ;
            }
            return( sb.ToString() ) ;
        }
        // OK: var str = s.ExceptChars(new[] { ' ', '\t', '\n', '\r' });
        // FASTER: var str = s.StripChars(new HashSet<char>(new[] { ' ', '\t', '\n', '\r' }));
    }
}
