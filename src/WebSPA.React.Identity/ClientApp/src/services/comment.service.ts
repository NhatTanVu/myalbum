import authService from '../components/api-authorization/AuthorizeService';
import { Comment, SaveComment } from '../models/comment';
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

    async create(comment: SaveComment) {
        const token = await authService.getAccessToken();
        let headers: any = {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        };
        if (token) {
            headers = {
                ...headers,
                'Authorization': `Bearer ${token}`
            }
        }

        return fetch(this.commentApiEndpoint, {
            method: 'POST', // *GET, POST, PUT, DELETE, etc.
            headers: headers,
            body: JSON.stringify(comment)
        })
            .then(response => {
                if (response.ok) {
                    return response.json()
                } else {
                    return null;
                }
            })
            .then(data => {
                var comment = data as Comment;
                if (comment) {
                    setDisplayName(comment.author);
                }
                return comment;
            });
    }

    async save(comment: SaveComment) {
        if (!comment.id || comment.content === "") return null;

        const token = await authService.getAccessToken();
        var formData = new FormData();
        formData.append('Id', comment.id.toString() as string);
        formData.append('Content', comment.content as string);

        return fetch(this.commentApiEndpoint + "/" + comment.id, {
            method: 'POST', // *GET, POST, PUT, DELETE, etc.
            body: formData,
            headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
        })
            .then(response => {
                if (response.ok) {
                    return response.json()
                } else {
                    return null;
                }
            })
            .then(data => {
                var res = data as Comment;
                return res;
            });
    }

    async delete(commentId: number) {
        const token = await authService.getAccessToken();

        return fetch(this.commentApiEndpoint + '/' + commentId, {
            method: 'DELETE', // *GET, POST, PUT, DELETE, etc.
            headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
        })
            .then(response => {
                if (response.ok) {
                    return true;
                } else {
                    return null;
                }
            })
            .then(res => {
                return res;
            });
    }
}