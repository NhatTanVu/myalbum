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

export interface User {
    userName: string;
    firstName: string;
    lastName: string;
    displayName: string;
}

export function setDisplayName(user: User)
{
  if (user.firstName && user.lastName)
    user.displayName = user.firstName + " " + user.lastName;
  else if (user.firstName)
    user.displayName = user.firstName;
  else if (user.lastName)
    user.displayName = user.lastName;
  else
    user.displayName = user.userName;
}