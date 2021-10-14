import React, { Component } from 'react';
import { Route, Switch } from 'react-router';
import { Layout } from './components/Layout';
import { ExplorePhotos } from './components/ExplorePhotos';
import { ExploreAlbums } from './components/ExploreAlbums';
import { AddPhoto } from './components/AddPhoto';
import { AddAlbum } from './components/AddAlbum';
import { WorldMap } from './components/WorldMap';
import { ViewPhoto } from './components/ViewPhoto';
import { ViewAlbum } from './components/ViewAlbum';
import { library } from '@fortawesome/fontawesome-svg-core'
import {
    faToriiGate, faGlobe, faPlus, faComments, faExternalLinkAlt, faPencilAlt, faCommentMedical,
    faImages, faUserPlus, faSignInAlt, faSignOutAlt, faUserCog, faCheck, faTimes
} from '@fortawesome/free-solid-svg-icons';
import { GlobalDataContextProvider } from './context/GlobalDataContextProvider';
import AuthorizeRoute from './components/api-authorization/AuthorizeRoute';
import ApiAuthorizationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import { ApplicationPaths } from './components/api-authorization/ApiAuthorizationConstants';

import './styles.css'

library.add(faToriiGate, faGlobe, faPlus, faComments, faExternalLinkAlt,
    faPencilAlt, faCommentMedical, faImages, faUserPlus, faSignInAlt, faSignOutAlt, faUserCog, faCheck, faTimes)

export default class App extends Component {
    static displayName = App.name;

    render() {
        return (
            <GlobalDataContextProvider>
                <Layout>
                    <Switch>
                        <Route exact path='/' component={ExplorePhotos} />
                        <Route exact path='/album' component={ExploreAlbums} />
                        <Route path='/worldmap' component={WorldMap} />
                        <AuthorizeRoute path='/photo/new' component={AddPhoto} />
                        <Route path='/photo/:id' component={ViewPhoto} />
                        <AuthorizeRoute path='/album/new' component={AddAlbum} />
                        <Route path='/album/:id' component={ViewAlbum} />
                        <Route path={ApplicationPaths.ApiAuthorizationPrefix} component={ApiAuthorizationRoutes} />
                    </Switch>
                </Layout>
            </GlobalDataContextProvider>
        );
    }
}
