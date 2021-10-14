import React, { Component } from 'react';
import { Row, Col } from 'reactstrap';
import { Album } from '../models/album';
import './ViewAlbum.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { RouteComponentProps } from 'react-router';
import { DisplayMode } from '../models/globalData';
import { GlobalDataContext } from '../context/GlobalDataContext';
import { ExplorePhotos } from "./ExplorePhotos"
import { AlbumService } from '../services/album.service';
import authService from './api-authorization/AuthorizeService';

interface IViewAlbumProps { }
interface IViewAlbumState {
    album: Album | null,
    isEditable: boolean,
    isEditMode: boolean,
    editAlbum: Album | null
}

interface IViewAlbumParams {
    id: string;
}

export class ViewAlbum extends Component<IViewAlbumProps & RouteComponentProps<IViewAlbumParams>, IViewAlbumState> {
    static contextType = GlobalDataContext;
    context!: React.ContextType<typeof GlobalDataContext>;

    private albumService = new AlbumService();
    private albumId: number;

    constructor(props: IViewAlbumProps & RouteComponentProps<IViewAlbumParams>) {
        super(props);
        this.albumId = parseInt(this.props.match.params.id);
        this.switchToEditMode = this.switchToEditMode.bind(this);
        this.changeAlbumName = this.changeAlbumName.bind(this);
        this.saveAlbumName = this.saveAlbumName.bind(this);
        this.cancelAlbumName = this.cancelAlbumName.bind(this);
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

    changeAlbumName(event: React.FormEvent<HTMLInputElement>) {
        let currentValue = event.currentTarget.value;
        this.setState((prevState, props) => ({
            editAlbum: {
                ...prevState.editAlbum,
                name: currentValue
            } as Album
        }));
    }

    saveAlbumName(event: React.MouseEvent<HTMLButtonElement>) {
        this.setState({
            isEditable: true,
            isEditMode: false
        });
    }

    cancelAlbumName(event: React.MouseEvent<HTMLButtonElement>) {
        this.setState({
            isEditable: true,
            isEditMode: false
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
                                    <button className="btn btn-secondary header-button" type="button" onClick={this.switchToEditMode}>
                                        <FontAwesomeIcon icon="pencil-alt" />
                                    </button>
                                }
                                {this.state.isEditMode &&
                                    <span className="form-inline">
                                        <input id="albumName" name="albumName" type="text" className="form-control col-lg-8 mr-3"
                                            value={this.state.editAlbum?.name as string} onChange={this.changeAlbumName} />
                                        <button className="btn btn-primary header-button mr-1" type="button" onClick={this.saveAlbumName}>
                                            <FontAwesomeIcon icon="check" />
                                        </button>
                                        <button className="btn btn-secondary header-button" type="button" onClick={this.cancelAlbumName}>
                                            <FontAwesomeIcon icon="times" />
                                        </button>
                                    </span>
                                }
                            </h3>
                            <hr />
                        </Col>
                    </Row>
                    <ExplorePhotos albumId={this.state.album.id} />
                </div>
        );
    }
}
