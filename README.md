# The Hope Meter

The best way to get up and running on the motivations and story behind this project is to read [Quantifying Hope on a Global Scale](https://elijahpotter.dev/articles/quantifying_hope_on_a_global_scale).

## Build

Before you can do anything, you have to prepare the SQLite database.
This is done with the [`dotnet ef`](https://learn.microsoft.com/en-us/ef/core/cli/dotnet) command.

```bash
dotnet ef database update
```

After that, it is trivial to build the entire app.
Assuming you are on a Linux system (or MacOS, but I haven't tested it), you can run:

```bash
./build.sh
```

It will read out the location of the build artifacts.

## Development

You can individually work on the ASP.NET Core app by running:

```bash
dotnet run
```

You can also individually work on the Svelte app by running:

```bash
cd site
yarn dev
```
