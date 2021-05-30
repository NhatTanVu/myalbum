import React, { Component } from 'react';
import { Container } from 'reactstrap';
import { NavMenu } from './NavMenu';

export class Layout extends Component {
  static displayName = Layout.name;

  render () {
    return (
      <div>
        <NavMenu />
        <Container fluid={true} className="pt-3 pb-3 background-dark-lighter text-light">
          {this.props.children}
        </Container>
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
                    <li><a className="nav-link text-light pt-0 pb-0" href="/swagger" target="_blank">Identity API</a></li>
                    <li><a className="nav-link text-light pt-0 pb-0" href="/swagger" target="_blank">Photo API</a></li>
                    <li><a className="nav-link text-light pt-0 pb-0" href="/swagger" target="_blank">Album API</a></li>
                    <li><a className="nav-link text-light pt-0 pb-0" href="/swagger" target="_blank">Comment API</a></li>
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
                    <li><a className="nav-link text-light pt-0 pb-0" href="https://github.com/NhatTanVu" target="_blank">GitHub</a></li>
                    <li><a className="nav-link text-light pt-0 pb-0" href="https://www.linkedin.com/in/tanvu/" target="_blank">LinkedIn</a></li>
                  </ul>
                </div>
              </div>
            </div>
          </div>
        </footer>
      </div>
    );
  }
}
