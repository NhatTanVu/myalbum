export class GlobalData {
  displayMode: DisplayMode;
  enableDisplayMode: boolean = true;
}

export class GlobalConfiguration {
  PhotoUrl: string;
  AlbumUrl: string;
  CommentUrl: string;
}

export const MAX_FILE_LENGTH: number = 1024*1024;

export enum DisplayMode {
  Photo = "photo",
  Album = "album"
}
