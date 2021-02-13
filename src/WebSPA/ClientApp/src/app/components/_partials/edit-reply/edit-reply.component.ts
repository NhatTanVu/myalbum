import { Component, OnInit, Input } from '@angular/core';
import { Comment } from '../../../models/comment';
import { CommentService } from 'src/app/services/comment.service';
import { ToastyService } from 'ng2-toasty';

@Component({
  selector: 'app-edit-reply',
  templateUrl: './edit-reply.component.html',
  styleUrls: ['./edit-reply.component.css']
})
export class EditReplyComponent implements OnInit {

  @Input()
  comment: Comment;

  constructor(
    private commentService: CommentService,
    private toasty: ToastyService
  ) { }

  ngOnInit() {
  }

  ngAfterViewInit() {
  }

  submitReply() {
    var result$ = this.commentService.save(this.comment);
    result$.subscribe(
      comment => {
        this.toasty.success({
          title: "Success",
          msg: "Updated successfully.",
          theme: "bootstrap",
          showClose: true,
          timeout: 1500
        });
        this.comment.isEditing = false;
        this.comment.modifiedDate = comment.modifiedDate;
      },
      err => {
        console.log(err);
      }
    );
  }
}