JCS_NetworkSettings

Store all the network settings.

## Variables

| Name              | Description                                                  |
|:------------------|:-------------------------------------------------------------|
| clientMode        | Current mode this client in, should be update by the server! |
| switchingServer   | On switching the server?                                     |
| forceSwitchServer | Flag to check if is force switching the server.              |
| onlineMode        | Is the current game with online mode active?                 |
| protocolType      | Type of the client protocal.                                 |
| host              | Client hostname.                                             |
| port              | Client port.                                                 |
| channelCount      | Channel count in this game.                                  |

## Functions

| Name          | Description                                                      |
|:--------------|:-----------------------------------------------------------------|
| CreateNetwork | Create socket and connect to the target hostname and target port |
| CloseSocket   | Close the current socket the safe way.                           |
| GetGameSocket | Returns the game client socket.                                  |
| SwitchServer  | Switch the server.                                               |
