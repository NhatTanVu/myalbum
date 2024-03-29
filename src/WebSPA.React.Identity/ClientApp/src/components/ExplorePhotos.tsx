import React, { Component } from 'react';
import { Photo } from '../models/photo';
import { PhotoService } from '../services/photo.service';
import styles from './ExplorePhotos.module.scss';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { GlobalDataContext } from '../context/GlobalDataContext';
import { DisplayMode } from '../models/globalData';
import { RouteComponentProps } from 'react-router';

declare const Tessarray: any;

interface IExplorePhotosProps {
    albumId: number;
}
interface IExplorePhotosState {
    photos: Photo[];
}

export class ExplorePhotos extends Component<IExplorePhotosProps & RouteComponentProps, IExplorePhotosState> {
    static contextType = GlobalDataContext;
    context!: React.ContextType<typeof GlobalDataContext>;

    private photoService = new PhotoService();

    constructor(props: IExplorePhotosProps & RouteComponentProps) {
        super(props);
        this.state = {
            photos: []
        };
    }

    componentDidMount() {
        console.log("React.version = " + React.version);
        let filter: any = {};
        let params = new URLSearchParams(this.props.location?.search);
        if (params?.get("catId"))
            filter.categoryId = parseInt(params.get("catId") as string);
        if (this.props.albumId) {
            filter.albumId = this.props.albumId;
            this.context?.setDisplayMode(DisplayMode.Album);
            this.context?.setEnableDisplayMode(false);
        }
        else {
            this.context?.setDisplayMode(DisplayMode.Photo);
            this.context?.setEnableDisplayMode(true);
        }
        this.photoService.getAll(filter).then(data => {
            this.setState({
                photos: data
            });
        });
    }

    render() {
        return (
            <div className={styles["explore-gallery"]}>
                {this.state.photos.map(photo =>
                    <a href={"/photo/" + photo.id} key={photo.id}>
                        <div className="image-box rounded">
                            <img src={photo.filePath} alt={photo.name} />
                            <div className="interaction-view">
                                <div className="interaction-bar">
                                    <div className="text">
                                        <div className="title" title={photo.name}>{photo.name}</div>
                                        <div className="author" title={photo?.author.displayName}>by {photo.author.displayName}</div>
                                    </div>
                                    <div className="engagement">
                                        <div className="total-comments" title={photo.totalComments.toString()}>
                                            <FontAwesomeIcon icon="comments" /> {photo.totalComments}
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </a>
                )}
            </div>
        );
    }

    componentDidUpdate(prevProps: IExplorePhotosProps, prevState: IExplorePhotosState) {
        new Tessarray("." + styles["explore-gallery"], ".image-box", {
            selectorClass: false,
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
                containerPadding: 0
            }
        });
    }
}
