import { Comment } from './comment';

export interface Photo {
    id: number;
    name: string;
    filePath: string;
    boundingBoxFilePath: string;
    width: number;
    height: number;
    photoCategories: string[];
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
}

export interface PositionModel {
    latitude: number;
    longitude: number;
}