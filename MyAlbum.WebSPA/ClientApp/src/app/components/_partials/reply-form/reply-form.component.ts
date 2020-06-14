import { Component, OnInit, Input } from '@angular/core';
import { Comment } from '../../../models/comment';
import { CommentService } from 'src/app/services/comment.service';
import { ToastyService } from 'ng2-toasty';

@Component({
  selector: 'app-reply-form',
  templateUrl: './reply-form.component.html',
  styleUrls: ['./reply-form.component.css']
})
export class ReplyFormComponent implements OnInit {

  newReply: Comment = {
    id: 0,
    photoId: 0,
    parentId: null,
    content: "",
    author: {
      userName: "",
      firstName: "",
      lastName: "",
      displayName: ""
    },
    connectionId: "",
    isNew: true,
    isReplying: false,
    isViewing: false,
    areRepliesLoaded: false,
    numOfReplies: 0,
    replies: []
  };

  @Input()
  parent: Comment;

  constructor(
    private commentService: CommentService,
    private toasty: ToastyService
  ) { }

  ngOnInit() {
  }

  ngAfterViewInit() {
    this.newReply.photoId = this.parent.photoId;
    this.newReply.parentId = this.parent.id;
  }

  resetNewReply() {
    this.newReply = {
      id: 0,
      photoId: this.parent.photoId,
      parentId: null,
      content: "",
      author: {
        userName: "",
        firstName: "",
        lastName: "",
        displayName: ""
      },
      connectionId: "",
      isNew: true,
      isReplying: false,
      isViewing: false,
      areRepliesLoaded: false,
      numOfReplies: 0,
      replies: []
    };
  }

  submitReply() {
    var result$ = this.commentService.create(this.newReply);
    result$.subscribe(
      comment => {
        this.toasty.success({
          title: "Success",
          msg: "Posted successfully.",
          theme: "bootstrap",
          showClose: true,
          timeout: 1500
        });
        this.resetNewReply();
        var result2$ = this.commentService.getReplies(comment.parentId);
        result2$.subscribe(
          replies => {
            this.parent.replies = replies;
            this.parent.numOfReplies = replies.length;
            this.parent.isReplying = false;
            this.parent.isViewing = true;
            this.parent.areRepliesLoaded = true;
          }
        );
      },
      err => {
        console.log(err);
      }
    );
  }
}