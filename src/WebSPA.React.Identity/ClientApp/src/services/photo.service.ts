import { Photo } from '../models/photo';
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
}