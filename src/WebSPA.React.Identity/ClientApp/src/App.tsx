import React, { Component } from 'react';
import { Route, Switch } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { AddPhoto } from './components/AddPhoto';
import { WorldMap } from './components/WorldMap';
import { ViewPhoto } from './components/ViewPhoto';
import { library } from '@fortawesome/fontawesome-svg-core'
import { faToriiGate, faGlobe, faPlus, faComments, faExternalLinkAlt, faPencilAlt, faCommentMedical } from '@fortawesome/free-solid-svg-icons';

import './styles.css'

library.add(faToriiGate, faGlobe, faPlus, faComments, faExternalLinkAlt, faPencilAlt, faCommentMedical)

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Switch>
          <Route exact path='/' component={Home} />
          <Route path='/worldmap' component={WorldMap} />
          <Route path='/photo/new' component={AddPhoto} />
          <Route path='/photo/:id' component={ViewPhoto} />
        </Switch>
      </Layout>
    );
  }
}
