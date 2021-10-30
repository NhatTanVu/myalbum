import React, { Component } from 'react';
import { Comment } from '../models/comment';
import { CommentService } from '../services/comment.service';
import './ReplyList.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import Moment from 'moment';
import { ReplyForm } from './ReplyForm';
import { toast } from 'react-toastify';

interface IReplyListProps {
    parent?: Comment,
    replies: Comment[],
    userName: string,
    onDeleteComment: (deletedComment: Comment) => void
}
interface IReplyListState {
}

export class ReplyList extends Component<IReplyListProps, IReplyListState> {
    private commentService = new CommentService();

    constructor(props: IReplyListProps) {
        super(props);
        this.handleEditComment = this.handleEditComment.bind(this);
        this.handleNewComment = this.handleNewComment.bind(this);
    }

    handleEditComment(newComment: Comment, editComment: Comment) {
        editComment.isEditing = false;
        editComment.content = newComment.content;
        editComment.modifiedDate = newComment.modifiedDate;
        this.forceUpdate();
    }

    handleNewComment(newComment: Comment, parentComment?: Comment) {
        if (parentComment) {
            parentComment.isReplying = false;
            parentComment.replies.push(newComment);
            parentComment.numOfReplies = parentComment.replies.length;
            this.forceUpdate();
        }
    }

    delete(reply: Comment) {
        this.commentService.delete(reply.id).then(comment => {
            if (comment) {
                toast("Deleted successfully.");
                this.props.onDeleteComment(reply);
            }
            else {
                toast("Error occurred. Please try again!");
            }
        });
    }

    toggleReplies(reply: Comment) {
        if (!reply.areRepliesLoaded) {
            this.commentService.getReplies(reply.id).then(replies => {
                reply.replies = replies;
                reply.areRepliesLoaded = true;
                reply.isViewing = true;
                console.log("Comment " + reply.id + " loaded, isViewing: " + reply.isViewing);
                this.forceUpdate();
            });
        }
        else {
            reply.isViewing = !reply.isViewing;
            console.log("Comment " + reply.id + " isViewing: " + reply.isViewing);
            this.forceUpdate();
        }
    }

    render() {
        return (
            this.props.replies.map(reply => {
                return (
                    <div className="mb-2 pl-2 reply-list" key={reply.id}>
                        <div>
                            <b>{reply.author.displayName}</b>
                            <span>&nbsp;- {Moment(reply.createdDate).format("HH:mm DD/MMM/YY")}</span>
                            {reply.modifiedDate && <span> (edited)</span>}
                            {reply.isNew && <span><FontAwesomeIcon icon="comment-medical" className="ml-2 text-info"></FontAwesomeIcon></span>}
                        </div>
                        {(reply.author.userName === this.props.userName) && <div>
                            <button className="btn btn-link p-0" onClick={(e: any) => { reply.isEditing = !reply.isEditing; this.forceUpdate(); }}>
                                {!reply.isEditing ? <span>Edit</span> : <span>Close</span >}
                            </button>
                            <span>&nbsp;|&nbsp;</span>
                            <button className="btn btn-link p-0" onClick={(e: any) => { this.delete(reply) }} title="Delete Comment">Delete</button>
                        </div>}
                        {!reply.isEditing ? <div>{reply.content}</div> : <ReplyForm isNew={false} photoId={reply.photoId} editComment={reply} onEditComment={this.handleEditComment} ></ReplyForm>}
                        <div>
                            {(reply.numOfReplies > 0) &&
                                <button className="btn btn-link p-0" onClick={(e: any) => { this.toggleReplies(reply) }}>
                                    {!reply.isViewing ? <span>View</span> : <span>Hide</span>} {reply.numOfReplies} replies
                                </button>}
                            {(reply.numOfReplies > 0) && <span>&nbsp;|&nbsp;</span>}
                            <button className="btn btn-link p-0" onClick={(e: any) => { reply.isReplying = !reply.isReplying; this.forceUpdate(); }}>
                                {!reply.isReplying ? <span>Reply</span> : <span>Close</span>}
                            </button>
                            {reply.isReplying && <ReplyForm isNew={true} photoId={reply.photoId} parentId={reply.id} parentComment={reply} onCreateComment={this.handleNewComment} ></ReplyForm>}
                            {reply.isViewing && <ReplyList parent={reply} replies={reply.replies} userName={this.props.userName} onDeleteComment={this.props.onDeleteComment} ></ReplyList>}
                        </div>
                    </div>
                );
            }));
    }
}
