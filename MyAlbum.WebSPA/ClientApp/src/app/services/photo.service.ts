import { SavePhoto, Photo } from '../models/photo';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, switchMap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class PhotoService {
  private readonly photosEndpoint = "/api/photos";
  constructor(private http: HttpClient) { }

  getAll(filter) {
    return this.http.get(this.photosEndpoint + '?' + this.toQueryString(filter))
      .pipe(map(res => <Photo[]>res));
  }

  toQueryString(obj) {
    var parts = [];
    for (var prop in obj) {
      var value = obj[prop];
      if (value != null && value != undefined)
        parts.push(encodeURIComponent(prop) + "=" + encodeURIComponent(value));
    }
    return parts.join("&");
  }  

  create(photo: SavePhoto, file) {
    var formData = new FormData();
    formData.append('FileToUpload', file);
    formData.append('Id', photo.id.toString());
    formData.append('Name', photo.name);

    return this.http.post(this.photosEndpoint, formData).pipe(map(res => <SavePhoto>res));
  }

  getPhoto(id) {
    return this.http.get(this.photosEndpoint + '/' + id)
    .pipe(map(res => <Photo>res));
  }
}
