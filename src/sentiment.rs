pub fn sentiment(text: &str) -> f64 {
    let analyzer = vader_sentiment::SentimentIntensityAnalyzer::new();
    let scores = analyzer.polarity_scores(text);
    scores["pos"] - scores["neg"]
}
