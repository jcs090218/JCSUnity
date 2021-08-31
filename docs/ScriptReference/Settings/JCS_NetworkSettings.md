JCS_NetworkSettings

Store all the network settings.

## Variables

| Name                | Description                                                  |
|:--------------------|:-------------------------------------------------------------|
| CLIENT_MODE         | Current mode this client in, should be update by the server! |
| ON_SWITCH_SERVER    | On switching the server?                                     |
| FORCE_SWITCH_SERVER | Flag to check if is force switching the server.              |
| ONLINE_MODE         | Is the current game with online mode active?                 |
| PROTOCAL_TYPE       | Type of the client protocal.                                 |
| HOST_NAME           | Client hostname.                                             |
| PORT                | Client port.                                                 |
| CHANNEL_COUNT       | Channel count in this game.                                  |

## Functions

| Name          | Description                                                      |
|:--------------|:-----------------------------------------------------------------|
| CreateNetwork | Create socket and connect to the target hostname and target port |
| CloseSocket   | Close the current socket the safe way.                           |
| GetGameSocket | Returns the game client socket.                                  |
| SwitchServer  | Switch the server.                                               |
