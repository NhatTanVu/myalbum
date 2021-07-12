import React, { Component } from 'react';
import { Route, Switch } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { ExploreAlbum } from './components/ExploreAlbum';
import { AddPhoto } from './components/AddPhoto';
import { WorldMap } from './components/WorldMap';
import { ViewPhoto } from './components/ViewPhoto';
import { ViewAlbum } from './components/ViewAlbum';
import { library } from '@fortawesome/fontawesome-svg-core'
import { faToriiGate, faGlobe, faPlus, faComments, faExternalLinkAlt, faPencilAlt, faCommentMedical, faImages } from '@fortawesome/free-solid-svg-icons';
import { GlobalDataContextProvider } from './context/GlobalDataContextProvider';

import './styles.css'

library.add(faToriiGate, faGlobe, faPlus, faComments, faExternalLinkAlt, faPencilAlt, faCommentMedical, faImages)

export default class App extends Component {
    static displayName = App.name;

    render() {
        return (
            <GlobalDataContextProvider>
                <Layout>
                    <Switch>
                        <Route exact path='/' component={Home} />
                        <Route exact path='/album' component={ExploreAlbum} />
                        <Route path='/worldmap' component={WorldMap} />
                        <Route path='/photo/new' component={AddPhoto} />
                        <Route path='/photo/:id' component={ViewPhoto} />
                        <Route path='/album/:id' component={ViewAlbum} />
                    </Switch>
                </Layout>
            </GlobalDataContextProvider>
        );
    }
}
