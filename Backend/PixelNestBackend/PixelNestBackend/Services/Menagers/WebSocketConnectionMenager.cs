using PixelNestBackend.Dto.WebSockets;
using PixelNestBackend.Models;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PixelNestBackend.Services.Menagers
{
    public class WebSocketConnectionMenager
    {
        private readonly ConcurrentDictionary<string, List<string>> _rooms = new();
        private readonly ConcurrentDictionary<string, WebSocket> _connections = new();

        public void AddSocket(WebSocket socket, string userID)
        {
          _connections.TryAdd(userID, socket);
        }

        
        public void JoinRoom(string roomID,string reversedRoomID, string connectionID)
        {
            string actualRoomID;

            if (_rooms.ContainsKey(roomID))
            {
                actualRoomID = roomID;
            }
            else if (_rooms.ContainsKey(reversedRoomID))
            {
                actualRoomID = reversedRoomID;
            }
            else
            {
                
                actualRoomID = roomID;
                _rooms[actualRoomID] = new List<string>();
            }

           
            if (!_rooms[actualRoomID].Contains(connectionID))
            {
                _rooms[actualRoomID].Add(connectionID);
                
            }
          
          
        }
        public void LeaveRoom(string roomID, string reversedRoomID, string connectionID)
        {
            string actualRoomID;

            if (_rooms.ContainsKey(roomID))
            {
                actualRoomID = roomID;
            }
            else if (_rooms.ContainsKey(reversedRoomID))
            {
                actualRoomID = reversedRoomID;
            }
            else
            {
                actualRoomID = roomID;
            }
            _rooms[actualRoomID].Remove(connectionID);
        }
        public async Task SendMessageToUser(string receiver, string sender, string message)
        {
            string roomID = $"{sender}-{receiver}";
            string reversedRoomID = $"{receiver}-{sender}";

            string actualRoomID = _findRoom(roomID, reversedRoomID);
            WebSocketMessage webSocketMessage = new WebSocketMessage
            {
                SenderUsername = sender,
                TargetUser = receiver,
                RoomID = actualRoomID,
                Content = message
            };
           
            if (_isUserInRoom(actualRoomID, receiver)) 
            {
                Console.Write("Ovde");
                webSocketMessage.Type = "Room";
                string jsonMessage = System.Text.Json.JsonSerializer.Serialize(webSocketMessage);
                _sendMessageToRoom(actualRoomID, receiver, jsonMessage);
                
            }else if (_connections.TryGetValue(receiver, out var webSocket) && webSocket.State == WebSocketState.Open)
            {
                webSocketMessage.Type = "Direct";
                string jsonMessage = System.Text.Json.JsonSerializer.Serialize(webSocketMessage);
                var buffer = Encoding.UTF8.GetBytes(jsonMessage);
                
                await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
        private async Task _sendMessageToRoom(string roomID, string targetUser, string message)
        {
            if (!_rooms.ContainsKey(roomID)) { return; }
            foreach (var connectionId in _rooms[roomID])
            {
                if (_connections.TryGetValue(connectionId, out var socket) && socket.State == WebSocketState.Open && connectionId.Equals(targetUser))
                {
                    var buffer = Encoding.UTF8.GetBytes(message);
                    await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }
        private bool _isUserInRoom(string roomID, string receiver)
        {
            if (!_rooms.ContainsKey(roomID))
            {
                return false;
            }

            foreach (var connectionID in _rooms[roomID])
            {
                if (_connections.TryGetValue(connectionID, out var socket) && connectionID == receiver)
                {
                    return true;
                }
            }
            return false;
        }
        private string _findRoom(string roomID, string reversedRoomID)
        {
            string actualRoomID;
            if (_rooms.ContainsKey(roomID))
            {
                actualRoomID = roomID;
            }
            else if (_rooms.ContainsKey(reversedRoomID))
            {
                actualRoomID = reversedRoomID;
            }
            else
            {
                actualRoomID = roomID;
            }
            return actualRoomID;
        }
        public void CleanUpConnections()
        {
            foreach(var connection in _connections)
            {
                if(connection.Value.State != WebSocketState.Open)
                {
                    _connections.TryRemove(connection.Key, out _);
                }
            }
        }
        public async Task CloseConnection(string connectionID)
        {
            if (_connections.TryRemove(connectionID, out var socket) && socket != null)
            {
                foreach (var room in _rooms.Values)
                {
                    room.Remove(connectionID);
                }

                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by server", CancellationToken.None);
            }
        }
    }
}
