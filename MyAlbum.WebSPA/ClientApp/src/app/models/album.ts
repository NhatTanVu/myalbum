import { User } from "./user";

export interface Album {
    id: number;
    name: string;
    createdDate: Date;
    modifiedDate: Date;
    author: User;    
}