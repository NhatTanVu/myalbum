import React, { Component } from 'react';
import { GlobalDataContext } from '../context/GlobalDataContext';
import { DisplayMode } from '../models/globalData';

interface IAddAlbumProps { }
interface IAddAlbumState {
    loading: boolean
}

export class AddAlbum extends Component<IAddAlbumProps, IAddAlbumState> {
    static contextType = GlobalDataContext;
    context!: React.ContextType<typeof GlobalDataContext>;

    constructor(props: IAddAlbumProps) {
        super(props);
    }

    componentDidMount() {
        this.context?.setDisplayMode(DisplayMode.Album);
        this.context?.setEnableDisplayMode(true);
    }

    render() {
        return (
            <div>
                Add Album
            </div>
        );
    }
}
