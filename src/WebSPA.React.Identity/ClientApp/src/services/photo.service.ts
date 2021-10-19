import authService from '../components/api-authorization/AuthorizeService';
import { Photo, SavePhoto } from '../models/photo';
import { setDisplayName } from '../models/user';

export class PhotoService {
    private photoApiEndpoint: string = "https://localhost:5002/api/photos";

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
        return fetch(this.photoApiEndpoint + '?' + this.toQueryString(filter))
            .then(response => response.json())
            .then(data => {
                var photos = data as Photo[];
                photos.forEach(photo => {
                    setDisplayName(photo.author);
                });
                return photos;
            });
    }

    get(id: number) {
        return fetch(this.photoApiEndpoint + '/' + id)
            .then(response => {
                if (response.ok) {
                    return response.json()
                } else {
                    return null;
                }
            })
            .then(data => {
                var photo = data as Photo;
                photo?.comments.forEach(comment => {
                    setDisplayName(comment.author);
                    comment.isNew = false;
                });
                return photo;
            });
    }

    async create(photo: SavePhoto, file: any) {
        if (!file || photo.name === "") return null;

        var formData = new FormData();
        formData.append('Name', photo.name as string);
        formData.append('FileToUpload', file);
        photo.locLat && formData.append('LocLat', photo.locLat?.toString() as string);
        photo.locLng && formData.append('LocLng', photo.locLng?.toString() as string);
        photo.centerLat && formData.append('CenterLat', photo.centerLat?.toString() as string);
        photo.centerLng && formData.append('CenterLng', photo.centerLng?.toString() as string);
        photo.mapZoom && formData.append('MapZoom', photo.mapZoom?.toString() as string);
        photo.album?.id && formData.append('Album.Id', photo.album?.id?.toString() as string);
        const token = await authService.getAccessToken();

        return fetch(this.photoApiEndpoint, {
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
            var photo = data as SavePhoto;
            return photo;
        });
    }

    async save(photo: Photo, file: any) {
        if (!photo.id || photo.name === "") return null;

        var formData = new FormData();
        formData.append('FileToUpload', file);
        formData.append('Id', photo.id?.toString() as string);
        formData.append('Name', photo.name as string);
        formData.append('LocLat', photo.locLat?.toString() as string);
        formData.append('LocLng', photo.locLng?.toString() as string);
        formData.append('CenterLat', photo.centerLat?.toString() as string);
        formData.append('CenterLng', photo.centerLng?.toString() as string);
        formData.append('MapZoom', photo.mapZoom?.toString() as string);
        formData.append('PhotoCategories', JSON.stringify(photo.photoCategories));
        if (photo.album && photo.album.id && photo.album.id > 0)
            formData.append('Album.Id', photo.album.id.toString());
        const token = await authService.getAccessToken();

        return fetch(this.photoApiEndpoint + '/' + photo.id, {
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
                var photo = data as SavePhoto;
                return photo;
            });
    }

    async delete(id: number) {
        const token = await authService.getAccessToken();

        return fetch(this.photoApiEndpoint + '/' + id, {
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