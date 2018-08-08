using System;

namespace SuperBasic.Utilities
{
    internal static class ExceptionUtilities
    {
        public static InvalidOperationException UnexpectedValue<TValue>(TValue value)
        {
            return new InvalidOperationException($"Unexpected value '{value}' of type '{typeof(TValue).FullName}'");
        }
    }
}
