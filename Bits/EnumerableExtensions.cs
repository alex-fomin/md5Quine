using System.Collections.Generic;
using System.Text;

namespace System.Linq
{
    static class EnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(this IEnumerable<IEnumerable<T>> sequences)
        {
            IEnumerable<IEnumerable<T>> emptyProduct = new[] { Enumerable.Empty<T>() };
            return sequences.Aggregate(
                emptyProduct,
                (accumulator, sequence) =>
                from accseq in accumulator
                from item in sequence
                select accseq.Concat(new[] { item }));
        }

		public static string ToString<T>(this IEnumerable<T> items, string separator)
		{
			var sb = new StringBuilder();
			sb = items.Aggregate(sb, (current, item) => current.Append(item).Append(separator));
			return sb.ToString();
		}
    }
}