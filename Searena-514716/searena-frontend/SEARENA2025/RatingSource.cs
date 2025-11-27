using Guna.UI2.WinForms;

namespace SEARENA2025
{
    public interface IRatingSource
    {
        int GetRating();
    }

    public class GunaRatingStarSource : IRatingSource
    {
        private readonly Guna2RatingStar _star;
        public GunaRatingStarSource(Guna2RatingStar star) => _star = star;
        public int GetRating() => _star != null ? (int)_star.Value : 5;
    }
}
