using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jwtApi.Helpers
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> OrEmptyIfNull<T>(this IEnumerable<T> source) =>
            source ?? Enumerable.Empty<T>();
    }
}
