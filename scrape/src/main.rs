mod persistance;
mod scrape;
mod sentiment;

use std::time::{SystemTime, UNIX_EPOCH};

use persistance::PersistantState;
use sentiment::sentiment;

use crate::scrape::get_all_feed_urls;

#[tokio::main]
async fn main() -> anyhow::Result<()> {
    let feed_urls = include_str!("../rss_sources.txt")
        .split('\n')
        .filter(|v| !v.is_empty());

    let sum = get_all_feed_urls(feed_urls)
        .await?
        .iter()
        .flat_map(|item| item.title())
        .fold(0.0, |acc, v| sentiment(v) + acc);

    let file_path = "state.json";

    let mut persistant_state = PersistantState::load_from_file(file_path)
        .await
        .unwrap_or_default();

    let unix_time = SystemTime::now().duration_since(UNIX_EPOCH)?.as_secs_f64();
    persistant_state.history.push(persistance::HopeRecord {
        unix_time,
        value: sum,
    });

    persistant_state.save_to_file(file_path).await?;

    dbg!(sum);

    Ok(())
}
