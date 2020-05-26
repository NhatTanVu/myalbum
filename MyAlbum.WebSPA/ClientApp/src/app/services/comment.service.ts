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
      'Content-Type':  'application/json',
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
        this.hubConnection.invoke("GetConnectionId").then((connectionId) =>{
          console.log("ConnectionId: " + connectionId);
          this.connectionId = connectionId;
        });
        console.log('Connection started');
      })
      .catch(err => console.log('Error while starting connection: ' + err))
  }

  public addedComment$: Observable<Comment> = new Observable<Comment>(e => this.emitter = e);
  private emitter: Subscriber<Comment>;
  private connectionId: string;
  private addCommentAddedListener = () => {
    this.hubConnection.on('commentAdded', (result) => {
      var comment = <Comment>result;
      console.log('commentAdded - ' + JSON.stringify(comment));
      if (comment.connectionId != this.connectionId) {
        setDisplayName(comment.author);
        comment.isNew = true;
        this.emitter.next(comment);
      }
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
}
