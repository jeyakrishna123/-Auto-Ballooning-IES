import React, { useState, useEffect } from "react";
import { NavItem, Button, Navbar, NavLink } from "reactstrap";
import classNames from "classnames";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faAlignLeft } from "@fortawesome/free-solid-svg-icons";
import useStore from "../Store/store";
import { Buttons } from "../Canvas/Buttons";
 

const SideBar = () => {

    let state = useStore.getState();
    let isOpen = state.sidebarIsOpen;
    let drawingDetails = state.drawingDetails;

    const [isHovering, setIsHovering] = React.useState(false);
    const handleMouseOver = () => { setIsHovering(true); };
    const handleMouseOut = () => { setIsHovering(false); };
    const arrow_left = require(`../../assets/arrow_left_2x.png`);

    function getCurrentDimension() {
        return {
            width: window.innerWidth,
            height: window.innerHeight,
            heightSet: window.scrollY
        }
    }
    const [screenSize, setScreenSize] = useState(getCurrentDimension());
    useEffect(() => {
        const updateDimension = () => {
            setScreenSize(getCurrentDimension())
        }
        window.addEventListener('resize', updateDimension);
        window.addEventListener('scroll', updateDimension);

        return (() => {
            window.removeEventListener('resize', updateDimension);
            window.removeEventListener('scroll', updateDimension);
        })
    }, [screenSize])

    const screenHeight = (screenSize.height / 2) + screenSize.heightSet;

    const handleItemView = (e) => {
        e.preventDefault();
        const index = e.target.dataset.index;
        let state = useStore.getState();
       // useStore.setState({ ItemView: index, sidebarIsOpen: !state.sidebarIsOpen })
        useStore.setState({ ItemView: index, sidebarIsOpen: false })
        if (state.ItemView !== index) {
            useStore.setState({ isLoading: true, loadingText: "Loading Image..." })
        }
       
    };
    const toggle = (e) => {
        e.preventDefault();
        let state = useStore.getState();
        if (state.drawingDetails.length !== 0) {
           // useStore.setState({ sidebarIsOpen: !state.sidebarIsOpen })
            useStore.setState({ sidebarIsOpen: false })
        } else {
            useStore.setState({ sidebarIsOpen: false })
        }
       
    };
    const props = useStore.getState();
    return (
        <>
            <div className="XltNde tTVLSc" style={{ width: isOpen ? "280px" : "72px" }}>
                <div className="w6VYqd" style={{
                    background: "rgba(255, 255, 255, 0.161)",
                    zIndex:"4"
                } }>
                    <div className="bJzME tTVLSc">
                        <ul className="navbar-nav flex-grow ODXihb Hk4XGb"  >
                            <NavItem className="wR3cXd d-none" style={{ marginTop: "52px"} }>
                                <Button className={classNames("toggleSidebar d-none", { "is-open": isOpen })} size="sm" color="info" onClick={toggle}   aria-label="Main menu" data-ogmb="1" role="button" tabIndex="0">
                                    <FontAwesomeIcon icon={faAlignLeft} />
                                </Button>
                            </NavItem>
                            <NavItem className="wR3cXd item m-0 " >
                                <Buttons drawingDetails={props.drawingDetails} ItemView={props.ItemView} />
                            </NavItem>
                        </ul>
                        
                    </div>

                    <div className="bJzME tTVLSc d-none">
                        <div className={classNames("k7jAl lJ3Kh", { "miFGmb": isOpen })} style={{ width: isOpen ? "208px" : "0px" }}>
                            <div className="e07Vkf kA9KIf">
                                <div className="aIFcqe" style={{ width: isOpen ? "208px" : "72px" }}>
                                    <div className="m6QErb WNBkOb">
                                        <div className="ZKCDEc">
                                            <div className="RZ66Rb FgCUCc">
                                                <Navbar className="navbar-expand-sm" container="md" light >

                                                    <ul className=" flex-grow">
                                                {drawingDetails.map((item, index) => {
                                                     
                                                    return (
                                                        <NavItem key={index} >
                                                            <NavLink key={index} data-index={index}  onClick={handleItemView} className="text-dark" style={{ cursor: "pointer"} } >{item.fileName}</NavLink>
                                                        </NavItem>
                                                    );
                                                })}
                                                    </ul>

                                                </Navbar>
                                            </div>
                                            
                                            <div className="jwfPme"></div>

                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div className="gYkzb d-none"
                    style={{ left: isOpen ? "280px" : "72px", top: screenHeight , "zIndex": 99999 }}
                    onMouseOver={handleMouseOver}
                    onMouseOut={handleMouseOut}  >
                    <Button style={{ "zIndex": 99999  }} className={classNames("yra0jd Hk4XGb", { "is-open": isOpen })} size="sm" color="info" onClick={toggle} aria-expanded={toggle} aria-label="Main menu" data-ogmb="1" role="button" tabIndex="0">
                        <img alt="" src={arrow_left} className="EIbCs" />
                    </Button>
                    <span className="PySCB EI48Lc" aria-hidden={!isOpen} style={{ display :isHovering ? "block" : "none" } } >

                        {isHovering && (
                            <div>
                                {!isOpen ? "Expand side panel" : "Collapse side panel"}
                            </div>
                        )}
                    </span>
                </div>
            </div>
        </>
    );

}
export default SideBar;
