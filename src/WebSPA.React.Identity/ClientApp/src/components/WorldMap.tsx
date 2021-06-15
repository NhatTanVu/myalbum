import React, { Component } from 'react';
import { Container, Row, Col } from 'reactstrap';
import { Photo } from '../models/photo';
import { PhotoService } from '../services/photo.service';
import styles from './WorldMap.module.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import GoogleMapReact from 'google-map-react';
import MarkerClusterer from '@googlemaps/markerclustererplus';

declare var Tessarray: any;

interface IWorldMapProps { }
interface IWorldMapState {
    allPhotos: Photo[],
    viewportPhotos: Photo[]
}

export class WorldMap extends Component<IWorldMapProps, IWorldMapState> {
    private photoService = new PhotoService();
    private googleMapRef: any;

    constructor(props: IWorldMapProps) {
        super(props);
        this.state = {
            allPhotos: [],
            viewportPhotos: []
        };
        this.setGoogleMapRef = this.setGoogleMapRef.bind(this);
    }

    componentDidMount() {
        this.photoService.getAll([]).then(data => {
            this.setState({
                allPhotos: data,
                viewportPhotos: data
            });
        });
    }

    setGoogleMapRef(map: google.maps.Map) {
        this.googleMapRef = map;

        let locations = [
            { lat: -31.563910, lng: 147.154312 },
            { lat: -33.718234, lng: 150.363181 },
            { lat: -33.727111, lng: 150.371124 }]
        let markers = locations && locations.map((location) => {
            return new google.maps.Marker({ position: location })
        })
        let markerCluster = new MarkerClusterer(map, markers, {
            imagePath: '/lib/@googlemaps/markerclustererplus/m'
        })
        this.googleMapRef.setCenter({ lat: -31.563910, lng: 147.154312 });
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
                        <div id="gmap">
                            <GoogleMapReact 
                                bootstrapURLKeys={{ key: "" }}
                                yesIWantToUseGoogleMapApiInternals
                                onGoogleApiLoaded={({ map }) => this.setGoogleMapRef(map)}
                                defaultCenter={{ lat: 3.140853, lng: 101.693207 }} /* KUL */
                                defaultZoom={ 12 }
                                options={{ mapTypeId: 'roadmap' }}
                            />
                        </div>
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
