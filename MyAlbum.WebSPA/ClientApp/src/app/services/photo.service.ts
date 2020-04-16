import { SavePhoto, Photo } from '../models/photo';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map, catchError } from 'rxjs/operators';
import { retryWithBackoff } from './retryWithBackoff.operator';
import { EMPTY } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PhotoService {
  private readonly photosEndpoint = "/api/photos";
  
  private readonly httpOptions = {
    headers: new HttpHeaders({
      'Content-Type':  'application/json',
      'Accept': 'application/json'
    })
  };

  constructor(private http: HttpClient) { }

  getAll(filter) {
    return this.http.get(this.photosEndpoint + '?' + this.toQueryString(filter), this.httpOptions)
      .pipe(
        retryWithBackoff(1000, 5, 10000),
        catchError(error => {
          console.error(error);
          return EMPTY;
        }),
        map(res => <Photo[]>res));
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

    return this.http.post(this.photosEndpoint, formData, this.httpOptions)
      .pipe(map(res => <SavePhoto>res));
  }

  getPhoto(id) {
    return this.http.get(this.photosEndpoint + '/' + id, this.httpOptions)
      .pipe(map(res => <Photo>res));
  }
}
