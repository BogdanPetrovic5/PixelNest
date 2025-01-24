using PixelNestBackend.Dto.WebSockets;
using PixelNestBackend.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Reflection;
using System.Text;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PixelNestBackend.Services.Menagers
{
    public class WebSocketConnectionMenager
    {
        private readonly ConcurrentDictionary<string, List<string>> _rooms = new();
        private readonly ConcurrentDictionary<string, WebSocket> _connections = new();
        private readonly ConcurrentDictionary<string, bool> _userStatuses = new();

        public async Task AddSocket(WebSocket socket, string userID)
        {
            _connections.TryAdd(userID, socket);
            _userStatuses[userID] = true;
            var activeUsers = _userStatuses.Where(u => u.Value && !u.Key.Equals(userID)).Select(u => new {
                username = u.Key,
                isActive = u.Value,
            }).ToList();
            var activeUsersMessage = new
            {
                Type = "ActiveUsers",
                Users = activeUsers
            };
            string messageJson = JsonSerializer.Serialize(activeUsersMessage);

            if (socket.State == WebSocketState.Open)
            {
                var buffer = Encoding.UTF8.GetBytes(messageJson);
                await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }

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
        public async Task NotifyUsers(string userID, bool isActive)
        {
            var statusMessage = new
            {
                UserID = userID,
                IsActive = isActive,
                Type = "Status"
            };
            string messageJson = JsonSerializer.Serialize(statusMessage);

            foreach (var connection in _connections.Values)
            {
                if (connection.State == WebSocketState.Open && !connection.Equals(userID))
                {
                    var buffer = Encoding.UTF8.GetBytes(messageJson);
                    await connection.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }
        public async Task SendMessageToUser(string receiver, string sender, string message)
        {
           

            string actualRoomID = FindRoom(receiver, sender);
            WebSocketMessage webSocketMessage = new WebSocketMessage
            {
                SenderUsername = sender,
                TargetUser = receiver,
                RoomID = actualRoomID,
                Content = message
            };
           
            if (IsUserInRoom(actualRoomID, receiver)) 
            {
                Console.Write("Ovde");
                webSocketMessage.Type = "Room";
                string jsonMessage = System.Text.Json.JsonSerializer.Serialize(webSocketMessage);
                await _sendMessageToRoom(actualRoomID, receiver, jsonMessage);
                
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
        public bool IsUserInRoom(string roomID, string receiver)
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
        public string FindRoom(string receiver, string sender)
        {
            string roomID = $"{sender}-{receiver}";
            string reversedRoomID = $"{receiver}-{sender}";

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
                _userStatuses[connectionID] = false;
                if (_userStatuses.ContainsKey(connectionID))
                {
                    _userStatuses.TryRemove(connectionID, out bool status);
                }
                await NotifyUsers(connectionID, false);
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by server", CancellationToken.None);
            }
        }
    }
}
