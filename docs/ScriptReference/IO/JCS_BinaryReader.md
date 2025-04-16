 # JCS_BinaryReader

Custom binary reader.

## Functions

| Name                            | Description                                                                    |
|:--------------------------------|:-------------------------------------------------------------------------------|
| ReadBoolean                     | Read boolean from buffer stream.                                               |
| ReadChars                       | Read number of chars from buffer stream.                                       |
| ReadChar                        | Read char from buffer stream.                                                  |
| ReadBytes                       | Read number of bytes from buffer stream.                                       |
| ReadByte                        | Read a byte from buffer stream.                                                |
| ReadString                      | Read String from buffer stream.                                                |
| ReadSingle                      | Read single from buffer stream.                                                |
| ReadFloat                       | Read float from buffer stream.                                                 |
| ReadDouble                      | Read double from buffer stream.                                                |
| ReadShort                       | Read Short from buffer stream.                                                 |
| ReadUShort                      | Read Unsigned short from buffer stream.                                        |
| ReadInt                         | Read Integer from buffer stream.                                               |
| ReadUInt                        | Read Unsigned Integer from buffer stream.                                      |
| ReadLong                        | Read Long from buffer stream.                                                  |
| ReadULong                       | Read Unsigned Long from buffer stream.                                         |
| ReadNullTerminatedASCIIString   | Read ASCII until the null byte appear and put data into a string data.         |
| ReadNullTerminatedUnicodeString | Read Unicode until the null byte appear and put data into a string data.       |
| ReadNullTerminatedUTF8String    | Read UTF-8 string until the null byte appear and put data into a string data.  |
| ReadNullTerminatedUTF32String   | Read UTF-32 string until the null byte appear and put data into a string data. |
| Available                       | Available data left in this stream.                                            |
| SetFloatPercise                 | How the float precise? (Default : 1000)                                        |
