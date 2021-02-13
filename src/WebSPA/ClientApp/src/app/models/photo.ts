import { SaveAlbum } from './album';
import { Comment } from './comment';
import { User } from './user';

export interface PhotoCategory {
    id: number;
    name: string;
}

export interface Photo {
    id: number;
    name: string;
    filePath: string;
    boundingBoxFilePath: string;
    width: number;
    height: number;
    photoCategories: PhotoCategory[];
    locLat: number;
    locLng: number;
    centerLat: number;
    centerLng: number;
    mapZoom: number;
    comments: Comment[];
    totalComments: number;
    createdDate: Date;
    modifiedDate: Date;
    author: User;
    album: SaveAlbum;
}

export interface SavePhoto {
    id: number;
    name: string;
    locLat: number;
    locLng: number;
    centerLat: number;
    centerLng: number;
    mapZoom: number;
    photoCategories: PhotoCategory[];
    album: SaveAlbum;
}

export interface PositionModel {
    latitude: number;
    longitude: number;
}