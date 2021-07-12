import React, { Component } from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';
import '../toggle-switch.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { DisplayMode } from '../models/globalData';
import { GlobalDataContext } from '../context/GlobalDataContext';

interface INavMenuProps {
}
interface INavMenuState {
    collapsed: boolean
}

export class NavMenu extends Component<INavMenuProps, INavMenuState> {
    static contextType = GlobalDataContext;
    context!: React.ContextType<typeof GlobalDataContext>;

    constructor(props: INavMenuProps) {
        super(props);

        this.toggleNavbar = this.toggleNavbar.bind(this);
        this.state = {
            collapsed: true
        };
    }

    toggleNavbar() {
        this.setState({
            collapsed: !this.state.collapsed
        });
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
                                <div>
                                    <input id="photo_mobile" name="displayMode_mobile" type="radio" value="photo"
                                        checked={this.context?.globalData.displayMode == DisplayMode.Photo}
                                        onClick={(e) => this.context?.setDisplayMode(DisplayMode.Photo)} />
                                    <label htmlFor="photo_mobile">Photo</label>
                                    <input id="album_mobile" name="displayMode_mobile" type="radio" value="album" 
                                        checked={this.context?.globalData.displayMode == DisplayMode.Album}
                                        onClick={(e) => this.context?.setDisplayMode(DisplayMode.Album)} />
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
                                    <NavLink tag={Link} className="text-light" to="/worldmap"> {/* TODO: Fix later */}
                                        <FontAwesomeIcon icon="globe" /> World Map
                                    </NavLink>
                                </NavItem>
                                <NavItem>
                                    <NavLink tag={Link} className="text-light" to="/photo/new"> {/* TODO: Fix later */}
                                        <FontAwesomeIcon icon="plus" /> Add
                                    </NavLink>
                                </NavItem>
                            </ul>
                            <span className="separator"></span>
                            <div className="rounded switch-toggle alert display-mode">
                                {/* <div className="overlay"></div> */}
                                <div>
                                    <input id="photo" name="displayMode" type="radio" value="photo"
                                        checked={this.context?.globalData.displayMode == DisplayMode.Photo}
                                        onClick={(e) => this.context?.setDisplayMode(DisplayMode.Photo)} />
                                    <label htmlFor="photo">Photo</label>
                                    <input id="album" name="displayMode" type="radio" value="album"
                                        checked={this.context?.globalData.displayMode == DisplayMode.Album}
                                        onClick={(e) => this.context?.setDisplayMode(DisplayMode.Album)} />
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
