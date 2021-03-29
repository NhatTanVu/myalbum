import { Album, SaveAlbum, AlbumQuery } from './../models/album';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { setDisplayName } from '../models/user';
import { GlobalDataService } from './globalData.service';
import { EmptyObservable } from 'rxjs/observable/EmptyObservable';

@Injectable({
  providedIn: 'root'
})
export class AlbumService {
  private albumApiEndpoint = "";

  private readonly httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    })
  };

  constructor(private http: HttpClient, private globalDataService: GlobalDataService) {
  }

  create(album: SaveAlbum) {
    var formData = new FormData();
    formData.append('Id', album.id.toString());
    formData.append('Name', album.name);

    return this.globalDataService.currentGlobalConfiguration$.pipe(
      map(globalConfiguration => {
        if (globalConfiguration.AlbumUrl) {
          this.albumApiEndpoint = globalConfiguration.AlbumUrl + "/api/albums";
          return this.http.post(this.albumApiEndpoint, formData)
            .pipe(map(res => <SaveAlbum>res));
        }
        else {
          return new EmptyObservable<SaveAlbum>();
        }
      }));
  }

  getAll(filter: AlbumQuery) {
    return this.globalDataService.currentGlobalConfiguration$.pipe(
      map(globalConfiguration => {
        if (globalConfiguration.AlbumUrl) {
          this.albumApiEndpoint = globalConfiguration.AlbumUrl + "/api/albums";
          return this.http.get(this.albumApiEndpoint + '?' + this.toQueryString(filter), this.httpOptions)
            .pipe(
              map(res => {
                var albums = <Album[]>res;
                albums.forEach(album => {
                  setDisplayName(album.author);
                  album.mainPhoto = (album.photos.length > 0) ? album.photos[0] : null;
                  album.subPhotos = [];
                  album.subPhotos.push((album.photos.length > 1) ? album.photos[1] : null);
                  album.subPhotos.push((album.photos.length > 2) ? album.photos[2] : null);
                  album.subPhotos.push((album.photos.length > 3) ? album.photos[3] : null);
                  album.subPhotos.push((album.photos.length > 4) ? album.photos[4] : null);
                });
                return albums;
              }));
        }
        else {
          return new EmptyObservable<Album[]>();
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

  save(album: SaveAlbum) {
    var formData = new FormData();
    formData.append('Id', album.id.toString());
    formData.append('Name', album.name);

    return this.globalDataService.currentGlobalConfiguration$.pipe(
      map(globalConfiguration => {
        if (globalConfiguration.AlbumUrl) {
          this.albumApiEndpoint = globalConfiguration.AlbumUrl + "/api/albums";
          return this.http.post(this.albumApiEndpoint + '/' + album.id, formData)
            .pipe(map(res => <SaveAlbum>res));
        }
        else {
          return new EmptyObservable<SaveAlbum>();
        }
      }));
  }

  get(id) {
    return this.globalDataService.currentGlobalConfiguration$.pipe(
      map(globalConfiguration => {
        if (globalConfiguration.AlbumUrl) {
          this.albumApiEndpoint = globalConfiguration.AlbumUrl + "/api/albums";
          return this.http.get(this.albumApiEndpoint + '/' + id, this.httpOptions)
            .pipe(map(res => {
              var album = <Album>res;
              return album;
            }));
        }
        else {
          return new EmptyObservable<Album>();
        }
      }));
  }

  delete(id) {
    return this.globalDataService.currentGlobalConfiguration$.pipe(
      map(globalConfiguration => {
        if (globalConfiguration.AlbumUrl) {
          this.albumApiEndpoint = globalConfiguration.AlbumUrl + "/api/albums";
          return this.http.delete(this.albumApiEndpoint + '/' + id, this.httpOptions)
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
