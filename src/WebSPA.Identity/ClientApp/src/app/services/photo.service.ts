import { SavePhoto, Photo } from '../models/photo';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map, catchError } from 'rxjs/operators';
import { retryWithBackoff } from './retryWithBackoff.operator';
import { EMPTY } from 'rxjs';
import { EmptyObservable } from "rxjs/observable/EmptyObservable";
import { setDisplayName } from '../models/user';
import { LoadingBarService } from '@ngx-loading-bar/core';
import { GlobalDataService } from './globalData.service';

@Injectable({
  providedIn: 'root'
})
export class PhotoService {
  private photoApiEndpoint: string = "";

  private readonly httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    })
  };

  constructor(private http: HttpClient, private loadingBar: LoadingBarService, private globalDataService: GlobalDataService) {
  }

  getAll(filter) {
    this.loadingBar.start();

    return this.globalDataService.currentGlobalConfiguration$.pipe(
      map(globalConfiguration => {
        if (globalConfiguration.PhotoUrl) {
          this.photoApiEndpoint = globalConfiguration.PhotoUrl + "/api/photos";
          return this.http.get(this.photoApiEndpoint + '?' + this.toQueryString(filter), this.httpOptions)
            .pipe(
              retryWithBackoff(1000, 5, 10000),
              catchError(error => {
                this.loadingBar.stop();
                console.error(error);
                return EMPTY;
              }),
              map(res => {
                this.loadingBar.stop();
                var photos = <Photo[]>res;
                photos.forEach(photo => {
                  setDisplayName(photo.author);
                });
                return photos;
              }));
        }
        else {
          return new EmptyObservable<Photo[]>();
        }
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
    formData.append('Album.Id', (photo.album && photo.album.id > 0) ? photo.album.id.toString() : null);

    return this.globalDataService.currentGlobalConfiguration$.pipe(
      map(globalConfiguration => {
        if (globalConfiguration.PhotoUrl) {
          this.photoApiEndpoint = globalConfiguration.PhotoUrl + "/api/photos";
          return this.http.post(this.photoApiEndpoint, formData)
            .pipe(map(res => <SavePhoto>res))
        }
        else {
          return new EmptyObservable<SavePhoto>();
        }
      }));
  }

  save(photo: SavePhoto, file) {
    var formData = new FormData();
    formData.append('FileToUpload', file);
    formData.append('Id', photo.id.toString());
    formData.append('Name', photo.name);
    formData.append('LocLat', photo.locLat ? photo.locLat.toString() : null);
    formData.append('LocLng', photo.locLng ? photo.locLng.toString() : null);
    formData.append('CenterLat', photo.centerLat ? photo.centerLat.toString() : null);
    formData.append('CenterLng', photo.centerLng ? photo.centerLng.toString() : null);
    formData.append('MapZoom', photo.mapZoom ? photo.mapZoom.toString() : null);
    formData.append('PhotoCategories', photo.photoCategories ? JSON.stringify(photo.photoCategories) : null);
    formData.append('Album.Id', (photo.album && photo.album.id > 0) ? photo.album.id.toString() : null);

    return this.globalDataService.currentGlobalConfiguration$.pipe(
      map(globalConfiguration => {
        if (globalConfiguration.PhotoUrl) {
          this.photoApiEndpoint = globalConfiguration.PhotoUrl + "/api/photos";
          return this.http.post(this.photoApiEndpoint + '/' + photo.id, formData)
            .pipe(map(res => <Photo>res))
        }
        else {
          return new EmptyObservable<Photo>();
        }
      }));
  }

  get(id) {
    return this.globalDataService.currentGlobalConfiguration$.pipe(
      map(globalConfiguration => {
        if (globalConfiguration.PhotoUrl) {
          this.photoApiEndpoint = globalConfiguration.PhotoUrl + "/api/photos";
          return this.http.get(this.photoApiEndpoint + '/' + id, this.httpOptions)
            .pipe(map(res => {
              var photo = <Photo>res;
              photo.comments.forEach(comment => {
                setDisplayName(comment.author);
                comment.isNew = false;
              });
              return photo;
            }));
        }
        else {
          return new EmptyObservable<Photo>();
        }
      }));
  }

  delete(id) {
    return this.globalDataService.currentGlobalConfiguration$.pipe(
      map(globalConfiguration => {
        if (globalConfiguration.PhotoUrl) {
          this.photoApiEndpoint = globalConfiguration.PhotoUrl + "/api/photos";
          return this.http.delete(this.photoApiEndpoint + '/' + id, this.httpOptions)
            .pipe(map(res => {
              return res;
            }));
        }
        else {
          return new EmptyObservable<Object>();
        }
      }));
  }
}
