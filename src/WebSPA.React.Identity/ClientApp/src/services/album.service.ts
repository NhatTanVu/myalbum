import { Album, SaveAlbum } from '../models/album';
import { setDisplayName, User } from '../models/user';
import authService from '../components/api-authorization/AuthorizeService';
import { GlobalDataService } from './globalData.service';

export class AlbumService {
    private globalDataService = new GlobalDataService();

    toQueryString(obj: any) {
        let parts = [];
        for (let prop in obj) {
            let value = obj[prop];
            if (value !== null && value !== undefined)
                parts.push(encodeURIComponent(prop) + "=" + encodeURIComponent(value));
        }
        return parts.join("&");
    }

    async getAll(filter: any) {
        let albumApiEndpoint = await this.globalDataService.getAlbumApiEndpoint();
        if (!albumApiEndpoint) return [];

        return fetch(albumApiEndpoint + '?' + this.toQueryString(filter))
            .then(response => response.json())
            .then(data => {
                let albums = data as Album[];
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

    async get(id: number) {
        let albumApiEndpoint = await this.globalDataService.getAlbumApiEndpoint();
        if (!albumApiEndpoint) return null;

        return fetch(albumApiEndpoint + '/' + id)
            .then(response => {
                if (response.ok) {
                    return response.json()
                } else {
                    return null;
                }
            })
            .then(data => {
                let album = data as Album;
                return album;
            });
    }

    async create(album: SaveAlbum) {
        if (album.name === "") return null;

        let formData = new FormData();
        formData.append('Name', album.name as string);
        const token = await authService.getAccessToken();

        let albumApiEndpoint = await this.globalDataService.getAlbumApiEndpoint();
        if (!albumApiEndpoint) return null;

        return fetch(albumApiEndpoint, {
            method: 'POST', // *GET, POST, PUT, DELETE, etc.
            body: formData,
            headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
        })
            .then(response => {
                if (response.ok) {
                    return response.json()
                } else {
                    return null;
                }
            })
            .then(data => {
                let album = data as SaveAlbum;
                return album;
            });
    }

    async save(album: SaveAlbum) {
        if (album.name === "") return null;

        let formData = new FormData();
        formData.append('Id', album.id?.toString() as string);
        formData.append('Name', album.name as string);
        const token = await authService.getAccessToken();

        let albumApiEndpoint = await this.globalDataService.getAlbumApiEndpoint();
        if (!albumApiEndpoint) return null;

        return fetch(albumApiEndpoint + '/' + album.id, {
            method: 'POST', // *GET, POST, PUT, DELETE, etc.
            body: formData,
            headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
        })
            .then(response => {
                if (response.ok) {
                    return response.json()
                } else {
                    return null;
                }
            })
            .then(data => {
                let album = data as SaveAlbum;
                return album;
            });
    }

    async delete(id: number) {
        const token = await authService.getAccessToken();
        let albumApiEndpoint = await this.globalDataService.getAlbumApiEndpoint();
        if (!albumApiEndpoint) return null;

        return fetch(albumApiEndpoint + '/' + id, {
            method: 'DELETE', // *GET, POST, PUT, DELETE, etc.
            headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
        })
            .then(response => {
                if (response.ok) {
                    return true;
                } else {
                    return null;
                }
            })
            .then(data => {
                return data;
            });
    }
}