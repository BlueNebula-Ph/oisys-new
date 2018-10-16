﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace OisysNew.Helpers
{
    public static class Extensions
    {
        public static bool IsNullOrZero(this int? number)
        {
            return !number.HasValue || (number.HasValue && number.Value == 0);
        }

        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetName();
        }
    }
}
