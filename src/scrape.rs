use rss::{Channel, Item};
use tokio::sync::mpsc::unbounded_channel;

async fn get_feed_url_items(url: &str) -> anyhow::Result<Vec<Item>> {
    let content = reqwest::get(url).await?.bytes().await?;

    let channel = Channel::read_from(&content[..])?;
    Ok(channel.into_items())
}

pub async fn get_all_feed_urls(urls: impl IntoIterator<Item = &str>) -> anyhow::Result<Vec<Item>> {
    let mut items_receiver = {
        let (items_sender, items_receiver) = unbounded_channel();

        for url in urls {
            let url = url.to_string();
            let items_sender = items_sender.clone();
            tokio::spawn(async move {
                items_sender.send(get_feed_url_items(&url).await).unwrap();
            });
        }
        items_receiver
    };

    let mut res = Vec::new();

    while let Some(items_res) = items_receiver.recv().await {
        let mut items = items_res?;
        res.append(&mut items);
    }

    Ok(res)
}
