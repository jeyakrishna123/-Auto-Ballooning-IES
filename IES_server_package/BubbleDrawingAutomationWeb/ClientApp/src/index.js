import 'bootstrap/dist/css/bootstrap.css';
import React from 'react';
import { createRoot } from 'react-dom/client';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import App from './App';
import * as serviceWorkerRegistration from './serviceWorkerRegistration';
import reportWebVitals from './reportWebVitals';
import Login from './components/Login/Login';
import useStore from "./components/Store/store";
//import { Home } from './components/Pages/Home';
//import { Counter } from './components/Pages/Counter';
//import { FetchData } from "./components/Pages/FetchData";
const fetchConfig = async () => {
    const response = await fetch('/config.json');
    const config = await response.json();
    return config;
};

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
const rootElement = document.getElementById('root');
const root = createRoot(rootElement);
fetchConfig().then((r) => {
    useStore.setState({
        AppSettings: r
    });
    root.render(
        <BrowserRouter basename={baseUrl}>
            <Routes>
                <Route path="/login" element={<Login></Login>} />;
                <Route path="/*" element={<App></App>} />;
            </Routes>
        </BrowserRouter>);
});
// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://cra.link/PWA
serviceWorkerRegistration.unregister();

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
