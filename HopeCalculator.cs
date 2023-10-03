using Microsoft.EntityFrameworkCore;
using VaderSharp;

namespace Hope;

public static class HopeCalculator
{
    public static async Task<double> CalculateHope(DataContext dataContext)
    {
        var analyzer = new SentimentIntensityAnalyzer();

        var lastMonth = await dataContext.Headlines.Where(h => h.Timestamp > DateTime.Now.AddMonths(-1)).ToListAsync();
        var lastDay = lastMonth.Where(h => h.Timestamp > DateTime.Now.AddDays(-1)).ToList();

        var lastMonthScores = lastMonth.Select(h => analyzer.PolarityScores(h.Content)).Select(score => score.Positive - score.Negative).ToList();
        var lastDayScores = lastDay.Select(h => analyzer.PolarityScores(h.Content)).Select(score => score.Positive - score.Negative).ToList();

        var lastMonthMin = lastDayScores.Min();
        var lastMonthMax = lastDayScores.Max();

        return (lastDayScores.Average() - lastMonthMin) / Math.Abs(lastMonthMax - lastMonthMin) * 100;
    }
}
