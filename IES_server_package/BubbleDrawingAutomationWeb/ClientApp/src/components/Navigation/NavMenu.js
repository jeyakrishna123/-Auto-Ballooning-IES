import React, { Component } from 'react';
import { NavbarBrand, Navbar, Button,Nav, NavItem } from 'reactstrap';
import { Link, Route, Routes } from 'react-router-dom';
import { SearchBox } from '../Tools/SearchBox';
import { Pagination } from '../Tools/Pagination';
import Image from '../Common/Image';
import * as Constants from '../Common/constants'
import * as rdd  from 'react-device-detect';
import './NavMenu.css';
import useStore from "../Store/store";
 import { v1 as uuid } from "uuid";
import classNames from "classnames";
import { shortBalloon, newBalloonPosition, saveAllBalloonsApi,  recKey, orgKey, config, capitalizeKeys } from '../Common/Common';

export class NavMenu extends Component {

    static displayName = NavMenu.name;
    constructor(props) {
        super(props);
        this.state = { isHoveringSave: false, };
        this.toggleTopbar = this.toggleTopbar.bind(this);
        this.handleMouseOverSave = this.handleMouseOverSave.bind(this);
        this.handleMouseOutSave = this.handleMouseOutSave.bind(this);
    }

    toggleTopbar() { useStore.setState({ topbarIsOpen: !useStore.getState().topbarIsOpen }); }
    handleMouseOverSave() { this.setState({ isHoveringSave: true }); }
    handleMouseOutSave() { this.setState({ isHoveringSave: false }); }

