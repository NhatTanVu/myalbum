import React, { Component } from 'react';
import { Row, Col } from 'reactstrap';
import { Album, SaveAlbum } from '../models/album';
import './ViewEditAlbum.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { RouteComponentProps } from 'react-router';
import { DisplayMode } from '../models/globalData';
import { GlobalDataContext } from '../context/GlobalDataContext';
import { ExplorePhotos } from "./ExplorePhotos"
import { AlbumService } from '../services/album.service';
import authService from './api-authorization/AuthorizeService';
import { toast } from 'react-toastify';

interface IViewEditAlbumProps { }
interface IViewEditAlbumState {
    album: Album | null,
    isEditable: boolean,
    isEditMode: boolean,
    editAlbum: Album | null
}

interface IViewEditAlbumParams {
    id: string;
}

export class ViewEditAlbum extends Component<IViewEditAlbumProps & RouteComponentProps<IViewEditAlbumParams>, IViewEditAlbumState> {
    static contextType = GlobalDataContext;
    context!: React.ContextType<typeof GlobalDataContext>;

    private albumService = new AlbumService();
    private albumId: number;

    constructor(props: IViewEditAlbumProps & RouteComponentProps<IViewEditAlbumParams>) {
        super(props);
        this.albumId = parseInt(this.props.match.params.id);
        this.switchToEditMode = this.switchToEditMode.bind(this);
        this.handleNameChange = this.handleNameChange.bind(this);
        this.saveAlbumName = this.saveAlbumName.bind(this);
        this.cancelAlbumName = this.cancelAlbumName.bind(this);
        this.deleteAlbum = this.deleteAlbum.bind(this);
        this.state = {
            album: null,
            isEditable: false,
            isEditMode: false,
            editAlbum: null
        };
    }

    componentDidMount() {
        this.context?.setDisplayMode(DisplayMode.Album);
        this.context?.setEnableDisplayMode(false);
        this.populateState();
    }

    async populateState() {
        let user = await authService.getUser();
        let userName = user ? user["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"] : null;
        this.albumService.get(this.albumId).then(album => {
            if (album) {
                this.setState({
                    album: album,
                    isEditable: album.author !== null && album.author.userName === userName
                });
            }
            else {
                this.props.history.push('/album');
            }
        });
    }

    switchToEditMode() {
        this.setState({
            isEditable: false,
            isEditMode: true,
            editAlbum: this.state.album
        });
    }

    handleNameChange(event: React.FormEvent<HTMLInputElement>) {
        let currentValue = event.currentTarget.value;
        this.setState((prevState, props) => ({
            editAlbum: {
                ...prevState.editAlbum,
                name: currentValue
            } as Album
        }));
    }

    saveAlbumName(event: React.MouseEvent<HTMLButtonElement>) {
        if (this.state.editAlbum?.name === "") {
            toast("Album Name required.");
            return;
        }

        this.albumService.save(this.state.editAlbum as SaveAlbum).then(album => {
            if (album) {
                toast("Saved successfully.");
                this.setState({
                    isEditable: true,
                    isEditMode: false,
                    album: this.state.editAlbum
                });
            }
            else {
                toast("Error occurred. Please try again!");
            }
        });
    }

    cancelAlbumName(event: React.MouseEvent<HTMLButtonElement>) {
        this.setState({
            isEditable: true,
            isEditMode: false
        });
    }

    deleteAlbum() {
        this.albumService.delete(this.state.album?.id as number).then(res => {
            if (res) {
                toast("Deleted successfully.");
                this.props.history.push('/album');
            }
            else {
                toast("Error occurred. Please try again!");
            }
        });
    }

    render() {
        return (
            this.state.album &&
                <div>
                    <Row>
                        <Col lg={{ size: 6 }}>
                            <h3 className="header-container">
                                {!this.state.isEditMode &&
                                    <span className="header-text" title={this.state.album.name as string}>{this.state.album.name as string}</span>
                                }
                                {this.state.isEditable &&
                                    <div className="float-right">
                                        <button className="btn btn-secondary header-button mr-1" type="button" onClick={this.switchToEditMode}>
                                            <FontAwesomeIcon icon="pencil-alt" />
                                        </button>
                                        <button className="btn btn-danger header-button" type="button" onClick={this.deleteAlbum}>
                                            <FontAwesomeIcon icon="trash-alt" />
                                        </button>
                                    </div>
                                }
                                {this.state.isEditMode &&
                                    <div className="container-fluid">
                                        <div className="row">
                                            <input id="albumName" name="albumName" type="text" className="form-control col-md-8 mb-2 mb-md-0"
                                                value={this.state.editAlbum?.name as string} onChange={this.handleNameChange} style={{ height: "auto" }} />
                                            <div className="col-md-4 text-right p-0">
                                                <button className="btn btn-primary header-button mr-1" type="button" onClick={this.saveAlbumName}>
                                                    <FontAwesomeIcon icon="check" />
                                                </button>
                                                <button className="btn btn-secondary header-button" type="button" onClick={this.cancelAlbumName}>
                                                    <FontAwesomeIcon icon="times" />
                                                </button>
                                            </div>
                                        </div>
                                    </div>
                                }
                            </h3>
                            <hr />
                        </Col>
                </Row>
                <ExplorePhotos albumId={this.state.album.id}
                    history={this.props.history}
                    location={this.props.location}
                    match={this.props.match} />
                </div>
        );
    }
}
