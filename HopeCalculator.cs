using MathNet.Numerics.Statistics;
using Microsoft.EntityFrameworkCore;
using VaderSharp;

namespace Hope;

public static class HopeCalculator
{
    public static async Task<double> CalculateHope(DataContext dataContext)
    {
        var analyzer = new SentimentIntensityAnalyzer();

        var lastMonth = await dataContext.Headlines.Where(h => h.Timestamp > DateTime.Now.AddMonths(-1)).ToListAsync();
        var lastSamples = lastMonth.Where(h => h.Timestamp > DateTime.Now.AddHours(-2)).ToList();

        var lastMonthScores = lastMonth.Select(h => analyzer.PolarityScores(h.Content)).Select(score => score.Positive - score.Negative).ToList();

        var lastSamplesScores = lastSamples.Select(h => analyzer.PolarityScores(h.Content)).Select(score => score.Positive - score.Negative).ToList();

        var lastMonthStdDev = lastMonthScores.StandardDeviation();

        return ((lastSamplesScores.Average() - lastMonthScores.Average()) / lastMonthStdDev * 200) + 50;
    }
}
