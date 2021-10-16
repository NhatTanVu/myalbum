import React, { Component } from 'react';
import { Row, Col } from 'reactstrap';
import { Photo } from '../models/photo';
import { User } from "../models/user";
import { SaveAlbum } from '../models/album';
import { PhotoService } from '../services/photo.service';
import './ViewPhoto.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import GoogleMapReact from 'google-map-react';
import { RouteComponentProps } from 'react-router';
import { Link } from 'react-router-dom';
import { ReplyList } from './ReplyList';
import { ReplyForm } from './ReplyForm';
import { DisplayMode } from '../models/globalData';
import { GlobalDataContext } from '../context/GlobalDataContext';
import authService from './api-authorization/AuthorizeService';

interface IViewPhotoProps { }
interface IViewPhotoState {
    photo: Photo,
    isShownBoundingBox: boolean,
    hasMap: boolean | null,
    userName: string,
    isEditable: boolean,
    flipContainerWidth: number,
    maxHeight: number,
    maxWidth: number,
    gmapHeight: number
}

interface IViewPhotoParams {
    id: string;
}

export class ViewPhoto extends Component<IViewPhotoProps & RouteComponentProps<IViewPhotoParams>, IViewPhotoState> {
    static contextType = GlobalDataContext;
    context!: React.ContextType<typeof GlobalDataContext>;

    private photoService = new PhotoService();
    private photoId: number;
    private photoContainerRef: React.RefObject<HTMLDivElement>;
    private imageElementRef: React.RefObject<HTMLImageElement>;

    constructor(props: IViewPhotoProps & RouteComponentProps<IViewPhotoParams>) {
        super(props);
        let windowWidth = window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth;
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
            isShownBoundingBox: false,
            hasMap: null,
            userName: "",
            isEditable: false,
            flipContainerWidth: 0,
            maxHeight: 0,
            maxWidth: 0,
            gmapHeight: (windowWidth > 575.98) ? 400 : 300
        };
        this.googleApiLoadedHandler = this.googleApiLoadedHandler.bind(this);
        this.initializeMap = this.initializeMap.bind(this);
        this.handleResize = this.handleResize.bind(this);
        this.calcPhotoSize = this.calcPhotoSize.bind(this);
        this.photoId = parseInt(this.props.match.params.id);
        this.photoContainerRef = React.createRef();
        this.imageElementRef = React.createRef();
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

    handleResize(event: any) {
        this.calcPhotoSize();
    }

    calcPhotoSize() {
        window.setTimeout(() => {
            let element = this.photoContainerRef.current as HTMLDivElement;
            let computedStyle = window.getComputedStyle(element);
            let elementHeight = element.clientHeight;  // height with padding
            let elementWidth = element.clientWidth;   // width with padding
            elementHeight -= parseFloat(computedStyle.paddingTop) + parseFloat(computedStyle.paddingBottom);
            elementWidth -= parseFloat(computedStyle.paddingLeft) + parseFloat(computedStyle.paddingRight);
            let maxWidth = elementWidth;
            let maxHeight = elementHeight;
            if (maxHeight > 10) {
                if (this.imageElementRef.current?.clientWidth && this.imageElementRef.current?.clientWidth > 0 && this.state.maxWidth === maxWidth && this.state.maxHeight === maxHeight) {
                    this.setState({
                        flipContainerWidth: this.imageElementRef.current?.clientWidth
                    });
                }
                else {
                    this.calcPhotoSize();
                }
                this.setState({
                    maxHeight: maxHeight,
                    maxWidth: maxWidth
                });
            }
            else {
                this.calcPhotoSize();
            }
        }, 200);
    }

    initializeMap(map: google.maps.Map) {
        if (this.state.hasMap === true) {
            var marker = new google.maps.Marker({
                position: new google.maps.LatLng(this.state.photo.locLat, this.state.photo.locLng),
                map: map
            });
            map.panTo(marker.getPosition() as google.maps.LatLng);
        }
        else {
            this.setState({ gmapHeight: 0 });
        }
    }

    componentDidMount() {
        window.addEventListener("resize", this.handleResize);
        this.context?.setDisplayMode(DisplayMode.Photo);
        this.context?.setEnableDisplayMode(false);
        this.populateState();
    }

    async populateState() {
        let user = await authService.getUser();
        let userName = user ? user["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"] : null;
        this.photoService.get(this.photoId).then(photo => {
            if (photo) {
                this.setState({
                    photo: photo,
                    userName: userName,
                    isEditable: photo.author && photo.author.userName === userName,
                    hasMap: (photo.locLat != null) && (photo.locLng != null)
                });
                this.calcPhotoSize();
            }
            else {
                this.props.history.push('/');
            }
        });
    }

    componentWillUnmount() {
        window.removeEventListener("resize", this.handleResize);
    }

