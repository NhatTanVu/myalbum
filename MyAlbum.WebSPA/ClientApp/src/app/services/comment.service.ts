import { CommentAdded } from './../models/commentAdded';
import { Comment } from '../models/comment';
import { setDisplayName } from '../models/user';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { map, catchError } from 'rxjs/operators';
import * as signalR from "@aspnet/signalr";
import { Observable, Subscriber, throwError } from 'rxjs';
import { Router } from '@angular/router';
import { ApplicationPaths, QueryParameterNames } from 'src/api-authorization/api-authorization.constants';

@Injectable({
  providedIn: 'root'
})
export class CommentService {
  private readonly commentApiEndpoint = "/api/comments";

  private readonly httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    })
  };

  private hubConnection: signalR.HubConnection;

  constructor(private http: HttpClient, private router: Router) {
    this.startConnection();
    this.addCommentAddedListener();
  }

  private startConnection = () => {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('/commentHub')
      .build();

    this.hubConnection
      .start()
      .then(() => {
        this.hubConnection.invoke("GetConnectionId").then((connectionId) => {
          console.log("ConnectionId: " + connectionId);
          this.connectionId = connectionId;
        });
        console.log('Connection started');
      })
      .catch(err => console.log('Error while starting connection: ' + err))
  }

  public addedComment$: Observable<CommentAdded> = new Observable<CommentAdded>(e => this.emitter = e);
  private emitter: Subscriber<CommentAdded>;
  private connectionId: string;
  private addCommentAddedListener = () => {
    this.hubConnection.on('commentAdded', (result) => {
      let comment = <Comment>result;
      if (comment.connectionId != this.connectionId) {
        let temp = comment;
        while (temp.replies && temp.replies.length > 0) {
          setDisplayName(temp.author);
          temp.isViewing = true;
          temp.areRepliesLoaded = true;
          let index = 0;
          for (var i = 0; i < temp.replies.length; i++) {
            let c = temp.replies[i];
            setDisplayName(c.author);
            if (c.connectionId == temp.connectionId)
              index = i;
          }
          temp = temp.replies[index];
        }
        temp.isNew = true;
        setDisplayName(temp.author);
        temp.isViewing = true;
        temp.areRepliesLoaded = true;

        let output: CommentAdded = {
          id: temp.id,
          ancestorOrSelf: comment
        };
        this.emitter.next(output);
      }
      //console.log('commentAdded - ' + JSON.stringify(comment));
    });
  }

  create(comment: Comment) {
    comment.connectionId = this.connectionId;
    return this.http.post(this.commentApiEndpoint, comment, this.httpOptions)
      .pipe(map(res => {
        var comment = <Comment>res;
        setDisplayName(comment.author);
        return comment;
      }), catchError((error: HttpErrorResponse) => {
        let errorMessage = 'Unknown error!';
        if (error.error instanceof ErrorEvent) {
          // Client-side errors
          errorMessage = `Error: ${error.error.message}`;
        } else {
          // Server-side errors
          if (error.status == 401) {
            this.router.navigate(ApplicationPaths.LoginPathComponents, {
              queryParams: {
                [QueryParameterNames.ReturnUrl]: this.router.url
              }
            });
          }
          errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
        }
        return throwError(errorMessage);
      }));
  }

  getReplies(commentId: number) {
    return this.http.get(this.commentApiEndpoint + "/" + commentId, this.httpOptions)
      .pipe(map(res => {
        var comments = <Comment[]>res;
        comments.forEach((comment) => {
          setDisplayName(comment.author);
          comment.replies = [];
        });
        return comments;
      }));
  }
}
