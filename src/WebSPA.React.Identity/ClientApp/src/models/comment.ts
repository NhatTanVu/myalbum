import { User } from "./user";

export interface Comment {
  id: number;
  photoId: number;
  parentId?: number;
  content: string;
  author: User;
  connectionId: string;
  isNew: boolean;
  isReplying: boolean;
  isViewing: boolean;
  isEditing: boolean;
  areRepliesLoaded: boolean;
  numOfReplies: number;
  replies: Comment[];
  createdDate: Date;
  modifiedDate: Date;
}

export interface SaveComment {
    id?: number;
    content: string;
    photoId: number;
    parentId?: number;
}

export const findReplyInComment = (id: number, comment: Comment): Comment | null => {
  if (!comment || !comment.replies)
    return null;

  let result1 = comment.replies.find(r => r && r.id == id);
  if (result1)
    return result1;
  else {
    for (let i = 0; i < comment.replies.length; i++) {
      let reply = comment.replies[i];
      let result2 = findReplyInComment(id, reply);
      if (result2)
        return result2;
    }
    return null;
  }
}

export const mergeNewComment = (orgComment: Comment, newComment: Comment): Comment => {
  if (orgComment.id == newComment.id) {
    let isNew = orgComment.isNew;
    let isReplying = orgComment.isReplying;
    let isViewing = orgComment.isViewing;
    let isEditing = orgComment.isEditing;
    let areRepliesLoaded = orgComment.areRepliesLoaded;
    let numOfReplies = orgComment.numOfReplies;
    let replies = orgComment.replies.slice();
    if (newComment.isNew)
      isNew = true;
    if (newComment.isViewing)
      isViewing = true;

    orgComment = newComment;

    orgComment.isNew = isNew;
    orgComment.isReplying = isReplying;
    orgComment.isViewing = isViewing;
    orgComment.isEditing = isEditing;

    if (areRepliesLoaded) {
      if (newComment.areRepliesLoaded) {
        orgComment.replies.forEach(newReply => {
          let orgReply = replies.find(r => r.id == newReply.id);
          if (orgReply)
            newReply = mergeNewComment(orgReply, newReply);
        });
      }
      else {
        orgComment.numOfReplies = numOfReplies;
        orgComment.replies = replies;
      }
    }
  }

  return orgComment;
}