    render() {
        return (
            <div id="view-photo">
                <Row>
                    <Col lg={{ size: 6 }}>
                        <h3 className="header-container">
                            <span className="header-text" title={this.state.photo.name}>{this.state.photo.name}</span>
                            {this.state.isEditable &&
                                    <a className="btn btn-secondary header-button" href={"/photo/edit/" + this.state.photo.id} title="Edit Photo">
                                        <FontAwesomeIcon icon="pencil-alt" />
                                    </a>
                            }
                        </h3>
                        <hr />
                    </Col>
                </Row>
                <Row>
                    <div className="col-lg-6 photo-container" ref={this.photoContainerRef}>
                        {this.state.maxWidth > 0 &&
                            <div className={"flip-container mb-2 " + (this.state.isShownBoundingBox ? 'flip' : '')}
                                onClick={(e) => { this.setState((s, p) => { return { isShownBoundingBox: !s.isShownBoundingBox }; }) }}
                                style={{ width: this.state.flipContainerWidth }}>
                                <div className="flipper">
                                    <div className="front">
                                        {/* front content */}
                                        <img alt={this.state.photo.name} src={this.state.photo.filePath} className="img-thumbnail" ref={this.imageElementRef}
                                            style={{ maxWidth: this.state.maxWidth, maxHeight: this.state.maxHeight }} />
                                    </div>
                                    <div className="back">
                                        {/* back content */}
                                        <img alt={this.state.photo.name} src={this.state.photo.boundingBoxFilePath} className="img-thumbnail"
                                            style={{ maxWidth: this.state.maxWidth, maxHeight: this.state.maxHeight }} />
                                    </div>
                                </div>
                            </div>
                        }
                    </div>
                    <Col lg={{ size: 6 }}>
                        <Row className={!this.state.photo.album ? "mb-5" : ""}>
                            <Col>
                                <h4>Album</h4>
                                <hr />
                            </Col>
                        </Row>
                        {this.state.photo.album &&
                            <Row>
                                <Col>
                                    {this.state.photo.album.name &&
                                        <Link className="btn btn-outline-secondary mr-2 mb-2" to={"/album/" + this.state.photo.album.id}>{this.state.photo.album.name}</Link>
                                    }
                                </Col>
                            </Row>
                        }
                        <Row className={(!this.state.photo.photoCategories || this.state.photo.photoCategories.length === 0) ? "mb-5" : ""}>
                            <Col>
                                <h4>Tags</h4>
                                <hr />
                            </Col>
                        </Row>
                        {this.state.photo.photoCategories && this.state.photo.photoCategories.length > 0 &&
                            <Row>
                                <Col>
                                    {this.state.photo.photoCategories.map(photoCategories =>
                                        <Link key={photoCategories.id} className="btn btn-outline-secondary mr-2 mb-2" to={"/?catId=" + photoCategories.id}>{photoCategories.name}</Link>
                                    )}
                                </Col>
                            </Row>
                        }
                        <Row className={!this.state.hasMap ? "mb-5" : ""}>
                            <div className="col">
                                <h4>Location</h4>
                                <hr />
                            </div>
                        </Row>
                        <Row>
                            <Col>
                                <div id="gmap-edit" style={{ height: this.state.gmapHeight }}>
                                    <GoogleMapReact
                                        bootstrapURLKeys={{
                                            key: "",
                                            libraries: 'places'
                                        }}
                                        yesIWantToUseGoogleMapApiInternals
                                        defaultCenter={{ lat: 3.140853, lng: 101.693207 }} /* KUL */
                                        defaultZoom={12}
                                        onGoogleApiLoaded={({ map }) => this.googleApiLoadedHandler(map)}
                                        options={{ mapTypeId: 'roadmap' }}
                                    />
                                    {!this.state.hasMap &&
                                        <div id="gmap-edit-overlay" className="background-dark-lighter" style={{ top: 0, left: 0, width: "100%", height: "100%", zIndex: 1000, position: "absolute" }}>
                                        </div>
                                    }
                                </div>
                            </Col>
                        </Row>
                    </Col>
                </Row>
                <Row>
                    <Col lg={{ size: 6 }}>
                        <div>
                            <h4>New Comment</h4>
                            <hr />
                        </div>
                        <div>
                            <ReplyForm></ReplyForm>
                        </div>
                    </Col>
                </Row>
                {this.state.photo.comments && this.state.photo.comments.length > 0 &&
                    <Row>
                        <Col lg={{ size: 6 }}>
                            <div>
                                <h4>All Comments</h4>
                                <hr />
                            </div>
                            <div>
                                <ReplyList replies={this.state.photo.comments} userName={this.state.userName}></ReplyList>
                            </div>
                         </Col>
                    </Row>
                }
            </div>
        );
    }
}
