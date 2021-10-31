import React, { Component } from 'react';
import { RouteComponentProps } from 'react-router';
import { GlobalDataContext } from '../context/GlobalDataContext';
import { SaveAlbum } from '../models/album';
import { DisplayMode, MAX_FILE_LENGTH } from '../models/globalData';
import { PositionModel, Photo, PhotoCategory } from '../models/photo';
import { AlbumService } from '../services/album.service';
import { PhotoService } from '../services/photo.service';
import './EditPhoto.css';
import authService from './api-authorization/AuthorizeService';
import GoogleMapReact from 'google-map-react';
import { User } from '../models/user';
import Select from 'react-select';
import { toast } from 'react-toastify';
import { GlobalDataService } from '../services/globalData.service';

interface IEditPhotoProps { }
interface IEditPhotoState {
    photo: Photo,
    hasMap: boolean | null,
    albums?: SaveAlbum[],
    googleApiKey: string,
    loadedGoogleApiKey: boolean
}

interface IEditPhotoParams {
    id: string;
}

export class EditPhoto extends Component<IEditPhotoProps & RouteComponentProps<IEditPhotoParams>, IEditPhotoState> {
    static contextType = GlobalDataContext;
    context!: React.ContextType<typeof GlobalDataContext>;

    readonly availableCategories: PhotoCategory[] = [
        {
            id: 1,
            name: "aeroplane"
        },
        {
            id: 2,
            name: "bicycle"
        },
        {
            id: 3,
            name: "bird"
        },
        {
            id: 4,
            name: "boat"
        },
        {
            id: 5,
            name: "bottle"
        },
        {
            id: 6,
            name: "bus"
        },
        {
            id: 7,
            name: "car"
        },
        {
            id: 8,
            name: "cat"
        }, {
            id: 9,
            name: "chair"
        },
        {
            id: 10,
            name: "cow"
        },
        {
            id: 11,
            name: "diningtable"
        },
        {
            id: 12,
            name: "dog"
        },
        {
            id: 13,
            name: "horse"
        },
        {
            id: 14,
            name: "motorbike"
        },
        {
            id: 15,
            name: "person"
        },
        {
            id: 16,
            name: "pottedplant"
        },
        {
            id: 17,
            name: "sheep"
        },
        {
            id: 18,
            name: "sofa"
        },
        {
            id: 19,
            name: "train"
        },
        {
            id: 20,
            name: "tvmonitor"
        }
    ];

    private albumService = new AlbumService();
    private photoService = new PhotoService();
    private globalDataService = new GlobalDataService();
    private photoId: number;
    private userName = "";
    private fileInput: React.RefObject<HTMLInputElement>;
    private gmapSearchBoxRef: React.RefObject<HTMLInputElement>;
    private position?: PositionModel | null;

    constructor(props: IEditPhotoProps & RouteComponentProps<IEditPhotoParams>) {
        super(props);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleCancel = this.handleCancel.bind(this);
        this.handleNameChange = this.handleNameChange.bind(this);
        this.handleAlbumChange = this.handleAlbumChange.bind(this);
        this.handleTagChange = this.handleTagChange.bind(this);
        this.handleDelete = this.handleDelete.bind(this);
        this.fileInput = React.createRef();
        this.gmapSearchBoxRef = React.createRef();
        this.googleApiLoadedHandler = this.googleApiLoadedHandler.bind(this);
        this.state = {
            photo: {
                id: 0,
                name: "",
                filePath: "",
                boundingBoxFilePath: "",
                width: 0,
                height: 0,
                photoCategories: [],
                locLat: -1,
                locLng: -1,
                centerLat: -1,
                centerLng: -1,
                mapZoom: -1,
                comments: [],
                totalComments: 0,
                createdDate: new Date(),
                modifiedDate: new Date(),
                author: new User(),
                album: new SaveAlbum()
            },
            albums: [],
            hasMap: null,
            googleApiKey: "",
            loadedGoogleApiKey: false
        };
        this.photoId = parseInt(this.props.match.params.id);
    }

    googleApiLoadedHandler(map: google.maps.Map) {
        window.setTimeout(() => {
            if (this.state.hasMap !== null) {
                this.initializeMap(map);
            }
            else {
                this.googleApiLoadedHandler(map);
            }
        }, 200);
    }

