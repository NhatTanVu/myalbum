import React, { Component } from 'react';
import { GlobalDataService } from '../services/globalData.service';

interface IFooterProps {
}

interface IFooterState {
    identityUrl: string | undefined,
    photoUrl: string | undefined,
    albumUrl: string | undefined,
    commentUrl: string | undefined
}

export class Footer extends Component<IFooterProps, IFooterState> {
    private globalDataService = new GlobalDataService();

    componentDidMount() {
        this.globalDataService.getConfiguration().then(config => {
            this.setState({
                identityUrl: config.IssuerUri as string,
                photoUrl: config.PhotoApiUrl?.replace("/api/photos", ""),
                albumUrl: config.AlbumApiUrl?.replace("/api/albums", ""),
                commentUrl: config.CommentApiUrl?.replace("/api/comments", "")
            });
        });
    }

    render() {
        return (
            <footer id="footer">
                <div className="container-fluid text-light background-dark-darker box-shadow-top">
                    <div className="row">
                        <div className="col-6">
                            <div className="row">
                                <div className="col-12 pt-3">
                                    <h5 className="border-bottom d-inline-block">Documentation</h5>
                                </div>
                            </div>
                            <div className="row">
                                <ul className="unstyled">
                                    <li><a className="nav-link text-light pt-0 pb-0" href={this.state?.identityUrl + "/swagger"} target="_blank" rel="noopener noreferrer">Identity API</a></li>
                                    <li><a className="nav-link text-light pt-0 pb-0" href={this.state?.photoUrl + "/swagger"} target="_blank" rel="noopener noreferrer">Photo API</a></li>
                                    <li><a className="nav-link text-light pt-0 pb-0" href={this.state?.albumUrl + "/swagger"} target="_blank" rel="noopener noreferrer">Album API</a></li>
                                    <li><a className="nav-link text-light pt-0 pb-0" href={this.state?.commentUrl + "/swagger"} target="_blank" rel="noopener noreferrer">Comment API</a></li>
                                </ul>
                            </div>
                        </div>
                        <div className="col-6">
                            <div className="row">
                                <div className="col-12 pt-3">
                                    <h5 className="border-bottom d-inline-block">About me</h5>
                                </div>
                            </div>
                            <div className="row">
                                <ul className="unstyled">
                                    <li><a className="nav-link text-light pt-0 pb-0" href="https://github.com/NhatTanVu" target="_blank" rel="noopener noreferrer">GitHub</a></li>
                                    <li><a className="nav-link text-light pt-0 pb-0" href="https://www.linkedin.com/in/tanvu/" target="_blank" rel="noopener noreferrer">LinkedIn</a></li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
            </footer>
        );
    }
}
