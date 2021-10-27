import authService from '../components/api-authorization/AuthorizeService';
import { Comment, SaveComment } from '../models/comment';
import { setDisplayName } from '../models/user';
import { GlobalDataService } from './globalData.service';

export class CommentService {
    private globalDataService = new GlobalDataService();

    async getReplies(commentId: number) {
        let commentApiEndpoint = await this.globalDataService.getCommentApiEndpoint();
        if (!commentApiEndpoint) return [];

        return fetch(commentApiEndpoint + "/" + commentId)
            .then(response => response.json())
            .then(data => {
                let comments = data as Comment[];
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

        let commentApiEndpoint = await this.globalDataService.getCommentApiEndpoint();
        if (!commentApiEndpoint) return null;

        return fetch(commentApiEndpoint, {
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
                let comment = data as Comment;
                if (comment) {
                    setDisplayName(comment.author);
                }
                return comment;
            });
    }

    async save(comment: SaveComment) {
        if (!comment.id || comment.content === "") return null;

        const token = await authService.getAccessToken();
        let formData = new FormData();
        formData.append('Id', comment.id.toString() as string);
        formData.append('Content', comment.content as string);

        let commentApiEndpoint = await this.globalDataService.getCommentApiEndpoint();
        if (!commentApiEndpoint) return null;

        return fetch(commentApiEndpoint + "/" + comment.id, {
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
                let res = data as Comment;
                return res;
            });
    }

    async delete(commentId: number) {
        const token = await authService.getAccessToken();
        let commentApiEndpoint = await this.globalDataService.getCommentApiEndpoint();
        if (!commentApiEndpoint) return null;

        return fetch(commentApiEndpoint + '/' + commentId, {
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