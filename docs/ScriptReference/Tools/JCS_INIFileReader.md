# JCS_INIFileReader

Initialize file reader, utility will read the `.ini` or `.properties` file.

## Functions

| Name        | Description                              |
|:------------|:-----------------------------------------|
| ReadINIFile | Read the .ini file and returns it value. |

## Example

```cs
  Dictionary<string, string> data = JCS_INIFileReader.ReadINIFile(Application.dataPath + "/example.ini");

  Debug.Log(EDITOR_INI["author"]);  // Output: Author Name
  Debug.Log(EDITOR_INI["email"]);   // Output: example@email.com
```

example.ini

```cs
  author=Author Name
  email=example@email.com
```
