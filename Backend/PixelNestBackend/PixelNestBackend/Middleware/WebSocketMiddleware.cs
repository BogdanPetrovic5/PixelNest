using PixelNestBackend.Services.Menagers;
using System.Net.WebSockets;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Text;
using PixelNestBackend.Services;
using PixelNestBackend.Interfaces;
using PixelNestBackend.Utility;
using PixelNestBackend.Models;
using PixelNestBackend.Data;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using PixelNestBackend.Dto.WebSockets;

namespace PixelNestBackend.Middleware
{
    public class WebSocketMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly WebSocketConnectionMenager _connectionMenager;
        private readonly IServiceScopeFactory _serviceFactory;


        public WebSocketMiddleware(
            RequestDelegate next,
            WebSocketConnectionMenager connectionMenager,
           IServiceScopeFactory serviceScopeFactory

            )
        {
            _next = next;
            _connectionMenager = connectionMenager;
            _serviceFactory = serviceScopeFactory;
           
        }


        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (httpContext.WebSockets.IsWebSocketRequest)
            {


                var socket = await httpContext.WebSockets.AcceptWebSocketAsync();
                    
                string? token = httpContext.Request.Cookies["jwtToken"];
                
                string userGuid = _extractUserGuid(token);

                using (var scope = _serviceFactory.CreateScope())
                {
                    var userUtility = scope.ServiceProvider.GetRequiredService<UserUtility>();
                    string clientGuid = userUtility.GetClientGuid(userGuid);

                    await _connectionMenager.AddSocket(socket, clientGuid);
                    
                    await _connectionMenager.NotifyUsers(clientGuid, true);
                    await Receive(socket, clientGuid);
                }

               
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
                        ProccessMessage(messageObj);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
        }

        private async Task ProccessMessage(WebSocketMessage message)
        {
            try
            {
                if(message != null && message.Type == "Typing")
                {
                    _connectionMenager.SendNotificationToUser(message);
                }
                if (message != null && message.Type == "StopTyping")
                {
                    _connectionMenager.SendNotificationToUser(message);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private string _extractUserGuid(string token)
        {

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

                return userIdClaim.Value;
            }
            catch
            {
                return null;
            }
        }
      

    }

}