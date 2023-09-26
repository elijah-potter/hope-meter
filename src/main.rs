use scraper::{Html, Selector};

#[tokio::main]
async fn main() -> anyhow::Result<()> {
    let mut headlines = get_headline_urls().await?;

    let results = futures::future::join_all(headlines.drain(..).map(get_news_page)).await;

    let mut total = 0.0;

    for res in results {
        let res = res?;

        let analyzer = vader_sentiment::SentimentIntensityAnalyzer::new();
        let scores = analyzer.polarity_scores(&res);
        let unified = scores["pos"] - scores["neg"];
        total += unified;
    }

    dbg!(total);

    Ok(())
}

async fn get_headline_urls() -> anyhow::Result<Vec<String>> {
    let doc = get_page_doc("https://www.reuters.com/").await?;

    Ok(doc
        .select(&Selector::parse(r#"a[data-testid="Heading"]"#).unwrap())
        .flat_map(|element| {
            element
                .value()
                .attr("href")
                .map(|url| format!("https://reuters.com{}", url))
        })
        .collect())
}

async fn get_news_page(url: String) -> anyhow::Result<String> {
    let doc = get_page_doc(&url).await?;

    Ok(doc
        .select(&Selector::parse(r#"p[data-testid*="paragraph"]"#).unwrap())
        .fold(String::new(), |mut acc, el| {
            el.text().for_each(|t| acc.push_str(t));
            acc
        }))
}

async fn get_page_doc(url: &str) -> anyhow::Result<Html> {
    let res = reqwest::get(url).await?;
    let html = res.text().await?;

    Ok(Html::parse_document(&html))
}
