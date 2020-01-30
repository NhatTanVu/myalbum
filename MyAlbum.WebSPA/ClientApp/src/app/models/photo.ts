export interface Photo {
    id: number;
    name: string;
    filePath: string;
    width: number;
    height: number;
    photoCategories: string[];
}

export interface SavePhoto {
    id: number;
    name: string;
}