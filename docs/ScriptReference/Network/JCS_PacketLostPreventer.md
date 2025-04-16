# JCS_PacketLostPreventer

Prevent the packet have been lost. If the packet is lost we
keep tracking the packet and resend the responed until the
server's response had arrived. This usually deal with the
UDP type communication.

## Variables

| Name              | Description                                            |
|:------------------|:-------------------------------------------------------|
| mResendTime       | Interval the packet resend.                            |
| mWaitingPacketIds | Packet's ID that are still waiting for resend/process. |

## Functions

| Name          | Description                                                                                                          |
|:--------------|:---------------------------------------------------------------------------------------------------------------------|
| AddTrack      | Track this packet. If the packet does not responed. keep sending the packet until the packet responed by the server. |
| IsPreventing  | Check if this preventer still preventing any data packet lost issue.                                                 |
| ClearTracking | Clear all tracking packets.                                                                                          |
