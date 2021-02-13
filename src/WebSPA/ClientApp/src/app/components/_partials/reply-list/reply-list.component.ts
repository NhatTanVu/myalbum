import { Component, OnInit, Input } from '@angular/core';
import { Comment } from '../../../models/comment';
import { CommentService } from 'src/app/services/comment.service';
import { ToastyService, ToastData } from 'ng2-toasty';

@Component({
  selector: 'app-reply-list',
  templateUrl: './reply-list.component.html',
  styleUrls: ['./reply-list.component.css']
})
export class ReplyListComponent implements OnInit {

  @Input()
  replies: Comment[] = null;
  @Input()
  parent: Comment = null;
  @Input()
  userName: string = null;

  constructor(
    private commentService: CommentService,
    private toasty: ToastyService
  ) { }

  ngOnInit() {
  }

  toggleReplies(reply: Comment) {
    if (!reply.areRepliesLoaded) {
      var result$ = this.commentService.getReplies(reply.id);
      result$.subscribe(
        replies => {
          reply.replies = replies;
          reply.areRepliesLoaded = true;
          reply.isViewing = true;
        },
        err => {
          console.log(err);
        }
      )
    }
    else {
      reply.isViewing = !reply.isViewing;
    }
  }

  delete(reply: Comment) {
    var result$ = this.commentService.delete(reply.id);
    result$.subscribe(
      res => {
        this.toasty.success({
          title: "Success",
          msg: "Deleted successfully.",
          theme: "bootstrap",
          showClose: true,
          timeout: 1500,
        });
        for (var i = 0; i < this.replies.length; i++) {
          if (this.replies[i].id === reply.id) {
            this.replies.splice(i, 1);
            if (this.parent) {
              this.parent.numOfReplies--;
            }
            break;
          }
        }
      },
      err => {
        this.toasty.error({
          title: "Error",
          msg: "Error occurred. Please try again!",
          theme: "bootstrap",
          showClose: true,
          timeout: 1500
        });
        console.log(err);
      }
    );
  }
}