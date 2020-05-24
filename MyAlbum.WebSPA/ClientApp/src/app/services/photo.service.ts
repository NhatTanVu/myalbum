import { SavePhoto, Photo } from '../models/photo';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map, catchError } from 'rxjs/operators';
import { retryWithBackoff } from './retryWithBackoff.operator';
import { EMPTY } from 'rxjs';
import { setDisplayName } from '../models/comment';
import { LoadingBarService } from '@ngx-loading-bar/core';

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

  constructor(private http: HttpClient, private loadingBar: LoadingBarService) { }

  getAll(filter) {
    this.loadingBar.start();
    return this.http.get(this.photosEndpoint + '?' + this.toQueryString(filter), this.httpOptions)
      .pipe(
        retryWithBackoff(1000, 5, 10000),
        catchError(error => {
          this.loadingBar.stop();
          console.error(error);
          return EMPTY;
        }),
        map(res => {
          this.loadingBar.stop();
          return <Photo[]>res;
        }));
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
    formData.append('LocLat', photo.locLat ? photo.locLat.toString() : null);
    formData.append('LocLng', photo.locLng ? photo.locLng.toString() : null);
    formData.append('CenterLat', photo.centerLat ? photo.centerLat.toString() : null);
    formData.append('CenterLng', photo.centerLng ? photo.centerLng.toString() : null);    
    formData.append('MapZoom', photo.mapZoom ? photo.mapZoom.toString() : null);

    return this.http.post(this.photosEndpoint, formData)
      .pipe(map(res => <SavePhoto>res));
  }

  getPhoto(id) {
    return this.http.get(this.photosEndpoint + '/' + id, this.httpOptions)
      .pipe(map(res => {
        var photo = <Photo>res;
        photo.comments.forEach(comment => {
          setDisplayName(comment.author);
        });
        return photo;
      }));
  }
}
