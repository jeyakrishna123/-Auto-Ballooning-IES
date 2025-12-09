import React, { Component } from 'react';
import Layout from './components/Common/Layout';
import Login from './components/Login/Login';
import './components/Common/custom.css';
import useStore from "./components/Store/store";
import { Navigate, Route, Routes } from 'react-router-dom';
 

export default class App extends Component {
    static displayName = App.name
    
 
    render() {
        let state = useStore.getState();
        const user = state.user;
        return (
            <>
            {user.length > 0 ? <Layout drawingDetails={null} ></Layout> : (<Navigate to="/login" />) }
            {user.length === 0 && (
                    <>
                        <Routes >
                            <Route exact path="/login" element={<Login />} />
                        </Routes>
                    </>
             )}
            </>
        );
  }
}
