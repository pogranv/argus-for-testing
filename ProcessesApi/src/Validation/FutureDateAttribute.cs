using System;
using System.ComponentModel.DataAnnotations;

namespace ProcessesApi.Validation
{
    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null) {
                return true;
            }

            if (value is DateTime date)
            {
                return date >= DateTime.Now;
            }
            return false;
        }
    }
} 