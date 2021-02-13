import { Photo } from './photo';
import { User } from "./user";

export interface AlbumQuery {
    categoryId: number;
    hasLocation: boolean;
    authorUserName: string;
}

export class Album {
    id: number = 0;
    name: string = null;
    createdDate: Date = null;
    modifiedDate: Date = null;
    author: User = null;
    mainPhoto: Photo = null;
    subPhotos: Photo[] = [];
    photos: Photo[] = [];
}

export interface SaveAlbum {
    id: number;
    name: string;
}