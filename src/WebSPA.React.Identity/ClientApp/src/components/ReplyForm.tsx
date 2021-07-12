import React, { Component } from 'react';
import './ReplyForm.css';
import { CommentService } from '../services/comment.service';

interface IReplyFormProps {
}
interface IReplyFormState {
}

export class ReplyForm extends Component<IReplyFormProps, IReplyFormState> {
    private commentService = new CommentService();

    render() {
        return (
            <div className="newReplyForm mb-2">
                <form className="container pt-2">
                    <div className="row">
                        <div className="col-12">
                            <label htmlFor="newComment" className="required">Comment:</label>
                        </div>
                        <div className="col-12">
                            <div className="form-group">
                                <textarea name="newComment" className="form-control" required></textarea>
                            </div>
                            <div className="form-group">
                                <button className="btn btn-primary" type="submit">Post</button>
                            </div>
                        </div>
                    </div>
                </form>                    
            </div>
        );
    }
}