    saveBalloons = (e) => {
        const { drawingDetails, drawingHeader, ItemView, originalRegions, partial_image } = useStore.getState();

        let drawingNo = drawingHeader[0].drawingNo;
        let revNo = drawingHeader[0].revision_No;
        let pageNo = 0;
        let totalPage = 0;
        let rotate;

        if (drawingDetails.length > 0 && ItemView != null) {
            pageNo = Object.values(drawingDetails)[parseInt(ItemView)].currentPage;
            totalPage = Object.values(drawingDetails)[parseInt(ItemView)].totalPage;
            rotate = drawingDetails.map((item) => {
                return item.rotation;

            });
        }

        useStore.setState({ isLoading: true, loadingText: "Saving Balloon... Please Wait..." });

        //console.log(originalRegions)
        // return false;
        let newPositionBAlloon = [];
        let rescale = [];
        let ballonDetails = originalRegions.map((item) => {
            let pageIndex = item.Page_No;
            let superScale = partial_image.filter((a) => {
                return a.item === parseInt(pageIndex);
            });
            if (superScale.length > 0) {
                rescale[pageIndex] = (superScale[0].scale);
            } else {
                rescale[pageIndex] = 1;
            }

            if (item.hasOwnProperty("newarr")) {
                let x = parseInt(item.newarr.Crop_X_Axis * rescale[pageIndex]);
                let y = parseInt(item.newarr.Crop_Y_Axis * rescale[pageIndex]);
                let w = parseInt(item.newarr.Crop_Width * rescale[pageIndex]);
                let h = parseInt(item.newarr.Crop_Height * rescale[pageIndex]);
                let cx = parseInt(item.newarr.Circle_X_Axis * rescale[pageIndex]);
                let cy = parseInt(item.newarr.Circle_Y_Axis * rescale[pageIndex]);
                item.x = x;
                item.y = y;
                item.width = w;
                item.height = h;
                item.Crop_X_Axis = x;
                item.Crop_Y_Axis = y;
                item.Crop_Width = w;
                item.Crop_Height = h;
                item.Circle_X_Axis = cx;
                item.Circle_Y_Axis = cy;
            }
            if (item.hasOwnProperty("xx")) {
                newPositionBAlloon.push({ ...item })
            }
            let b = item.Balloon.toString();
            item.Balloon = b.replaceAll(".", "-");
            item.Balloon_Text_FontSize = 12;
            let plusT = item.PlusTolerance.toString();
            let minusT = item.MinusTolerance.toString();
            item.PlusTolerance = plusT.replaceAll("+", "")
            item.MinusTolerance = minusT.replaceAll("-", "");

            return { ...item, isballooned: true, selectedRegion: "" };
        });

       // console.log(ballonDetails);
         

        let req = {
            drawingNo: drawingNo,
            revNo: revNo,
            pageNo: pageNo,
            totalPage: totalPage,
            ballonDetails: ballonDetails,
            rotate: JSON.stringify(rotate)
        };
        useStore.setState({ selectedRowIndex: null });
       // useStore.setState({ isLoading: false });
       // console.log(rescale)
       // return false;
        setTimeout(() =>
            saveAllBalloonsApi(req).then(r => {
                return r.data;
            })
                .then(r => {
                   if (config.console)
                        console.log(r, "saveAllBalloonsApi")
                    // return false;


                    if (r.length > 0) {

                         //console.log("saved data", r)
                        let rescale = [];
                        r = r.map((item, index) => {
                            if (item.hasOwnProperty("drawLineID")) {
                                delete item.drawLineID;
                            }
                            item.balloon = item.balloon.replaceAll("-", ".");
                            let plusT = item.plusTolerance.replaceAll("+", "")
                            let minusT = item.minusTolerance.replaceAll("-", "")
                            item.plusTolerance = "+" + plusT;
                            item.minusTolerance = "-" + minusT;

                            let pageIndex = item.page_No ;
                            let superScale = partial_image.filter((a) => {
                                return a.item === parseInt(pageIndex);
                            });
                           
                            if (superScale.length > 0) {
                                rescale[pageIndex] = (superScale[0].scale);
                            } else {
                                rescale[pageIndex] = 1;
                            }

                            

                            item.circle_X_Axis = parseInt(item.circle_X_Axis / rescale[pageIndex]);
                            item.circle_Y_Axis = parseInt(item.circle_Y_Axis / rescale[pageIndex]);
                            item.crop_Height = parseInt(item.crop_Height / rescale[pageIndex]);
                            item.crop_Width = parseInt(item.crop_Width / rescale[pageIndex]);
                            item.crop_X_Axis = parseInt(item.crop_X_Axis / rescale[pageIndex]);
                            item.crop_Y_Axis = parseInt(item.crop_Y_Axis / rescale[pageIndex]);

                            return item;
                        });
                        
                        useStore.setState({
                            originalRegions: [],
                            draft: [],
                            drawingRegions: [],
                            balloonRegions: []
                        });


                        //clone a array of object
                        //const oversearchData = JSON.parse(JSON.stringify(r));
                        const oversearchData = [...r];
                        if (config.console)
                        console.log(oversearchData, "saveAllBalloonsApi")
                       // return false;
                        let searchOvergroup = oversearchData.reduce((acc, obj) => {
                            let key = obj.balloon.toString().split('.')[0];
                            acc[key] = acc[key] || [];
                            acc[key].push(obj);
                            return acc;
                        }, {});

                        let grouped = Object.values(searchOvergroup);

                        let groupOverSingle = grouped.reduce((res, curr) => {
                            if (!res[parseInt(curr[0].balloon)]) {
                                res[parseInt(curr[0].balloon)] = { key: parseInt(curr[0].balloon), value: curr }
                            }
                            return res;
                        }, []).filter((a) => a);

                        if (config.console)
                         console.log("oversearchDataSingle", groupOverSingle  )
                       //  useStore.setState({ isLoading: false })
                       //  return;
                        let items = [];
                        let qtyi = [];
                        let groupshapped = groupOverSingle.reduce((r, c) => {
                            let qty = c.value[0].quantity;
                            //if (c.value.length === 1) {
                            // r.push({ b: c.key });
                            //  let i = r.length;
                            //   const id = uuid();
                            //  items[i] = { ...c.value[0], subBalloon: [], id: id, drawLineID: i };
                            // } else {
                            // create quantity and sub balloon based on final object


                            if (qty === 1) {
                                let b = parseInt(c.key).toString();
                                if (c.value.length > 1) {
                                    b = parseInt(c.key).toString() + ".1";
                                }  
                                r.push({ b: b });
                                let i = r.length;
                                const id = uuid();
                                items[i] = { ...c.value[0], id: id, drawLineID: i, isDeleted: false, isballooned: true };
                                let subitems = c.value;
                                let sub = [];
                               
                                if (c.value.length > 1) {
                                    let withoutfirst = subitems.shift();

                                    if (config.console)
                                        console.log("withoutfirst", withoutfirst)
                                    sub = subitems.map(a => {
                                        const sqid = uuid();
                                        r.push({ b: a.balloon });
                                        let isub = r.length;
                                        a.isDeleted = false;
                                        a.isballooned = true;
                                        a.id = sqid;
                                        a.drawLineID = isub;
                                        items[isub] = a;
                                        return a;


                                    });
                                }
                                items[i].subBalloon = sub;


                            }
                            else {
                                    for (let qi = 1; qi <= qty; qi++) {
                                        let b = parseInt(c.key).toString() + "." + qi.toString();

                                        if (!qtyi.includes(b)) {
                                            qtyi.push(b);
                                            let main = c.value.map(a => {
                                                if (b.toString() === a.balloon.toString()) {
                                                    //console.log(b, a.balloon)
                                                    return a;
                                                }
                                                return false;
                                            }).filter(x => x !== false);

                                            if (main.length > 0) {
                                                r.push({ b: c.key });
                                                let i = r.length;
                                                const qid = uuid();
                                                items[i] = { ...main[0], id: qid, drawLineID: i };

                                                let sub = c.value.map(a => {
                                                    if (a.balloon.includes(b + ".")) {
                                                        const sqid = uuid();
                                                        r.push({ b: a.balloon });
                                                        let isub = r.length;
                                                        a.isDeleted = false;
                                                        a.isballooned = true;
                                                        a.id = sqid;
                                                        a.drawLineID = isub;
                                                        items[isub] = a;

                                                        return a;
                                                    }
                                                    return false;
                                                }).filter(x => x !== false);
                                                items[i].subBalloon = sub;
                                            }

                                        }
                                    }
                                }
                           // }
                            return r;
                        }, []);

                        if (config.console)
                            console.log("shapped", groupOverSingle, groupshapped.filter(a => a), items.filter(a => a), qtyi)
                        let newitems = items.filter(a => a);
                       // useStore.setState({ isLoading: false })
                       // return false;
                        if (config.console)
                            console.log( newitems)
                        //return false;
                        let newrects = newitems.map((item, ind) => {
                            const id = uuid();
                            var keys = Object.keys(item);
                            //console.log(item)
                            let newarr = [];
                            var res = keys.reduce((prev, curr, index) => {
                                if (curr === recKey[index]) {
                                    newarr[orgKey[index]] = ((item[curr] === null) ? "" : item[curr]);
                                    return { ...newarr, newarr }
                                }
                                if (curr === "drawLineID") {
                                    newarr["DrawLineID"] = ((item[curr] === null) ? "" : item[curr]);
                                    return { ...newarr, newarr }
                                }
                                if (curr === "isDeleted") {
                                    newarr["isDeleted"] = ((item[curr] === null) ? "" : item[curr]);
                                    return { ...newarr, newarr }
                                }
                                if (curr === "subBalloon") {
                                    let es = item.subBalloon.map(obj => {
                                        let cap = capitalizeKeys(obj);
                                        cap.isDeleted = cap.IsDeleted;
                                        delete cap.IsDeleted;
                                        delete cap.Isballooned;
                                        return { ...cap, isballooned: true, newarr: cap }
                                    });
                                    newarr["subBalloon"] = ((item[curr] === null) ? [] : es);
                                    return { ...newarr, newarr }
                                }
                                return {
                                    ...newarr, newarr: { ...newarr }
                                }
                            }, {});
                            //console.log(res)
                            delete res.newarr.subBalloon;
                            let w = parseInt(item.crop_Width * 1);
                            let h = parseInt(item.crop_Height * 1);
                            let x = parseInt(item.crop_X_Axis * 1);
                            let y = parseInt(item.crop_Y_Axis * 1);
                            return { ...res, x, y, width: w, height: h, id: id, isballooned: true, selectedRegion: "", DrawLineID: ind + 1 };
                        })
                        newrects = shortBalloon(newrects, "DrawLineID");

                        if (config.console)
                            console.log(newrects)
                        const newstate = useStore.getState();
                        let newrect = newBalloonPosition(newrects, newstate);
                        //console.log("saved data org", newrects, newPositionBAlloon)
                        useStore.setState({
                            originalRegions: newrects,
                            draft: newrects,
                            savedDetails: ((newrects.length > 0) ? true : false),
                            drawingRegions: newrect,
                            balloonRegions: newrect
                        });

                        /* const newstate = useStore.getState();
                         if (newstate.savedDetails) {
                             // let originalRegions = newstate.originalRegions;
                             let newrect = newBalloonPosition(newrects, newstate);
                             useStore.setState({
                                 drawingRegions: newrect,
                                 balloonRegions: newrect,
                             });
                         }*/

                    } else {
                        useStore.setState({
                            originalRegions: [],
                            draft: [],
                            drawingRegions: [],
                            balloonRegions: []
                        });
                    }

                    this.setState({ isHoveringSave: false });

                }, (error) => {
                    console.log("Error", error);
                    useStore.setState({ isLoading: false });
                }).catch(error => {
                    console.log(error);
                    useStore.setState({ isLoading: false });
                })
            , 500);
        const state = useStore.getState();
        setTimeout(() => {
            useStore.setState({ ItemView :null});
            }
            , 200);
        setTimeout(() => {
            useStore.setState({ ItemView: state.ItemView });
        }
            , 200);
        return true;

    }
    render() {
        let state = useStore.getState();
        let originalRegions = state.originalRegions;
        let pageNo = 0;

        if (state.drawingDetails.length > 0 && state.ItemView != null) {
            pageNo = parseInt(Object.values(state.drawingDetails)[parseInt(state.ItemView)].currentPage);
        }

        const newrects = originalRegions.map((item) => {
            if (!item.hasOwnProperty("newarr")) {
                return false;
            }
            if (item.Page_No === pageNo) {
                
                return item;
            }
            return false;
        }).filter(item => item !== false);

        if (config.console)
            console.log(newrects);
            return (
                <>
                    <div className="nH w-asV bbg aiw container-fluid p-0">
                        <div className="nH oy8Mbf qp">
                            <header style={{ position: "sticky" }} className="gb_Ka gb_bb" >
                                <div className="gb_ld gb_fd gb_Jc">
                                    <div className="gb_kd gb_ad gb_bd" style={{ "minWidth": rdd.isMobile ? "0px" : "238px" }}>
                                        <div className="wR3cXd">&nbsp;</div>

                                        <div className="gb_Ac" >
                                            <div className="gb_Bc gb_3d">
                                                <NavbarBrand tag={Link} to="/" className="gb_2d gb_Cc gb_0d">
                                                    <Image name='halliburton-logo.svg' alt={Constants.APP_NAME} title={Constants.APP_NAME} className="gb_Hc" />
                                                </NavbarBrand>
                                            </div>
                                        </div>
                                    </div>
                                    <div className="gb_kd gb_ud gb_ze gb_Me gb_Re" style={{ marginLeft:   "inherit"  }}>
                                        <div className="gb_be gb_ae"></div>
                                        <div className="gb_ye">
                                            <div className="searchBox">
                                              
                                                <Routes >
                                                    <Route exact path="/" element={<SearchBox
                                                         />} />
                                                </Routes>
                                              
                                            </div>
                                        </div>
                                    </div>
                                    {state.drawingDetails.length > 0 && (
                                        <>
                                            <Nav style={{ margin: "12px  0 0  0" }}>
                                                <NavItem className="box" style={{ margin: "auto" }} >
                                                    <Button color="light" className={classNames("light-btn buttons Savebtn primary", { "primary_hover": state.drawingDetails.length > 0 })} 
                                                        onClick={this.saveBalloons}
                                                        disabled={(state.drawingDetails.length > 0 && state.originalRegions.length > 0) ? false : true}
                                                        onMouseOver={this.handleMouseOverSave}
                                                        onMouseOut={this.handleMouseOutSave}
                                                    >
                                                <div style={{ position: "relative" }}>
                                                            <span className="PySCBInfobottom EI48Lc" style={{ left: "auto" }}   >
                                                        Save
                                                    </span>
                                                        </div>
                                                <div className="gb_be gb_ae" style={{ display:"contents"} }>Save &nbsp;</div>
                                                <Image name='save-white.svg' className="icon" alt="Save" />
                                            </Button>
                                                </NavItem>
                                            </Nav>
                                        </> 
                                    )}

                                    <Navbar style={{marginLeft:"auto"} }>
                                        {state.drawingDetails.length > 0 && (
                                        <div className="gb_kd gb_ud gb_ze gb_Me gb_Re" style={{ margin: "0  10px 0  0" }}>
                                            <div className="gb_be gb_ae"></div>
                                            <div className="gb_ye">
                                                <div className="searchBox">

                                                    <Routes >
                                                        <Route exact path="/" element={<Pagination
                                                            {...state} />} />
                                                    </Routes>

                                                </div>
                                            </div>
                                            </div>
                                        )}
                                        </Navbar>
                                    
                                    <>
                                        
                                        {/**
                                    <Navbar className="navbar-expand-sm d-none " container="md" light>

                                        <ul className="navbar-nav flex-grow">
                                            <NavItem>
                                                <NavLink onClick={this.toggleTopbar} tag={Link} className="text-dark" to="/">Home</NavLink>
                                            </NavItem>
                                            <NavItem>
                                                <NavLink onClick={this.toggleTopbar} tag={Link} className="text-dark" to="/fetch-data">sample</NavLink>
                                            </NavItem>
                                        </ul>

                                        </Navbar>
                                     */}
                                  </>
                                </div>
 
                            </header>
                        </div>
                    </div>

                </>
            );
        }

}
