import { Comment } from './comment';

export interface CommentAdded {
    id: number;
    ancestorOrSelf: Comment;
}