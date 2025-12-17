namespace JustRentItAPI.Utils
{
    public static class FilterParser
    {
        public static List<int>? ParseIds(string? input)
        {
            if (string.IsNullOrEmpty(input)) return null;

            var ids = input
                .Split('_')
                .Select(s => int.TryParse(s, out var n) ? (int?)n : null)
                .Where(n => n.HasValue)
                .Select(n => n.Value)
                .ToList();

            return ids.Any() ? ids : null;
        }

        public static T? ParseEnum<T>(string? input) where T : struct, Enum
        {
            if (string.IsNullOrEmpty(input)) return null;

            return Enum.TryParse<T>(input, true, out var result) ? result : null;
        }

        public static List<T>? ParseEnums<T>(string? input) where T : struct, Enum
        {
            if (string.IsNullOrEmpty(input)) return null;

            var parts = input.Split('_')
                             .Select(s => Enum.TryParse<T>(s, true, out var val) ? (T?)val : null)
                             .Where(e => e.HasValue)
                             .Select(e => e.Value)
                             .ToList();

            return parts.Any() ? parts : null;
        }
    }
}
