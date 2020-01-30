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

  getAll() {
    return this.http.get(this.photosEndpoint)
      .pipe(map(res => <Photo[]>res));
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
