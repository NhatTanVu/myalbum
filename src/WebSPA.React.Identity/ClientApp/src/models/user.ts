export class User {
    userName: string = "";
    firstName: string = "";
    lastName: string = "";
    displayName: string = "";
    createdDate: Date = new Date();
    modifiedDate: Date = new Date();
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