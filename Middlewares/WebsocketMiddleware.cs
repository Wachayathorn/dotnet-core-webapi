using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace webapi.Middleware
{
    public class WebsocketMiddleware
    {
        private readonly RequestDelegate next;
        public WebsocketMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path == "/ws")
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    using (WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync())
                    {
                        await Echo(context, webSocket);
                    }
                }
                else
                {
                    context.Response.StatusCode = (int)StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync("Request is invalid.");
                }
            }
            else
            {
                await next(context);
            }
        }

        private async Task Echo(HttpContext context, WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                var serverMsg = Encoding.UTF8.GetBytes($"Server: Hello. You said: {Encoding.UTF8.GetString(buffer)}");
                await webSocket.SendAsync(new ArraySegment<byte>(serverMsg, 0, serverMsg.Length), result.MessageType, result.EndOfMessage, CancellationToken.None);
            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
    }
}