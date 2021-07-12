import React, { Component } from 'react';
import { Row, Col } from 'reactstrap';
import { Album } from '../models/album';
import './ViewAlbum.css';
//import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { RouteComponentProps } from 'react-router';
import { DisplayMode } from '../models/globalData';
import { GlobalDataContext } from '../context/GlobalDataContext';

interface IViewAlbumProps { }
interface IViewAlbumState {
    album: Album
}

interface IViewAlbumParams {
    id: string;
}

export class ViewAlbum extends Component<IViewAlbumProps & RouteComponentProps<IViewAlbumParams>, IViewAlbumState> {
    static contextType = GlobalDataContext;
    context!: React.ContextType<typeof GlobalDataContext>;

    constructor(props: IViewAlbumProps & RouteComponentProps<IViewAlbumParams>) {
        super(props);
        this.state = {
            album: {
                id: -1,
                name: "Test",
                createdDate: null,
                modifiedDate: null,
                author: null,
                mainPhoto: null,
                subPhotos: [],
                photos: []
            }
        };
    }

    componentDidMount() {
        this.context?.setDisplayMode(DisplayMode.Album);
    }

    componentWillUnmount() {
    }

    render() {
        return (
            <div>
                <div className="row">
                    <div className="col-lg-6">
                        <h3 className="header-container">
                            <span className="header-text" title={this.state.album.name as string}>{this.state.album.name as string}</span>
                            {/*<a className="btn btn-secondary header-button" href="/album/edit/{{ album.id }}" *ngIf="isEditable()"
                                title="Edit Album">
                                <i class="fas fa-pencil-alt"></i>
                            </a>*/}
                        </h3>
                        <hr />
                    </div>
                </div>
            </div>
        );
    }
}
