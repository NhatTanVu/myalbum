import { Photo } from './photo';
import { User } from "./user";

export interface AlbumQuery {
    categoryId: number;
    hasLocation: boolean;
    authorUserName: string;
}

export interface Album {
    id: number;
    name: string;
    createdDate: Date;
    modifiedDate: Date;
    author: User;
    mainPhoto: Photo;
    subPhotos: Photo[];
    photos: Photo[];
}

export interface SaveAlbum {
    id: number;
    name: string;
}