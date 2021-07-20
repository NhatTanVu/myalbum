import React, { Component } from 'react';
import { GlobalDataContext } from '../context/GlobalDataContext';
import { DisplayMode } from '../models/globalData';

interface IAddPhotoProps { }
interface IAddPhotoState {
    forecasts: any[]
    loading: boolean
}

export class AddPhoto extends Component<IAddPhotoProps, IAddPhotoState> {
    static contextType = GlobalDataContext;
    context!: React.ContextType<typeof GlobalDataContext>;

    constructor(props: IAddPhotoProps) {
        super(props);
    }

    componentDidMount() {
        this.context?.setDisplayMode(DisplayMode.Photo);
        this.context?.setEnableDisplayMode(true);
    }

    render() {
        return (
            <div>
                Add Photo
            </div>
        );
    }
}
