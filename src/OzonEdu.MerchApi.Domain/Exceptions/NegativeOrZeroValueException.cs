﻿using System;

namespace OzonEdu.MerchApi.Domain.Exceptions
{
    public class NegativeOrZeroValueException : Exception
    {
        public NegativeOrZeroValueException(string message) : base(message)
        {
        }

        public NegativeOrZeroValueException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}