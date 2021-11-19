﻿using System;
using System.Collections.Generic;

namespace OzonEdu.MerchApi.Infrastructure.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<Out> Map<In, Out>(this IEnumerable<In> source, Func<In, Out> mapper)
        {
            foreach (In item in source)
            {
                yield return mapper(item);
            }
        }
    }
}