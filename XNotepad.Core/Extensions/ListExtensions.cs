using System.Collections.Generic;

namespace XNotepad.Core.Extensions
{
    public static class ListExtensions
    {
        public static bool TryRemove<T>(this IList<T> source, T item)
        {
            if (source.Contains(item))
            {
                source.Remove(item);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
