import React from "react";
import { Route, Routes, useLocation } from 'react-router-dom';
import AppRoutes from '../../AppRoutes';
import classNames from "classnames";
import { Home } from "../Pages/Home"


const Content = ({ sidebarIsOpen }) => {
    const location = useLocation();
    //console.log(location.pathname);
    let classNamess = location.pathname === "/" ? "content bkk":""
    return (

        <div
             
            className={classNames(classNamess, { "is-open": sidebarIsOpen })}
              >

            <Routes>
                {AppRoutes.map((route, index) => {
                    const { element, ...rest } = route;
                    
                    if (route.index) {
                        return <Route key={index} {...rest} exact path="/" element={<Home  />} />
                    } else {
              
                        return <Route key={index} {...rest} element={element} />;
                    }
                   
                })}
            </Routes>
        </div>

    );
}
export default Content;
