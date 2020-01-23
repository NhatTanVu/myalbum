import { SavePhoto } from './../models/SavePhoto';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, switchMap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class PhotoService {

  constructor(private http: HttpClient) { }

  getAll() {
    return this.http.get('/api/photos')
      .pipe(map(res => <Object[]>res));
  }

  create(photo: SavePhoto, file) {
    var formData = new FormData();
    formData.append('FileToUpload', file);
    formData.append('Id', photo.id.toString());
    formData.append('Name', photo.name);

    return this.http.post('/api/photo', formData).pipe(map(res => <SavePhoto>res));
  }
}
