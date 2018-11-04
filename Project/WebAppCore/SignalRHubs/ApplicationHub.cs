using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DomainModel.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using ServiceInterface;

namespace WebAppCore.SignalRHubs
{
    public class ApplicationHub : Hub
    {
        private readonly IHttpContextAccessor _IHttpContextAccessor;
        private readonly IAppAnalyticsService _IAppAnalyticsService;

        public ApplicationHub(IHttpContextAccessor httpContextAccessor, IAppAnalyticsService iAppAnalyticsService)
        {
            _IHttpContextAccessor = httpContextAccessor;
            _IAppAnalyticsService = iAppAnalyticsService;
        }

        public class UserHubModels
        {
            public string UserName { get; set; }
            public Int64 UserId { get; set; }
            public HashSet<string> ConnectionIds { get; set; }
        }

        public class ActiveSessions
        {
            public string ConnectionId { get; set; }
            public string IpAddress { get; set; }
            public DateTime ConnectedOn { get; set; }
            public string UserName { get; set; }
            public Int64 UserId { get; set; }
            public Guid CookieUniqueId { get; set; }
        }

        private static readonly ConcurrentDictionary<string, UserHubModels> Users =
new ConcurrentDictionary<string, UserHubModels>(StringComparer.InvariantCultureIgnoreCase);

        public static class ActiveConnections
        {
            public static List<string> ActiveUsers = new List<string>();
            public static List<string> ActiveSessions = new List<string>();
            public static List<ActiveSessions> ActiveSessionsList = new List<ActiveSessions>();
        }

        public Task SendMessageToGroups(string message)
        {
            List<string> groups = new List<string>() { "SignalR Users" };
            return Clients.Groups(groups).SendAsync("ReceiveMessage", message);
        }

        public override async Task OnConnectedAsync()
        {
            // var serverVars = Context.GetHttpContext().Request.
            var ipAddress = this._IHttpContextAccessor
                                                .HttpContext.Connection.RemoteIpAddress
                                                .ToString();

            string userName = "";
            Int64 userId = 0;
            Guid cookieUniqueId;
            string connectionId = Context.ConnectionId;
            if (Context.User.Identity.Name != null)
            {
                userName = Context.User.Claims.Where(w => w.Value == "UserName").FirstOrDefault().ValueType;
                userId = Convert.ToInt32(Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                cookieUniqueId = new Guid(Context.User.Claims.Where(w => w.Value == "CookieUniqueId").FirstOrDefault().ValueType);
            }
            else
            {
                cookieUniqueId = Guid.NewGuid();
            }
            ActiveConnections.ActiveUsers.Add(userName);
            ActiveConnections.ActiveSessions.Add(Context.ConnectionId);
            ActiveConnections.ActiveSessionsList.Add(new ActiveSessions
            {
                UserName = userName,
                UserId = userId,
                ConnectionId = Context.ConnectionId,
                IpAddress = ipAddress,
                ConnectedOn = DateTime.Now,
                CookieUniqueId = cookieUniqueId
            });

            await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string userName = Context.User.Identity.Name;
            string connectionId = Context.ConnectionId;
            ActiveConnections.ActiveUsers.Remove(userName);
            ActiveConnections.ActiveSessions.Remove(Context.ConnectionId);

            ActiveSessions activeSessions = new ActiveSessions();
            activeSessions = ActiveConnections.ActiveSessionsList.Where(x => x.ConnectionId == Context.ConnectionId).FirstOrDefault();
            ActiveConnections.ActiveSessionsList.Remove(activeSessions);

            if (!ActiveConnections.ActiveSessionsList.Any(w => w.UserId == activeSessions.UserId)
                    && activeSessions.UserId > 0)
            {
                IpPropertiesModal ipAddressDetails = new IpPropertiesModal();
                ipAddressDetails.UserId = activeSessions.UserId;
                ipAddressDetails.IpAddress = activeSessions.IpAddress;
                ipAddressDetails.SessionDisconnectedOn = DateTime.Now;
                ipAddressDetails.CookieUniqueId = activeSessions.CookieUniqueId;
                _IAppAnalyticsService.UpdatedUserDisConnectionTracking(ipAddressDetails);
            }

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}