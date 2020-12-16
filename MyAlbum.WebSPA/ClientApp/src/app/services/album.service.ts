import { Album } from './../models/album';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { setDisplayName } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class AlbumService {
  private readonly albumApiEndpoint = "/api/albums";

  private readonly httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    })
  };

  constructor(private http: HttpClient) { }

  create(album: Album) {
    var formData = new FormData();
    formData.append('Id', album.id.toString());
    formData.append('Name', album.name);

    return this.http.post(this.albumApiEndpoint, formData)
      .pipe(map(res => <Album>res));
  }

  getAll(filter) {
    return this.http.get(this.albumApiEndpoint + '?' + this.toQueryString(filter), this.httpOptions)
      .pipe(
        map(res => {
          var albums = <Album[]>res;
          albums.forEach(album => {
            setDisplayName(album.author);
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

  save(album: Album) {
    var formData = new FormData();
    formData.append('Id', album.id.toString());
    formData.append('Name', album.name);

    return this.http.post(this.albumApiEndpoint + '/' + album.id, formData)
      .pipe(map(res => <Album>res));
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
