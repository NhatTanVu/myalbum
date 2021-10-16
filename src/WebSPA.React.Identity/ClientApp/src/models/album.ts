import { Photo } from './photo';
import { User } from "./user";

export interface AlbumQuery {
    categoryId: number;
    hasLocation: boolean;
    authorUserName: string;
}

export class Album {
    id: number = 0;
    name: string | null = null;
    createdDate: Date | null = null;
    modifiedDate: Date | null = null;
    author: User | null = null;
    mainPhoto: Photo | null = null;
    subPhotos: (Photo | null)[] = [];
    photos: Photo[] = [];
}

export class SaveAlbum {
    id?: number;
    name?: string;
}