using Microsoft.AspNetCore.SignalR;

namespace MyAlbum.Services.Comment.API.Hubs
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