import React, { Component } from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

export class NavMenu extends Component {
  static displayName = NavMenu.name;

  constructor (props) {
    super(props);

    this.toggleNavbar = this.toggleNavbar.bind(this);
    this.state = {
      collapsed: true
    };
  }

  toggleNavbar () {
    this.setState({
      collapsed: !this.state.collapsed
    });
  }

  render () {
    return (
      <header>
        <Navbar className="navbar navbar-expand-md navbar-toggleable-md navbar-dark box-shadow-bottom background-dark-darker" light>
          <Container fluid={true}>
            <NavbarBrand tag={Link} to="/" className="navbar-brand text-light">
              <img className="rounded" src="/logo.jpg" alt="logo.jpg" height="40px" width="40px"/>
              <span> My Album</span>
            </NavbarBrand>
              <div className="d-inline-flex">
                <div className="rounded switch-toggle alert display-mode mobile">
                <div className="d-none"> {/* TODO: Fix later */}
                  <input id="photo1" name="displayMode1" type="radio" value="photo"/>
                  <label htmlFor="photo1">Photo</label>
                  <input id="album1" name="displayMode1" type="radio" value="album"/>
                  <label htmlFor="album1">Album</label>
                  <a className="btn btn-primary"></a>
                </div>
              </div>
              <span className="separator mobile"></span>
              <NavbarToggler onClick={this.toggleNavbar} className="mr-2 text-light" />
            </div>
            <Collapse className="d-md-inline-flex flex-md-row-reverse" isOpen={!this.state.collapsed} navbar>
              <ul className="navbar-nav flex-grow">
                <NavItem>
                  <NavLink tag={Link} className="text-light" to="/">
                    <FontAwesomeIcon icon="torii-gate" /> Explore
                  </NavLink>
                </NavItem>
                <NavItem>
                  <NavLink tag={Link} className="text-light" to="/worldmap">
                    <FontAwesomeIcon icon="globe" /> World Map
                  </NavLink>
                </NavItem>
                <NavItem>
                  <NavLink tag={Link} className="text-light" to="/photo/new">
                    <FontAwesomeIcon icon="plus" /> Add
                  </NavLink>
                </NavItem>
              </ul>
            </Collapse>
          </Container>
        </Navbar>
      </header>
    );
  }
}
