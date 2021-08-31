# JCS_UDPGameSocket

UDP socket class.

Functions

| Name                     | Description                                                 |
|:-------------------------|:------------------------------------------------------------|
| Connect                  | Try connect to the sever.                                   |
| OnConnect                | Callback when on connect.                                   |
| GetBytesFromInputBuffer  | Get byte array from the current input buffer memory space.  |
| GetBytesFromOutputBuffer | Get byte array from the current output buffer memory space. |
| SocketConnected          | Check if the socket connected?                              |
| SetupRecieveCallback     | Setup the callback for recieved data and loss of conneciton |
| Close                    | Close the socket.                                           |
| IsConnected              | Check is connected to the server or not.                    |
| SendPacket               | Send a sequence of byte to server.                          |
