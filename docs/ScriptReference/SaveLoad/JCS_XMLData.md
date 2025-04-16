# JCS_XMLData

Interface of storing game data as XML format.

## Example

ExampleGameData.cs

```cs
[System.Serializable]
public class ExampleGameData : JCS_XMLData {
    public string Name = "";  // Name of the player.
    public string Gold = "";  // Cash in the game.
}
```

## Usage

```cs
/* Setup the path. */
string filePath = "/path/to/save/game.data";

/* Load the data. */
var gameData = ExampleGameData.LoadFromFile<ExampleGameData>(filePath);

/* Save the data. */
gameData.Save<ExampleGameData>(filePath);
```

## Functions

| Name         | Description                   |
|:-------------|:------------------------------|
| Save         | Save the game data to file.   |
| LoadFromFile | Load the game data from file. |
