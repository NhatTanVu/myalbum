import React, { Component } from 'react';
import { Row, Col } from 'reactstrap';
import { Album } from '../models/album';
import './ViewAlbum.css';
//import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { RouteComponentProps } from 'react-router';
import { DisplayMode } from '../models/globalData';
import { GlobalDataContext } from '../context/GlobalDataContext';
import { ExplorePhotos } from "./ExplorePhotos"
import { AlbumService } from '../services/album.service';

interface IViewAlbumProps { }
interface IViewAlbumState {
    album: Album | null
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
        this.state = {
            album: null
        };
    }

    componentDidMount() {
        this.albumService.get(this.albumId).then(album => {
            if (album) {
                this.setState({
                    album: album
                });
            }
            else {
                this.props.history.push('/album');
            }
        });
        this.context?.setDisplayMode(DisplayMode.Album);
        this.context?.setEnableDisplayMode(false);
    }

    componentWillUnmount() {
    }

    render() {
        return (
            this.state.album &&
                <div>
                    <Row>
                        <Col lg={{ size: 6 }}>
                            <h3 className="header-container">
                                <span className="header-text" title={this.state.album.name as string}>{this.state.album.name as string}</span>
                                {/*<a className="btn btn-secondary header-button" href="/album/edit/{{ album.id }}" *ngIf="isEditable()"
                                    title="Edit Album">
                                    <i class="fas fa-pencil-alt"></i>
                                </a>*/}
                            </h3>
                            <hr />
                        </Col>
                    </Row>
                    <ExplorePhotos albumId={this.state.album.id} />
                </div>
        );
    }
}
