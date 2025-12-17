using System.Text.RegularExpressions;

namespace JustRentItAPI.Validation
{
    static class PhoneValidation
    {
        // Regex למספר ישראלי: מתחיל ב-05 או 0 עם ספרה נוספת, כולל 7 ספרות נוספות
        private static readonly Regex PhoneRegex = new Regex(
            @"^(?:\+972|0)(5\d{8})$",
            RegexOptions.Compiled);

        public static bool IsValidPhone(this string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            return PhoneRegex.IsMatch(phone);
        }
    }
}
