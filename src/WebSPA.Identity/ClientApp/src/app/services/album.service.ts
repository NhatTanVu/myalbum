import { Album, SaveAlbum, AlbumQuery } from './../models/album';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { setDisplayName } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class AlbumService {
  private readonly albumApiEndpoint = "https://localhost:5003/api/albums";

  private readonly httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    })
  };

  constructor(private http: HttpClient) { }

  create(album: SaveAlbum) {
    var formData = new FormData();
    formData.append('Id', album.id.toString());
    formData.append('Name', album.name);

    return this.http.post(this.albumApiEndpoint, formData)
      .pipe(map(res => <SaveAlbum>res));
  }

  getAll(filter: AlbumQuery) {
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

    return this.http.post(this.albumApiEndpoint + '/' + album.id, formData)
      .pipe(map(res => <SaveAlbum>res));
  }

  get(id) {
    return this.http.get(this.albumApiEndpoint + '/' + id, this.httpOptions)
      .pipe(map(res => {
        var album = <Album>res;
        return album;
      }));
  }

  delete(id) {
    return this.http.delete(this.albumApiEndpoint + '/' + id, this.httpOptions)
      .pipe(map(res => {
        return res;
      }));
  }
}
