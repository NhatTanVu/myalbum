import React from 'react';
import { DisplayMode, GlobalData } from '../models/globalData';

export type GlobalDataContextType = {
    globalData: GlobalData;
    setDisplayMode: (displayMode: DisplayMode) => void;
    setEnableDisplayMode: (enableDisplayMode: boolean) => void;
};
export const GlobalDataContext = React.createContext<GlobalDataContextType | undefined>(undefined);