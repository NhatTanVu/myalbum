import { Comment } from './comment';

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
    createdDate: Date;
    modifiedDate: Date;
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
}

export interface PositionModel {
    latitude: number;
    longitude: number;
}