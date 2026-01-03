import React, { Component } from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import './ReplyForm.css';
import { CommentService } from '../services/comment.service';
import { AIService } from '../services/ai.service';
import { Comment, SaveComment } from '../models/comment';
import { SuggestCommentRequest } from '../models/ai.service';
import { toast } from 'react-toastify';


interface IReplyFormProps {
    isNew: boolean,
    photoId: number,
    photoUrl: string,
    parentId?: number,
    parentComment?: Comment,
    editComment?: Comment,
    onCreateComment?: (newComment: Comment, parentComment?: Comment) => void
    onEditComment?: (newComment: Comment, editComment: Comment) => void
}
interface IReplyFormState {
    comment: SaveComment
}

export class ReplyForm extends Component<IReplyFormProps, IReplyFormState> {
    private commentService = new CommentService();
    private aiService = new AIService();

    constructor(props: IReplyFormProps) {
        super(props);
        this.handleContentChange = this.handleContentChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleSuggestCommentAI = this.handleSuggestCommentAI.bind(this);
        this.state = {
            comment: {
                content: "",
                photoId: this.props.photoId,
                parentId: this.props.parentId
            }
        };
    }

    handleSubmit(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();

        if (this.state.comment.content === "") {
            toast("Content required.");
            return;
        }

        if (this.props.isNew) {
            this.commentService.create(this.state.comment).then(comment => {
                if (comment) {
                    toast("Created successfully.");
                    this.props.onCreateComment
                        && this.props.onCreateComment(comment, this.props.parentComment);
                    this.setState({
                        comment: {
                            ...comment,
                            content: ""
                        }
                    });
                }
                else {
                    toast("Error occurred. Please try again!");
                }
            });
        }
        else {
            this.commentService.save(this.state.comment).then(comment => {
                if (comment) {
                    toast("Saved successfully.");
                    this.props.onEditComment
                        && this.props.editComment
                        && this.props.onEditComment(comment, this.props.editComment);
                    this.setState({
                        comment: {
                            ...comment,
                            content: ""
                        }
                    });
                }
                else {
                    toast("Error occurred. Please try again!");
                }
            });
        }
    }

    handleSuggestCommentAI(event: React.MouseEvent<HTMLButtonElement>) {
        const rawContent = this.state.comment.content || "";
        const hints = rawContent
            .split(",")
            .map(h => h.trim())
            .filter(h => h.length > 0);

        if (hints.length === 0) {
            toast("Please provide at least one hint before asking for suggestions.");
            return;
        }

        let payload: SuggestCommentRequest = {
            image_url: this.props.photoUrl,
            hints: hints
        }
        this.aiService.suggestComment(payload)
            .then(response => {
                if (response && response.comments && response.comments.length > 0) {
                    this.setState((prevState, props) => ({
                        comment: {
                            ...prevState.comment,
                            content: response.comments[0]
                        } as SaveComment
                    }));
                } else {
                    toast("Error occurred. Please try again!");
                }
            })
            .catch(() => {
                toast("Error occurred. Please try again!");
            });
    }

    handleContentChange(event: React.FormEvent<HTMLTextAreaElement>) {
        let currentValue = event.currentTarget.value;
        this.setState((prevState, props) => ({
            comment: {
                ...prevState.comment,
                content: currentValue
            } as SaveComment
        }));
    }

    componentDidMount() {
        this.populateState();
    }

    populateState() {
        if (!this.props.isNew && this.props.editComment) {
            this.setState({
                comment: this.props.editComment
            });
        }
        else {
            this.setState({
                comment: {
                    content: "",
                    photoId: this.props.photoId,
                    parentId: this.props.parentId
                }
            });
        }
    }

    render() {
        return (
            <div className="newReplyForm mb-2">
                <form className="container pt-2" onSubmit={this.handleSubmit}>
                    <div className="row">
                        <div className="col-12">
                            <label htmlFor="newComment" className="required">Comment:</label>
                        </div>
                        <div className="col-12">
                            <div className="form-group">
                                <textarea name="newComment" className="form-control" required value={this.state.comment.content} onChange={this.handleContentChange}></textarea>
                            </div>
                            <div className="form-group">
                                <button className="btn btn-primary" type="submit">Post</button>
                                <button className="btn btn-secondary header-button ml-1" type="button" title="Suggest a comment using AI" onClick={this.handleSuggestCommentAI}>
                                    <FontAwesomeIcon icon="magic" />
                                </button>
                            </div>
                        </div>
                    </div>
                </form>
            </div>
        );
    }
}
