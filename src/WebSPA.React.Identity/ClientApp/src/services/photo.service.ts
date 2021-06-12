import { Photo } from '../models/photo';
import { setDisplayName } from '../models/user';

export class PhotoService {
    private photoApiEndpoint: string = "https://localhost:5002/api/photos";

    toQueryString(obj: any) {
        var parts = [];
        for (var prop in obj) {
            var value = obj[prop];
            if (value != null && value != undefined)
                parts.push(encodeURIComponent(prop) + "=" + encodeURIComponent(value));
        }
        return parts.join("&");
    }

    getAll(filter: any) {
        return fetch(this.photoApiEndpoint + '?' + this.toQueryString(filter))
            .then(response => response.json())
            .then(data => {
                var photos = <Photo[]>data;
                photos.forEach(photo => {
                    setDisplayName(photo.author);
                });
                return photos;
            });
    }
}