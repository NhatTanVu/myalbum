import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { FetchData } from './components/FetchData';
import { Counter } from './components/Counter';
import { library } from '@fortawesome/fontawesome-svg-core'
import { faToriiGate, faGlobe, faPlus, faComments } from '@fortawesome/free-solid-svg-icons';

import './styles.css'

library.add(faToriiGate, faGlobe, faPlus, faComments)

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/counter' component={Counter} />
        <Route path='/fetch-data' component={FetchData} />
      </Layout>
    );
  }
}
