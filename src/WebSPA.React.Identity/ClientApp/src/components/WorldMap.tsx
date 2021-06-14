import React, { Component } from 'react';
import { Container, Row, Col } from 'reactstrap';
import { Photo } from '../models/photo';
import { PhotoService } from '../services/photo.service';
import styles from './WorldMap.module.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

declare var Tessarray: any;

interface IWorldMapProps { }
interface IWorldMapState {
    allPhotos: Photo[],
    viewportPhotos: Photo[]
}

export class WorldMap extends Component<IWorldMapProps, IWorldMapState> {
    private photoService = new PhotoService();

    constructor(props: IWorldMapProps) {
        super(props);
        this.state = {
            allPhotos: [],
            viewportPhotos: []
        };
        //this.incrementCounter = this.incrementCounter.bind(this);
    }

    //incrementCounter() {
    //  this.setState({
    //    currentCount: this.state.currentCount + 1
    //  });
    //}

    componentDidMount() {
        this.photoService.getAll([]).then(data => {
            this.setState({
                allPhotos: data,
                viewportPhotos: data
            });
        });
    }

    render() {
        return (
            <div id="world-map">
                <Row>
                    <Col lg={{ size: 4, order: 1 }} className="order-2">
                        <div className={styles["explore-gallery"]}>
                            {this.state.viewportPhotos.map(photo =>
                                <div className="image-box rounded" key={photo.id}>
                                    <img src={photo.filePath} alt={photo.name} />
                                    <a href={"/photo/" + photo.id} target="_blank" className="external-button">
                                        <FontAwesomeIcon icon="external-link-alt" className="edit-link" />
                                    </a>
                                </div>
                            )}
                        </div>
                    </Col>
                    <Col lg={{ size: 8, order: 2 }} className="mb-lg-0 mb-2 order-1">
                        <input id="gmapSearchBox" className="form-control mb-2" type="text" placeholder="Type location here..." />
                        <div id="gmap"></div>
                    </Col>
                </Row>
            </div>
        );
    }

    componentDidUpdate(prevProps: IWorldMapProps, prevState: IWorldMapState) {
        var tessarray = new Tessarray("." + styles["explore-gallery"], ".image-box", {
            selectorClass: false,
            boxTransition: false,
            boxTransformOutTransition: false,
            flickr: {
                // http://flickr.github.io/justified-layout/
                targetRowHeight: 150,
                targetRowHeightTolerance: 0.25,
                boxSpacing: {
                    horizontal: 5,
                    vertical: 5
                },
                containerPadding: 0
            }
        });
    }
}
