﻿import { Album } from '../models/album';
import { setDisplayName, User } from '../models/user';

export class AlbumService {
    private albumApiEndpoint: string = "https://localhost:5003/api/albums";

    toQueryString(obj: any) {
        var parts = [];
        for (var prop in obj) {
            var value = obj[prop];
            if (value !== null && value !== undefined)
                parts.push(encodeURIComponent(prop) + "=" + encodeURIComponent(value));
        }
        return parts.join("&");
    }

    getAll(filter: any) {
        return fetch(this.albumApiEndpoint + '?' + this.toQueryString(filter))
            .then(response => response.json())
            .then(data => {
                var albums = data as Album[];
                albums.forEach(album => {
                    setDisplayName(album.author as User);
                    album.mainPhoto = (album.photos.length > 0) ? album.photos[0] : null;
                    album.subPhotos = [];
                    album.subPhotos.push((album.photos.length > 1) ? album.photos[1] : null);
                    album.subPhotos.push((album.photos.length > 2) ? album.photos[2] : null);
                    album.subPhotos.push((album.photos.length > 3) ? album.photos[3] : null);
                    album.subPhotos.push((album.photos.length > 4) ? album.photos[4] : null);
                });
                return albums;
            });
    }

    get(id: number) {
        return fetch(this.albumApiEndpoint + '/' + id)
            .then(response => {
                if (response.ok) {
                    return response.json()
                } else {
                    return null;
                }
            })
            .then(data => {
                var album = data as Album;
                return album;
            });
    }
}