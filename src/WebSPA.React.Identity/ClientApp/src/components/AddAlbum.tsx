import React, { Component } from 'react';
import { RouteComponentProps } from 'react-router';
import { GlobalDataContext } from '../context/GlobalDataContext';
import { SaveAlbum } from '../models/album';
import { DisplayMode } from '../models/globalData';
import { AlbumService } from '../services/album.service';

interface IAddAlbumProps { }
interface IAddAlbumState {
    album: SaveAlbum
}

export class AddAlbum extends Component<IAddAlbumProps & RouteComponentProps, IAddAlbumState> {
    static contextType = GlobalDataContext;
    context!: React.ContextType<typeof GlobalDataContext>;

    private albumService = new AlbumService();

    constructor(props: IAddAlbumProps & RouteComponentProps) {
        super(props);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.handleCancel = this.handleCancel.bind(this);
        this.state = {
            album: {
                name: ""
            }
        };
    }

    handleSubmit(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();

        if (this.state.album.name === "") {
            alert("Album Name required.");
            return;
        }

        this.albumService.create(this.state.album).then(album => {
            if (album) {
                alert("Created successfully.");
                this.props.history.push('/album');
            }
            else {
                alert("Error occurred. Please try again!");
            }
        });
    }

    handleChange(event: React.FormEvent<HTMLInputElement>) {
        this.setState({
            album: {
                name: event.currentTarget.value
            }
        });
    }

    handleCancel(event: React.MouseEvent<HTMLButtonElement>) {
        this.props.history.push('/album');
    }

    componentDidMount() {
        this.context?.setDisplayMode(DisplayMode.Album);
        this.context?.setEnableDisplayMode(true);
    }

    render() {
        return (
            <div>
                <h1>Add Album</h1>
                <form onSubmit={this.handleSubmit}>
                    <div className="form-group">
                        <label htmlFor="albumName"><h5>Name:</h5></label>
                        <input id="albumName" name="albumName" type="text" className="form-control col-lg-8"
                            value={this.state.album.name} onChange={this.handleChange} />
                    </div>
                    <div className="col-lg-8">
                        <div className="row mb-3">
                            <button className="btn btn-primary mr-3" type="submit">Save</button>
                            <button className="btn btn-secondary" type="button" onClick={this.handleCancel}>Cancel</button>
                        </div>
                    </div>
                </form>
            </div>
        );
    }
}
