using System;

namespace KoenZomers.Tado.Api.Helpers
{
    internal static class EnumValidation
    {
        internal static void EnsureEnumWithinRange<TEnum>(TEnum value) where TEnum : struct, IConvertible
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException($"{nameof(TEnum)} must be an enumeration type.");
            }

            if (!Enum.IsDefined(typeof(TEnum), value))
            {
                throw new ArgumentOutOfRangeException($"Value: '{value}' is not within enumeration range of type: '{typeof(TEnum).Name}'");
            }
        }
    }
}
