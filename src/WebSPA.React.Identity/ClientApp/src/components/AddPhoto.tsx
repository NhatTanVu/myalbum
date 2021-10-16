import React, { Component } from 'react';
import { RouteComponentProps } from 'react-router';
import { GlobalDataContext } from '../context/GlobalDataContext';
import { SaveAlbum } from '../models/album';
import { DisplayMode, MAX_FILE_LENGTH } from '../models/globalData';
import { PositionModel, SavePhoto } from '../models/photo';
import { AlbumService } from '../services/album.service';
import { PhotoService } from '../services/photo.service';
import './AddPhoto.css';
import authService from './api-authorization/AuthorizeService';
import GoogleMapReact from 'google-map-react';

interface IAddPhotoProps { }
interface IAddPhotoState {
    photo: SavePhoto,
    albums?: SaveAlbum[]
}

export class AddPhoto extends Component<IAddPhotoProps & RouteComponentProps, IAddPhotoState> {
    static contextType = GlobalDataContext;
    context!: React.ContextType<typeof GlobalDataContext>;

    private albumService = new AlbumService();
    private photoService = new PhotoService();
    private userName = "";
    private fileInput: React.RefObject<HTMLInputElement>;
    private gmapSearchBoxRef: React.RefObject<HTMLInputElement>;
    private position?: PositionModel | null;

    constructor(props: IAddPhotoProps & RouteComponentProps) {
        super(props);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleCancel = this.handleCancel.bind(this);
        this.handleNameChange = this.handleNameChange.bind(this);
        this.handleAlbumChange = this.handleAlbumChange.bind(this);
        this.fileInput = React.createRef();
        this.gmapSearchBoxRef = React.createRef();
        this.googleApiLoadedHandler = this.googleApiLoadedHandler.bind(this);
        this.state = {
            photo: {
                name: ""
            },
            albums: []
        };
    }

