use std::path::Path;

use serde::{Deserialize, Serialize};
use tokio::fs::{read, write};

#[derive(Default, Debug, Serialize, Deserialize)]
pub struct PersistantState {
    pub history: Vec<HopeRecord>,
}

#[derive(Debug, Serialize, Deserialize)]
pub struct HopeRecord {
    /// Seconds since the Unix Epoch
    pub unix_time: f64,
    pub value: f64,
}

impl PersistantState {
    pub async fn load_from_file(path: impl AsRef<Path>) -> anyhow::Result<Self> {
        let contents = read(path).await?;

        Ok(serde_json::from_slice(&contents)?)
    }

    pub async fn save_to_file(&self, path: impl AsRef<Path>) -> anyhow::Result<()> {
        let content = serde_json::to_vec_pretty(self)?;

        write(path, content).await?;

        Ok(())
    }
}
