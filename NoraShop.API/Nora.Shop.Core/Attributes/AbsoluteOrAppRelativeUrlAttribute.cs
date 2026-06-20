using System.ComponentModel.DataAnnotations;

namespace Nora.Shop.Core.Attributes
{
    public sealed class AbsoluteOrAppRelativeUrlAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is null)
            {
                return true;
            }

            var text = value as string;
            if (string.IsNullOrWhiteSpace(text))
            {
                return true;
            }

            if (Uri.TryCreate(text, UriKind.Absolute, out _))
            {
                return true;
            }

            return text.StartsWith("/", StringComparison.Ordinal) ||
                   text.StartsWith("~/", StringComparison.Ordinal);
        }
    }
}