    googleApiLoadedHandler(map: google.maps.Map) {
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
                placeMarkerAndPanTo(map.getCenter() as google.maps.LatLng);
            }
        });
        let marker: google.maps.Marker;
        let placeMarkerAndPanTo = (latLng: google.maps.LatLng) => {
            if (marker)
                marker.setMap(null);
            marker = new google.maps.Marker({
                position: latLng,
                map: map
            });
            map.panTo(latLng);
            let center = map.getCenter() as google.maps.LatLng;
            this.position = {
                latitude: latLng.lat(),
                longitude: latLng.lng(),
                centerLat: center.lat(),
                centerLng: center.lng(),
                mapZoom: map.getZoom()
            };

            marker.addListener('click', (e: any) => {
                marker.setMap(null);
                this.position = null;
            });
        };
        google.maps.event.addListener(map, 'click', (e: any) => {
            placeMarkerAndPanTo(e.latLng);
        });
        google.maps.event.addListener(map, 'bounds_changed', (e: any) => {
            let bound = map.getBounds() as google.maps.LatLngBounds;
            searchBox.setBounds(bound);
            let center = map.getCenter() as google.maps.LatLng;
            this.position = {
                ...this.position,
                centerLat: center.lat(),
                centerLng: center.lng(),
                mapZoom: map.getZoom()
            };
        });
    }

    handleNameChange(event: React.FormEvent<HTMLInputElement>) {
        let currentValue = event.currentTarget.value;
        this.setState((prevState, props) => ({
            photo: {
                ...prevState.photo,
                name: currentValue
            }
        }));
    }

    handleAlbumChange(event: React.FormEvent<HTMLSelectElement>) {
        if (event.currentTarget.value !== "") {
            let currentValue = event.currentTarget.value;
            this.setState((prevState, props) => ({
                photo: {
                    ...prevState.photo,
                    album: {
                        id: parseInt(currentValue) ?? null
                    }
                }
            }));
        }
        else {
            this.setState((prevState, props) => ({
                photo: {
                    ...prevState.photo,
                    album: undefined
                }
            }));
        }
    }

    handleSubmit(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();

        if (this.state.photo.name === "") {
            alert("Photo Name required.");
            return;
        }

        let nativeElement = this.fileInput.current;
        if (!nativeElement || !nativeElement.files || nativeElement.files.length == 0) {
            alert("Photo required.");
            return;
        }
        else if (nativeElement.files[0].size > MAX_FILE_LENGTH) {
            alert("File size must not exceed " + MAX_FILE_LENGTH + "MB.");
            return;
        }

        let photoFile = nativeElement.files[0];
        let photo = this.state.photo;
        if (this.position) {
            photo.locLat = this.position.latitude;
            photo.locLng = this.position.longitude;
            photo.centerLat = this.position.centerLat;
            photo.centerLng = this.position.centerLng;
            photo.mapZoom = this.position.mapZoom;
        }

        this.photoService.create(photo, photoFile).then(photo => {
            if (photo) {
                alert("Saved successfully.");
                this.props.history.push('/');
            }
            else {
                alert("Error occurred. Please try again!");
            }
        });
    }

    handleCancel(event: React.MouseEvent<HTMLButtonElement>) {
        this.props.history.push('/');
    }

    componentDidMount() {
        this.context?.setDisplayMode(DisplayMode.Photo);
        this.context?.setEnableDisplayMode(true);
        this.populateState();
    }

    async populateState() {
        let user = await authService.getUser();
        this.userName = user ? user["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"] : null;
        let albumQuery = {
            categoryId: null,
            hasLocation: null,
            authorUserName: this.userName
        };
        this.albumService.getAll(albumQuery).then(albums => {
            this.setState({
                albums: albums as SaveAlbum[]
            });
        });
    }

    render() {
        return (
            <div>
                <h1>Add Photo</h1>
                <form onSubmit={this.handleSubmit}>
                    <div className="form-group">
                        <label htmlFor="photoName"><h5>Name:</h5></label>
                        <input id="photoName" name="photoName" type="text" className="form-control col-lg-8"
                            value={this.state.photo.name} onChange={this.handleNameChange} />
                    </div>
                    <div className="form-group">
                        <label htmlFor="photoFile"><h5>Photo:</h5></label>
                        <div className="row">
                            <div className="col-12">
                                <input type="file" className="form-control-file" id="photoFile" name="photoFile" ref={this.fileInput} />
                                <i>(File size must not exceed 1MB)</i>
                            </div>
                            <div className="col-lg-2 col-form-label-sm">Please choose photos having these objects:</div>
                            <div className="col-lg-6 text-justify col-form-label-sm">
                                <i>aeroplane, bicycle, bird, boat, bottle, bus, car, cat, chair, cow, diningtable, dog, horse, motorbike,
                                    person, pottedplant, sheep, sofa, train, tvmonitor</i>
                            </div>
                        </div>
                    </div>
                    <div className="form-group">
                        <div className="row">
                            <label className="col-lg-2" htmlFor="photoAlbum"><h5>Album:</h5></label>
                            <div className="col-lg-6">
                                <select id="photoAlbum" name="photoAlbum" value={this.state.photo.album?.id || ""} onChange={this.handleAlbumChange}>
                                    <option value=""></option>
                                    {this.state?.albums?.map(album =>
                                        <option value={album.id} key={album.id}>{album.name}</option>
                                    )}
                                </select>
                            </div>
                        </div>
                    </div >
                    <div className="form-group">
                        <div className="row mb-2">
                            <label className="col-lg-2" htmlFor="gmapSearchBox"><h5>Location:</h5></label>
                            <div className="col-lg-6">
                                <input id="gmapSearchBox" className="form-control" type="text"
                                    placeholder="Type location here..." ref={this.gmapSearchBoxRef} />
                            </div>
                        </div>
                        <div className="row">
                            <div className="col-lg-8">
                                <div id="gmap">
                                    <GoogleMapReact
                                        bootstrapURLKeys={{
                                            key: "",
                                            libraries: 'places'
                                        }}
                                        yesIWantToUseGoogleMapApiInternals
                                        onGoogleApiLoaded={({ map }) => this.googleApiLoadedHandler(map)}
                                        defaultCenter={{ lat: 3.140853, lng: 101.693207 }} /* KUL */
                                        defaultZoom={12}
                                        options={{ mapTypeId: 'roadmap' }}
                                    />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div className="col-lg-8">
                        <div className="row">
                            <button className="btn btn-primary mr-3" type="submit">Save</button>
                            <button className="btn btn-secondary" type="button" onClick={this.handleCancel}>Cancel</button>
                        </div>
                    </div>
                </form>
            </div>
        );
    }
}
