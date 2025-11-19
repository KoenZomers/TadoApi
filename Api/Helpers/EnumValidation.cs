namespace KoenZomers.Tado.Api.Helpers;

/// <summary>
/// Helpers to work with enums
/// </summary>
internal static class EnumValidation
{
    /// <summary>
    /// Ensures the provided enum is valid for the range of allowed enum options
    /// </summary>
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