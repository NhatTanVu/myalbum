import React, { Component } from 'react';
import { Photo } from '../models/photo';
import { PhotoService } from '../services/photo.service';
import styles from './Home.module.scss';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

declare var Tessarray: any;

interface IHomeProps { }
interface IHomeState {
    photos: Photo[];
}

export class Home extends Component<IHomeProps, IHomeState> {
    private photoService = new PhotoService();

    constructor(props: IHomeProps) {
        super(props);
        this.state = {
            photos: []
        };
    }

    componentDidMount() {
        this.photoService.getAll([]).then(data => {
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
                                        <div className="author" title={photo.author.displayName}>by {photo.author.displayName}</div>
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

    componentDidUpdate(prevProps: IHomeProps, prevState: IHomeState) {
        var tessarray = new Tessarray("." + styles["explore-gallery"], ".image-box", {
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
