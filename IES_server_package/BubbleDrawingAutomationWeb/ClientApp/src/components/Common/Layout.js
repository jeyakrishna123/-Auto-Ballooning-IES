import React  from 'react';
import  SideBar  from '../Tools/SideBar';
import Content from './Content';
import { NavMenu } from '../Navigation/NavMenu';
import { Overlay } from './Loader';
import useStore from "../Store/store";
import { Route, Routes } from 'react-router-dom';

const Layout = () => {
    const state = useStore();
    return (
        <div className="top_wrapper container-fluid p-0">
            
            <Overlay isLoading={state.isLoading} />
            <Routes >
                <Route exact path="/" element={<NavMenu />} />
            </Routes>
            <>
                <Routes >
                    <Route exact path="/" element={<SideBar />} />
                </Routes>
                <Content />
                </>
    
    </div>
    );
}
export default Layout;


