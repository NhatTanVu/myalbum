import { CommentAdded } from './../models/commentAdded';
import { Comment, User, setDisplayName } from '../models/comment';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';
import * as signalR from "@aspnet/signalr";
import { Observable, Subscriber } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CommentService {
  private readonly commentsEndpoint = "/api/comments";

  private readonly httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    })
  };

  private hubConnection: signalR.HubConnection;

  constructor(private http: HttpClient) {
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
          for(var i = 0; i < temp.replies.length; i++){
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
    return this.http.post(this.commentsEndpoint, comment, this.httpOptions)
      .pipe(map(res => {
        var comment = <Comment>res;
        setDisplayName(comment.author);
        return comment;
      }));
  }

  getReplies(commentId: number) {
    return this.http.get(this.commentsEndpoint + "/" + commentId, this.httpOptions)
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
