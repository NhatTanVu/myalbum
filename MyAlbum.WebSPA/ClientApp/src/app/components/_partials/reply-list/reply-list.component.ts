import { Component, OnInit, Input } from '@angular/core';
import { Comment } from '../../../models/comment';
import { CommentService } from 'src/app/services/comment.service';

@Component({
  selector: 'app-reply-list',
  templateUrl: './reply-list.component.html',
  styleUrls: ['./reply-list.component.css']
})
export class ReplyListComponent implements OnInit {

  @Input()
  replies: Comment[] = null;

  constructor(
    private commentService: CommentService
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
}