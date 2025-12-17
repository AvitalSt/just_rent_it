using System.Text.RegularExpressions;

namespace JustRentItAPI.Validation
{
    static class FullNameValidation
    {
        public static bool IsValidName(this string name)
        {
            // בודק שהמחרוזת לא ריקה ושאורך השם לפחות 2 תווים
            return !string.IsNullOrWhiteSpace(name) && name.Trim().Length >= 2;
        }
    }
}