    initializeMap(map: google.maps.Map) {
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

        if (this.state.hasMap) {
            placeMarkerAndPanTo(new google.maps.LatLng(this.state.photo.locLat as number, this.state.photo.locLng as number));
        }

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
                    album: {}
                }
            }));
        }
    }

    handleTagChange(newValues: any, actionMeta: any) {
        let photoCategories: PhotoCategory[] = newValues as PhotoCategory[];
        this.setState((prevState, props) => ({
            photo: {
                ...prevState.photo,
                photoCategories: photoCategories
            }
        }));
    }

    handleSubmit(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();

        if (this.state.photo.name === "") {
            toast("Photo Name required.");
            return;
        }

        let nativeElement = this.fileInput.current;
        let photoFile = null;
        if (nativeElement && nativeElement.files && nativeElement.files.length > 0) {
            if (nativeElement.files[0].size > MAX_FILE_LENGTH) {
                toast("File size must not exceed " + MAX_FILE_LENGTH / (1024 * 1024) + "MB.");
                return;
            }
            else
                photoFile = nativeElement.files[0];
        }

        let photo = this.state.photo;
        if (this.position) {
            photo.locLat = this.position.latitude as number;
            photo.locLng = this.position.longitude as number;
            photo.centerLat = this.position.centerLat as number;
            photo.centerLng = this.position.centerLng as number;
            photo.mapZoom = this.position.mapZoom as number;
        }
        else {
            photo.locLat = null;
            photo.locLng = null;
            photo.centerLat = null;
            photo.centerLng = null;
            photo.mapZoom = null;
        }

        this.photoService.save(photo, photoFile).then(photo => {
            if (photo) {
                toast("Saved successfully.");
                this.props.history.push('/');
            }
            else {
                toast("Error occurred. Please try again!");
            }
        });
    }

    handleCancel(event: React.MouseEvent<HTMLButtonElement>) {
        this.props.history.push('/');
    }

    handleDelete(event: React.MouseEvent<HTMLButtonElement>) {
        this.photoService.delete(this.state.photo?.id as number).then(res => {
            if (res) {
                toast("Deleted successfully.");
                this.props.history.push('/');
            }
            else {
                toast("Error occurred. Please try again!");
            }
        });
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
        this.photoService.get(this.photoId).then(photo => {
            if (photo) {
                this.setState({
                    photo: photo,
                    hasMap: (photo.locLat != null) && (photo.locLng != null)
                });
            }
            else {
                this.props.history.push('/');
            }
        });
        this.globalDataService.getGoogleApiKeyEndpoint().then(googleApiKey => {
            this.setState({
                googleApiKey: googleApiKey,
                loadedGoogleApiKey: true
            });
        });
    }

    render() {
        return (
            <div>
                <h1>Edit Photo</h1>
                <form onSubmit={this.handleSubmit}>
                    <div className="form-group">
                        <h5>Name:</h5>
                        <input id="photoName" name="photoName" type="text" className="form-control col-lg-8"
                            value={this.state.photo.name} onChange={this.handleNameChange} />
                    </div>
                    <div className="form-group">
                        <h5>Photo:</h5>
                        <div className="row mb-2">
                            <div className="col-lg-6 photo-container">
                                <img alt={this.state.photo.name} src={this.state.photo.filePath} className="img-thumbnail" />
                            </div>
                        </div>
                        <div className="row mb-2">
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
                        <div className="row mb-2">
                            <h5 className="col-lg-2">Album:</h5>
                            <div className="col-lg-6">
                                <select className="form-control" id="photoAlbum" name="photoAlbum" value={this.state.photo.album?.id || ""} onChange={this.handleAlbumChange}>
                                    <option value=""></option>
                                    {this.state?.albums?.map(album =>
                                        <option value={album.id} key={album.id}>{album.name}</option>
                                    )}
                                </select>
                            </div>
                        </div>
                        <div className="row">
                            <h5 className="col-lg-2">Tags:</h5>
                            <div className="col-lg-6">
                                <Select id="photoTag" name="photoTag"
                                    className="basic-multi-select"
                                    classNamePrefix="select"
                                    isMulti
                                    value={this.state.photo.photoCategories}
                                    onChange={this.handleTagChange}
                                    options={this.availableCategories}
                                    getOptionLabel={option => option.name}
                                    getOptionValue={option => option.id.toString()} />
                            </div>
                        </div>
                    </div>
                    <div className="form-group">
                        <div className="row mb-2">
                            <h5 className="col-lg-2">Location:</h5>
                            <div className="col-lg-6">
                                <input id="gmapSearchBox" className="form-control" type="text"
                                    placeholder="Type location here..." ref={this.gmapSearchBoxRef} />
                            </div>
                        </div>
                        <div className="row">
                            <div className="col-lg-8">
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
                            </div>
                        </div>
                    </div>
                    <div className="col-lg-8">
                        <div className="row">
                            <button className="btn btn-secondary mr-3" type="button" onClick={this.handleCancel}>Back</button>
                            <button className="btn btn-primary mr-3" type="submit">Save</button>
                            <button className="btn btn-danger" type="button" onClick={this.handleDelete}>Delete</button>
                        </div>
                    </div>
                </form>
            </div>
        );
    }
}
