import { Comment } from '../models/comment';
import { setDisplayName } from '../models/user';

export class CommentService {
    private commentApiEndpoint: string = "https://localhost:5004/api/comments";

    toQueryString(obj: any) {
        var parts = [];
        for (var prop in obj) {
            var value = obj[prop];
            if (value !== null && value !== undefined)
                parts.push(encodeURIComponent(prop) + "=" + encodeURIComponent(value));
        }
        return parts.join("&");
    }

    getReplies(commentId: number) {
        return fetch(this.commentApiEndpoint + "/" + commentId)
            .then(response => response.json())
            .then(data => {
                var comments = data as Comment[];
                comments.forEach((comment) => {
                    setDisplayName(comment.author);
                    comment.replies = [];
                });
                return comments;
            });
    }
}