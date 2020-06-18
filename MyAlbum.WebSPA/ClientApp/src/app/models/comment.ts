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
}

export function findReplyInComment(id: number, comment: Comment) {
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

export interface User {
  userName: string;
  firstName: string;
  lastName: string;
  displayName: string;
}

export function setDisplayName(user: User) {
  if (user.firstName && user.lastName)
    user.displayName = user.firstName + " " + user.lastName;
  else if (user.firstName)
    user.displayName = user.firstName;
  else if (user.lastName)
    user.displayName = user.lastName;
  else
    user.displayName = user.userName;
}