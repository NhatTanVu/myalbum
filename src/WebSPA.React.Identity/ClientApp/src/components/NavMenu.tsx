import React, { Component } from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';
import '../toggle-switch.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { DisplayMode } from '../models/globalData';
import { GlobalDataContext } from '../context/GlobalDataContext';
import { RouteComponentProps, withRouter } from 'react-router';

interface INavMenuProps {
}
interface INavMenuState {
    collapsed: boolean
}

class NavMenu extends Component<INavMenuProps & RouteComponentProps, INavMenuState> {
    static contextType = GlobalDataContext;
    context!: React.ContextType<typeof GlobalDataContext>;

    constructor(props: INavMenuProps & RouteComponentProps) {
        super(props);

        this.toggleNavbar = this.toggleNavbar.bind(this);
        this.changeDisplayMode = this.changeDisplayMode.bind(this);
        this.state = {
            collapsed: true
        };
    }

    toggleNavbar() {
        this.setState({
            collapsed: !this.state.collapsed
        });
    }

    changeDisplayMode(displayMode: DisplayMode) {
        let currentPath = window.location.pathname;

        switch (currentPath) {
            case "/":
                if (displayMode == DisplayMode.Album) {
                    this.props.history.push('/album');
                }
                else {
                    this.props.history.push('/');
                }
                break;
            case "/photo/new":
                if (displayMode == DisplayMode.Album) {
                    this.props.history.push('/album/new');
                }
                break;
            case "/album":
                if (displayMode == DisplayMode.Photo) {
                    this.props.history.push('/');
                }
                break;
            case "/album/new":
                if (displayMode == DisplayMode.Photo) {
                    this.props.history.push('/photo/new');
                }
                break;
        }

        console.log("currentPath = " + currentPath);
        console.log("displayMode = " + displayMode);
    }

    render() {
        return (
            <header>
                <Navbar className="navbar navbar-expand-md navbar-toggleable-md navbar-dark box-shadow-bottom background-dark-darker" light>
                    <Container fluid={true}>
                        <NavbarBrand tag={Link} to="/" className="navbar-brand text-light">
                            <img className="rounded" src="/logo.jpg" alt="logo.jpg" height="40px" width="40px" />
                            <span> My Album</span>
                        </NavbarBrand>
                        <div className="d-inline-flex">
                            <div className="rounded switch-toggle alert display-mode mobile">
                                {!this.context?.globalData.enableDisplayMode && <div className="overlay"></div>}
                                <div>
                                    <input id="photo_mobile" name="displayMode_mobile" type="radio" value="photo"
                                        checked={this.context?.globalData.displayMode === DisplayMode.Photo}
                                        disabled={!this.context?.globalData.enableDisplayMode}
                                        onChange={(e) => this.changeDisplayMode(DisplayMode.Photo)} />
                                    <label htmlFor="photo_mobile">Photo</label>
                                    <input id="album_mobile" name="displayMode_mobile" type="radio" value="album" 
                                        checked={this.context?.globalData.displayMode === DisplayMode.Album}
                                        disabled={!this.context?.globalData.enableDisplayMode}
                                        onChange={(e) => this.changeDisplayMode(DisplayMode.Album)} />
                                    <label htmlFor="album_mobile">Album</label>
                                    <a className="btn btn-primary"></a>
                                </div>
                            </div>
                            <span className="separator mobile"></span>
                            <NavbarToggler onClick={this.toggleNavbar} className="mr-2 text-light" />
                        </div>
                        <Collapse className="d-md-inline-flex flex-md-row-reverse" isOpen={!this.state.collapsed} navbar>
                            <ul className="navbar-nav flex-grow">
                                <NavItem>
                                    {this.context?.globalData.displayMode === DisplayMode.Photo &&
                                        <NavLink tag={Link} className="text-light" to="/">
                                            <FontAwesomeIcon icon="torii-gate" /> Explore
                                        </NavLink>
                                    }
                                    {this.context?.globalData.displayMode === DisplayMode.Album &&
                                        <NavLink tag={Link} className="text-light" to="/album">
                                            <FontAwesomeIcon icon="torii-gate" /> Explore
                                        </NavLink>
                                    }
                                </NavItem>
                                <NavItem>
                                    {this.context?.globalData.displayMode === DisplayMode.Photo &&
                                        <NavLink tag={Link} className="text-light" to="/worldmap">
                                            <FontAwesomeIcon icon="globe" /> World Map
                                        </NavLink>
                                    }
                                </NavItem>
                                <NavItem>
                                    {this.context?.globalData.displayMode === DisplayMode.Photo &&
                                        <NavLink tag={Link} className="text-light" to="/photo/new">
                                            <FontAwesomeIcon icon="plus" /> Add
                                        </NavLink>
                                    }
                                    {this.context?.globalData.displayMode === DisplayMode.Album &&
                                        <NavLink tag={Link} className="text-light" to="/album/new">
                                            <FontAwesomeIcon icon="plus" /> Add
                                        </NavLink>
                                    }
                                </NavItem>
                            </ul>
                            <span className="separator"></span>
                            <div className="rounded switch-toggle alert display-mode">
                                {!this.context?.globalData.enableDisplayMode && <div className="overlay"></div>}
                                <div>
                                    <input id="photo" name="displayMode" type="radio" value="photo"
                                        checked={this.context?.globalData.displayMode === DisplayMode.Photo}
                                        disabled={!this.context?.globalData.enableDisplayMode}
                                        onChange={(e) => this.changeDisplayMode(DisplayMode.Photo)} />
                                    <label htmlFor="photo">Photo</label>
                                    <input id="album" name="displayMode" type="radio" value="album"
                                        checked={this.context?.globalData.displayMode === DisplayMode.Album}
                                        disabled={!this.context?.globalData.enableDisplayMode}
                                        onChange={(e) => this.changeDisplayMode(DisplayMode.Album)} />
                                    <label htmlFor="album">Album</label>
                                    <a className="btn btn-primary"></a>
                                </div>
                            </div>
                        </Collapse>
                    </Container>
                </Navbar>
            </header>
        );
    }
}

export default withRouter(NavMenu);