export class GlobalData {
  displayMode: DisplayMode | null = null;
  enableDisplayMode: boolean = true;
}

export class GlobalConfiguration {
  PhotoUrl: string | null = null;
  AlbumUrl: string | null = null;
  CommentUrl: string | null = null;
  GoogleAPIKey: string = "";
}

export const MAX_FILE_LENGTH: number = 1024*1024;

export enum DisplayMode {
  Photo = "photo",
  Album = "album"
}
