export class GlobalData {
    displayMode: DisplayMode | null = null;
    enableDisplayMode: boolean = true;
}

export class GlobalConfiguration {
    IssuerUri: string | null = null;
    PhotoApiUrl: string | null = null;
    AlbumApiUrl: string | null = null;
    CommentApiUrl: string | null = null;
    GoogleApiKey: string = "";
}

export const MAX_FILE_LENGTH: number = 1024 * 1024;

export enum DisplayMode {
    Photo = "photo",
    Album = "album"
}
