using PixelNestBackend.Services.Menagers;
using System.Net.WebSockets;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Text;
using PixelNestBackend.Services;
using PixelNestBackend.Interfaces;
using PixelNestBackend.Utility;
using PixelNestBackend.Models;

namespace PixelNestBackend.Middleware
{
    public class WebSocketMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly WebSocketConnectionMenager _connectionMenager;


        public WebSocketMiddleware(
            RequestDelegate next,
            WebSocketConnectionMenager connectionMenager

            )
        {
            _next = next;
            _connectionMenager = connectionMenager;

        }


        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (httpContext.WebSockets.IsWebSocketRequest)
            {


                var socket = await httpContext.WebSockets.AcceptWebSocketAsync();
                string username = httpContext.Request.Query["userID"];


                await _connectionMenager.AddSocket(socket, username);
                Console.WriteLine($"WebSocket connected: {username}");
                await _connectionMenager.NotifyUsers(username, true);
                await Receive(socket, username);
            }
            else
            {
                await _next(httpContext);
            }
        }


        public async Task Receive(WebSocket socket, string connectionID)
        {
            var buffer = new byte[1024 * 2];

            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await _connectionMenager.CloseConnection(connectionID);
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);

                }
                else
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);


                    try
                    {
                        var messageObj = System.Text.Json.JsonSerializer.Deserialize<WebSocketMessage>(message);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
        }

        private async Task ProccessMessage(WebSocket socket, string connectionID, string message)
        {
            try
            {
                var messageObj = System.Text.Json.JsonSerializer.Deserialize<WebSocketMessage>(message);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private class WebSocketMessage
        {
            public string Type { get; set; }
            public string RoomID { get; set; }
            public string TargetUser { get; set; }
            public string Content { get; set; }
        }
    }

}