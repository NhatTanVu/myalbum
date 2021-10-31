import React, { Component } from 'react';
import { Container } from 'reactstrap';
import NavMenu from './NavMenu';
import { Footer } from './Footer';
import { progressBarFetch, setOriginalFetch, ProgressBar } from './ProgressBar';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

// Let react-fetch-progressbar know what the original fetch is.
setOriginalFetch(window.fetch);

/* 
  Now override the fetch with progressBarFetch, so the ProgressBar
  knows how many requests are currently active.
*/
window.fetch = progressBarFetch;

export class Layout extends Component {
    render() {
        return (
            <div>
                <ProgressBar />
                <ToastContainer position="bottom-right" hideProgressBar={true} autoClose={3000} />
                <NavMenu />
                <Container fluid={true} className="pt-3 pb-3 background-dark-lighter text-light" id="container-body">
                    {this.props.children}
                </Container>
                <Footer />
            </div>
        );
    }
}
