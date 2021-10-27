import 'bootstrap/dist/css/bootstrap.css';
import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter } from 'react-router-dom';
import App from './App';
//import registerServiceWorker from './registerServiceWorker';
import { unregister } from './registerServiceWorker.js';

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
const rootElement = document.getElementById('root');

ReactDOM.render(
  <BrowserRouter basename={baseUrl}>
    <App />
  </BrowserRouter>,
  rootElement);

// https://stackoverflow.com/questions/69034791/issue-publishing-asp-net-core-with-react-using-identity
// Problem for me was that the service-worker would grab the request to authentication 
// and instead of redirecting to the appropriate page(login or register) it would just give back a stock 200.
//registerServiceWorker();
unregister();