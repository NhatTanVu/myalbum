import { Comment, User, setDisplayName } from '../models/comment';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map, catchError } from 'rxjs/operators';
import { retryWithBackoff } from './retryWithBackoff.operator';
import { EMPTY } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CommentService {
  private readonly commentsEndpoint = "/api/comments";
  
  private readonly httpOptions = {
    headers: new HttpHeaders({
      'Content-Type':  'application/json',
      'Accept': 'application/json'
    })
  };

  constructor(private http: HttpClient) { }

  create(comment: Comment) {
    var formData = new FormData();

    return this.http.post(this.commentsEndpoint, comment, this.httpOptions)
      .pipe(map(res => {
        var comment = <Comment>res;
        setDisplayName(comment.author);
        return comment;
      }));
  }
}
