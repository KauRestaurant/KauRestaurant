using System.Collections.Generic;

namespace KauRestaurant.Services
{
    public static class UIConstants
    {
        public static readonly Dictionary<string, (string icon, string bgColor)> MealCategoryInfo = new()
        {
            { "الإفطار", ("bi-sun", "#2e7d32") },
            { "الغداء", ("bi-brightness-high", "#1b5e20") },
            { "العشاء", ("bi-moon", "#004d40") }
        };

        public static readonly Dictionary<string, string> MealTypeBadgeColors = new()
        {
            { "الطبق الرئيسي", "bg-primary" },
            { "طبق جانبي", "bg-info" },
            { "حلويات", "bg-warning text-dark" },
            { "حلوى", "bg-warning text-dark" },
            { "مشروب", "bg-secondary" }
        };
    }
}