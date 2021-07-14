import React, { Component } from 'react';
import { Album } from '../models/album';
import { AlbumService } from '../services/album.service';
import './ExploreAlbums.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { GlobalDataContext } from '../context/GlobalDataContext';
import { DisplayMode } from '../models/globalData';

declare const Tessarray: any;

interface IExploreAlbumsProps { }
interface IExlploreAlbumsState {
    albums: Album[];
}

export class ExploreAlbums extends Component<IExploreAlbumsProps, IExlploreAlbumsState> {
    static contextType = GlobalDataContext;
    context!: React.ContextType<typeof GlobalDataContext>;

    private albumService = new AlbumService();

    constructor(props: IExploreAlbumsProps) {
        super(props);
        this.state = {
            albums: []
        };
    }

    componentDidMount() {
        this.albumService.getAll([]).then(data => {
            this.setState({
                albums: data
            });
        });
        this.context?.setDisplayMode(DisplayMode.Album);
        this.context?.setEnableDisplayMode(true);
    }

    render() {
        return (
            <div className="album-gallery">
                {this.state.albums.map(album =>
                    <a href={"/album/" + album.id} key={album.id}>
                        <div className="album-box rounded">
                            <div className={"main-photo" + ((album.subPhotos[0] == null) ? " full" : "")}>
                                {(album.mainPhoto != null) && <img src={album.mainPhoto.filePath} alt={album.name ?? ""} />}
                                {(album.mainPhoto == null) && <img className="default-album" src="/default_album.jpg" alt={album.name ?? ""} />}
                            </div>
                            <div className="sub-photos">
                                {album.subPhotos.map(photo =>
                                    (photo != null) &&
                                    <div className="photo" key={photo.id}>
                                        <img src={photo.filePath} alt={album.name ?? ""} />
                                    </div>
                                )}
                            </div>
                            <div className="content">
                                <div className="text">
                                    <div>
                                        <div className="title" title={album.name ?? ""}>{album.name}</div>
                                        <div className="author">by {album.author?.displayName}</div>
                                    </div>
                                </div>
                                <div className="total-photos">
                                    <div>
                                        <FontAwesomeIcon icon="images" /> {album.photos.length}
                                    </div>
                                </div>
                            </div>
                        </div>
                    </a>
                )}
            </div>
        );
    }

    componentDidUpdate(prevProps: IExploreAlbumsProps, prevState: IExlploreAlbumsState) {
        const tessarray = new Tessarray(".album-gallery", ".album-box", {
            selectorClass: false,
            imageClass: ".album-box .main-photo img",
            boxTransition: false,
            boxTransformOutTransition: false,
            flickr: {
                // http://flickr.github.io/justified-layout/
                targetRowHeight: 300,
                targetRowHeightTolerance: 0.25,
                boxSpacing: {
                    horizontal: 20,
                    vertical: 20
                },
                containerPadding: {
                    left: 0,
                    right: 15,
                    bottom: 0,
                    top: 0
                },
                forceAspectRatio: 1
            }
        });
    }
}
