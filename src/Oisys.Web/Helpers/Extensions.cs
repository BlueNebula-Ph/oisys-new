using System;
using System.Linq;
using OisysNew.DTO;
using OisysNew.Models;

namespace OisysNew.Helpers
{
    public static class Extensions
    {
        public static SummaryResult<T1> ToSummaryResult<T, T1>(this IQueryable<T> queryable, FilterRequestBase filter)
        {
           return null;
        }
        
    }

    public class SummaryResult<T>
    {

    }
}