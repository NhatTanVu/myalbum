import React, { Component } from 'react';
import { Row, Col } from 'reactstrap';
import { Photo } from '../models/photo';
import { PhotoService } from '../services/photo.service';
import styles from './WorldMap.module.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import GoogleMapReact from 'google-map-react';
import MarkerClusterer from '@googlemaps/markerclustererplus';
import { GlobalDataContext } from '../context/GlobalDataContext';
import { DisplayMode } from '../models/globalData';
import { GlobalDataService } from '../services/globalData.service';

declare const Tessarray: any;

interface IWorldMapProps { }
interface IWorldMapState {
    viewportPhotos: Photo[],
    googleApiKey: string,
    loadedGoogleApiKey: boolean
}

export class WorldMap extends Component<IWorldMapProps, IWorldMapState> {
    static contextType = GlobalDataContext;
    context!: React.ContextType<typeof GlobalDataContext>;

    private photoService = new PhotoService();
    private globalDataService = new GlobalDataService();
    private allPhotos: Photo[] = [];
    private gmapSearchBoxRef: React.RefObject<HTMLInputElement>;
    private map?: google.maps.Map;

    constructor(props: IWorldMapProps) {
        super(props);
        this.state = {
            viewportPhotos: [],
            googleApiKey: "",
            loadedGoogleApiKey: false
        };
        this.googleApiLoadedHandler = this.googleApiLoadedHandler.bind(this);
        this.gmapSearchBoxRef = React.createRef();
        this.panTo.bind(this);
    }

    panTo(photo: Photo, e: any) {
        if (this.map && photo.locLat && photo.locLng) {
            var location = new google.maps.LatLng({ lat: photo.locLat, lng: photo.locLng });
            this.map.setCenter(location);
        }
    }

    googleApiLoadedHandler(map: google.maps.Map) {
        this.map = map;
        this.photoService.getAll([]).then(data => {
            this.allPhotos = data;
            if (this.allPhotos.length > 0) {
                let allBounds = new google.maps.LatLngBounds();
                let markers = this.allPhotos.filter((photo) => photo.locLat && photo.locLng).map((photo, i) => {
                    let markerPosition = new google.maps.LatLng(photo.locLat as number, photo.locLng as number);
                    let marker = new google.maps.Marker({
                        position: markerPosition,
                        label: "",
                    });
                    google.maps.event.addListener(marker, 'click', () => {
                        map.setCenter(marker.getPosition() as google.maps.LatLng);
                        map.setZoom(map.getZoom() as number + 1);
                    });
                    google.maps.event.addListener(marker, 'rightclick', () => {
                        map.setCenter(marker.getPosition() as google.maps.LatLng);
                        map.setZoom((map.getZoom() as number - 1) > 1 ? (map.getZoom() as number - 1) : 1);
                    });
                    allBounds.extend(markerPosition);
                    return marker;
                });
                map.fitBounds(allBounds);
                new MarkerClusterer(map, markers, {
                    imagePath: "/lib/@googlemaps/markerclustererplus/m"
                });
            }
            google.maps.event.addListener(map, 'bounds_changed', (e: any) => {
                let bound = map.getBounds();
                let photosWithLocation = this.allPhotos.filter((photo) => {
                    return photo.locLat && photo.locLng;
                });
                let viewportPhotos = photosWithLocation.filter((photo) => {
                    return bound?.contains({ lat: photo.locLat as number, lng: photo.locLng as number });
                });
                if (viewportPhotos.length === 0)
                    viewportPhotos = photosWithLocation;
                this.setState({
                    viewportPhotos: viewportPhotos
                });
            });
            google.maps.event.addListener(map, 'click', (e: any) => {
                map.setCenter(e.latLng);
                map.setZoom(map.getZoom() as number + 2);
            });
            google.maps.event.addListener(map, 'rightclick', (e: any) => {
                map.setCenter(e.latLng);
                map.setZoom((map.getZoom() as number - 2) > 2 ? (map.getZoom() as number - 2) : 2);
            });
            // Create the search box and link it to the UI element.
            let searchBox = new google.maps.places.SearchBox(this.gmapSearchBoxRef.current as HTMLInputElement);
            searchBox.addListener('places_changed', (e: any) => {
                let places = searchBox.getPlaces();
                if (places && places.length > 0) {
                    let bounds = new google.maps.LatLngBounds();
                    // For each place, get the icon, name and location.

                    places.forEach(function (place) {
                        if (!place.geometry) {
                            console.log("Returned place contains no geometry");
                            return;
                        }

                        if (place.geometry.viewport) {
                            // Only geocodes have viewport.
                            bounds.union(place.geometry.viewport);
                        } else {
                            bounds.extend(place.geometry.location as google.maps.LatLng);
                        }
                    });

                    map.fitBounds(bounds);

                    this.setState({
                        viewportPhotos: this.allPhotos.filter((photo) => {
                            return bounds.contains({ lat: photo.locLat as number, lng: photo.locLng as number });
                        })
                    });
                }
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
                                <div className="image-box rounded" key={photo.id} onClick={(e) => { this.panTo(photo, e) }}>
                                    <img src={photo.filePath} alt={photo.name} />
                                    <a href={"/photo/" + photo.id} target="_blank" rel="noopener noreferrer" className="external-button">
                                        <FontAwesomeIcon icon="external-link-alt" className="edit-link" />
                                    </a>
                                </div>
                            )}
                        </div>
                    </Col>
                    <Col lg={{ size: 8, order: 2 }} className="mb-lg-0 mb-2 order-1">
                        <input id="gmapSearchBox"
                            className="form-control mb-2"
                            type="text"
                            placeholder="Type location here..."
                            ref={this.gmapSearchBoxRef} />
                        <div id="gmap">
                            {this.state.loadedGoogleApiKey && <GoogleMapReact
                                bootstrapURLKeys={{
                                    key: this.state.googleApiKey,
                                    libraries: 'places'
                                }}
                                yesIWantToUseGoogleMapApiInternals
                                onGoogleApiLoaded={({ map }) => this.googleApiLoadedHandler(map)}
                                defaultCenter={{ lat: 3.140853, lng: 101.693207 }} /* KUL */
                                defaultZoom={12}
                                options={{ mapTypeId: 'roadmap' }}
                            />}
                        </div>
                    </Col>
                </Row>
            </div>
        );
    }

    componentDidMount() {
        this.context?.setDisplayMode(DisplayMode.Photo);
        this.context?.setEnableDisplayMode(false);
        this.globalDataService.getGoogleApiKeyEndpoint().then(googleApiKey => {
            this.setState({
                googleApiKey: googleApiKey,
                loadedGoogleApiKey: true
            });
        });
    }

    componentDidUpdate(prevProps: IWorldMapProps, prevState: IWorldMapState) {
        new Tessarray("." + styles["explore-gallery"], ".image-box", {
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
