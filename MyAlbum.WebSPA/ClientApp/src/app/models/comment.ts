import { User } from "./user";

export interface Comment {
  id: number;
  photoId: number;
  parentId: number;
  content: string;
  author: User;
  connectionId: string;
  isNew: boolean;
  isReplying: boolean;
  isViewing: boolean;
  areRepliesLoaded: boolean;
  numOfReplies: number;
  replies: Comment[];
  createdDate: Date;
  modifiedDate: Date;
}

export const findReplyInComment = (id: number, comment: Comment): Comment => {
  if (!comment || !comment.replies)
    return null;

  var result1 = comment.replies.find(r => r && r.id == id);
  if (result1)
    return result1;
  else {
    for (var i = 0; i < comment.replies.length; i++) {
      var reply = comment.replies[i];
      var result2 = findReplyInComment(id, reply);
      if (result2)
        return result2;
    }
    return null;
  }
}

export const mergeNewComment = (orgComment: Comment, newComment: Comment): Comment => {
  if (orgComment.id == newComment.id) {
    var isNew = orgComment.isNew;
    var isReplying = orgComment.isReplying;
    var isViewing = orgComment.isViewing;
    var areRepliesLoaded = orgComment.areRepliesLoaded;
    var numOfReplies = orgComment.numOfReplies;
    var replies = orgComment.replies.slice();
    if (newComment.isNew)
      isNew = true;
    if (newComment.isViewing)
      isViewing = true;

    orgComment = newComment;

    orgComment.isNew = isNew;
    orgComment.isReplying = isReplying;
    orgComment.isViewing = isViewing;

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