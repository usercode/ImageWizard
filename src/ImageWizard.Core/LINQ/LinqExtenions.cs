using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Core
{
    public static class LinqExtenions
    {
        public static void Foreach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach(var item in items)
            {
                action(item);
            }
        }
    }
}
