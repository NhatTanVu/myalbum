import React, { Component } from 'react';
import { Container } from 'reactstrap';
import NavMenu from './NavMenu';
import { Footer } from './Footer';

export class Layout extends Component {
    render() {
        return (
            <div>
                <NavMenu />
                <Container fluid={true} className="pt-3 pb-3 background-dark-lighter text-light" id="container-body">
                    {this.props.children}
                </Container>
                <Footer />
            </div>
        );
    }
}
