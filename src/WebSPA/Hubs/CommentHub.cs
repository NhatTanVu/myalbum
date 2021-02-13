using Microsoft.AspNetCore.SignalR;
using MyAlbum.WebSPA.Controllers.Resources;
using System.Threading.Tasks;

namespace MyAlbum.WebSPA.Hubs
{
    public class CommentHub: Hub
    {
        // TODO: Only add methods called by clients here
        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }
    }
}