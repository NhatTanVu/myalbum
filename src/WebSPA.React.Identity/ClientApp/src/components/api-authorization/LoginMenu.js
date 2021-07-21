import React, { Component, Fragment } from 'react';
import { NavItem, NavLink } from 'reactstrap';
import { NavLink as RRNavLink } from 'react-router-dom';
import authService from './AuthorizeService';
import { ApplicationPaths } from './ApiAuthorizationConstants';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

export class LoginMenu extends Component {
    constructor(props) {
        super(props);

        this.state = {
            isAuthenticated: false,
            userName: null
        };
    }

    componentDidMount() {
        this._subscription = authService.subscribe(() => this.populateState());
        this.populateState();
    }

    componentWillUnmount() {
        authService.unsubscribe(this._subscription);
    }

    async populateState() {
        const [isAuthenticated, user] = await Promise.all([authService.isAuthenticated(), authService.getUser()])
        this.setState({
            isAuthenticated,
            userName: user && user["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"]
        });
    }

    render() {
        const { isAuthenticated, userName } = this.state;
        if (!isAuthenticated) {
            const registerPath = `${ApplicationPaths.Register}`;
            const loginPath = `${ApplicationPaths.Login}`;
            return this.anonymousView(registerPath, loginPath);
        } else {
            const profilePath = `${ApplicationPaths.Profile}`;
            const logoutPath = { pathname: `${ApplicationPaths.LogOut}`, state: { local: true } };
            return this.authenticatedView(userName, profilePath, logoutPath);
        }
    }

    authenticatedView(userName, profilePath, logoutPath) {
        return (
            <Fragment>
                <NavItem>
                    <NavLink tag={RRNavLink} className="text-light" to={profilePath} activeClassName="link-active">
                        {/*Hello {userName}*/}
                        <FontAwesomeIcon icon="user-cog" /> Manage
                    </NavLink>
                </NavItem>
                <NavItem>
                    <NavLink tag={RRNavLink} className="text-light" to={logoutPath} activeClassName="link-active">
                        <FontAwesomeIcon icon="sign-out-alt" /> Logout
                    </NavLink>
                </NavItem>
            </Fragment>
        );

    }

    anonymousView(registerPath, loginPath) {
        return (
            <Fragment>
                <NavItem>
                    <NavLink tag={RRNavLink} className="text-light" to={registerPath} activeClassName="link-active">
                        <FontAwesomeIcon icon="user-plus" /> Register
                    </NavLink>
                </NavItem>
                <NavItem>
                    <NavLink tag={RRNavLink} className="text-light" to={loginPath} activeClassName="link-active">
                        <FontAwesomeIcon icon="sign-in-alt" /> Login
                    </NavLink>
                </NavItem>
            </Fragment>
        );
    }
}
