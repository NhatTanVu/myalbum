import React, { Component } from 'react';
import { GlobalDataContext } from './GlobalDataContext';
import { DisplayMode, GlobalData } from '../models/globalData';

export class GlobalDataContextProvider extends Component {
    private globalData: GlobalData;

    constructor(props: any) {
        super(props);
        this.setDisplayModeHandler = this.setDisplayModeHandler.bind(this);
        this.setEnableDisplayModeHandler = this.setEnableDisplayModeHandler.bind(this);
        this.globalData = {
            displayMode: DisplayMode.Photo,
            enableDisplayMode: true
        };
    }

    setDisplayModeHandler(displayMode: DisplayMode) {
        this.globalData.displayMode = displayMode;
        this.setState(this.globalData);
    }

    setEnableDisplayModeHandler(enableDisplayMode: boolean) {
        this.globalData.enableDisplayMode = enableDisplayMode;
        this.setState(this.globalData);
    }

    render() {
        return (
            <GlobalDataContext.Provider value={{ globalData: this.globalData, setDisplayMode: this.setDisplayModeHandler, setEnableDisplayMode: this.setEnableDisplayModeHandler }}>
                {this.props.children}
            </GlobalDataContext.Provider>
        );
    }
}
