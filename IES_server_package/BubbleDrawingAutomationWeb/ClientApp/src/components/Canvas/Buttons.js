import React, { Component } from 'react';
import { Row, Col, Button, Nav, NavItem } from "reactstrap";
import Image from '../Common/Image';
import useStore from "../Store/store";
import initialState from "../Store/init";
import Swal from 'sweetalert2'
import { v1 as uuid } from "uuid";
import { config, shortBalloon, actualSize, resetBalloonsProcess, newBalloonPosition, rotateProcessApi, makeAutoballoonApi, saveBalloonsApi, recKey, orgKey, capitalizeKeys } from '../Common/Common';

 
export class Buttons extends Component {
    static displayName = Buttons.name;

    constructor(props) {
        super(props);
        this.state = {
            isHoveringACT: false,
            isHoveringFIT: false,
            isHoveringZoomOut: false,
            isHoveringZoomIn: false,
            isHoveringCCW: false,
            isHoveringCW: false,
            isHoveringMAG: false,
            isHoveringFH: false,
            isHoveringDel: false,
            isHoveringSave: false,
            isHoveringReset: false,
            isHoveringManual: false,
            isHoveringSelected: false,
            isHoveringAuto: false,
            isHoveringUnSelected: false,
            isHoveringResetB: false,
            isHoveringSPL: false,
            
            selectedRegion: "",
            width: 0,
            height: 0,
            topbarIsOpen: 1,
            propertyclick: 1 
        };
        this.timer = null;
        this.windowResized = this.windowResized.bind(this);
        this.updateWindowWidth = this.updateWindowWidth.bind(this);

        this.selectedRegion = this.selectedRegion.bind(this); 
 
        this.toggleTopbar = this.toggleTopbar.bind(this);
        this.propertyclick = this.propertyclick.bind(this);

        this.handleMouseOverACT = this.handleMouseOverACT.bind(this);
        this.handleMouseOutACT = this.handleMouseOutACT.bind(this);

        this.handleMouseOverFIT = this.handleMouseOverFIT.bind(this);
        this.handleMouseOutFIT = this.handleMouseOutFIT.bind(this);

        this.handleMouseOverZoomOut = this.handleMouseOverZoomOut.bind(this);
        this.handleMouseOutZoomOut = this.handleMouseOutZoomOut.bind(this);

        this.handleMouseOverZoomIn = this.handleMouseOverZoomIn.bind(this);
        this.handleMouseOutZoomIn = this.handleMouseOutZoomIn.bind(this);

        this.handleMouseOverCCW = this.handleMouseOverCCW.bind(this);
        this.handleMouseOutCCW = this.handleMouseOutCCW.bind(this);

        this.handleMouseOverCW = this.handleMouseOverCW.bind(this);
        this.handleMouseOutCW = this.handleMouseOutCW.bind(this);

        this.handleMouseOverMAG = this.handleMouseOverMAG.bind(this);
        this.handleMouseOutMAG = this.handleMouseOutMAG.bind(this);

        this.handleMouseOverFH = this.handleMouseOverFH.bind(this);
        this.handleMouseOutFH = this.handleMouseOutFH.bind(this);

        this.handleMouseOverDel = this.handleMouseOverDel.bind(this);
        this.handleMouseOutDel = this.handleMouseOutDel.bind(this);

        this.handleMouseOverSave = this.handleMouseOverSave.bind(this);
        this.handleMouseOutSave = this.handleMouseOutSave.bind(this);

        this.handleMouseOverReset = this.handleMouseOverReset.bind(this);
        this.handleMouseOutReset = this.handleMouseOutReset.bind(this);
        this.showAlertOnReset = this.showAlertOnReset.bind(this);

        this.handleZoomIn = this.handleZoomIn.bind(this);
        this.handleZoomOut = this.handleZoomOut.bind(this);

        this.handleMouseOverManual = this.handleMouseOverManual.bind(this);
        this.handleMouseOutManual = this.handleMouseOutManual.bind(this);

        this.handleMouseOverSelected = this.handleMouseOverSelected.bind(this);
        this.handleMouseOutSelected = this.handleMouseOutSelected.bind(this);

        this.handleMouseOverUnSelected = this.handleMouseOverUnSelected.bind(this);
        this.handleMouseOutUnSelected = this.handleMouseOutUnSelected.bind(this);

        this.handleMouseOverResetB = this.handleMouseOverResetB.bind(this);
        this.handleMouseOutResetB = this.handleMouseOutResetB.bind(this);

        this.handleMouseOverAuto = this.handleMouseOverAuto.bind(this);
        this.handleMouseOutAuto = this.handleMouseOutAuto.bind(this);

        this.handleMouseOverSPL = this.handleMouseOverSPL.bind(this);
        this.handleMouseOutSPL = this.handleMouseOutSPL.bind(this);

    } 

    componentDidMount() {
        window.addEventListener("resize", this.windowResized);
        this.updateWindowWidth();
    }
    componentDidUpdate(oldProps) {
        if (oldProps.ItemView !== this.props.ItemView &&  this.props.drawingDetails.length > 0
            && this.props.ItemView !== null) {

            useStore.setState ({
                isDisabledACT: false,
                isDisabledFIT: false,
                isDisabledZoomOut: false,
                isDisabledZoomIn: false,
                isDisabledCCW: false,
                isDisabledCW: false,
                isDisabledMAG: false,
                isDisabledFH: false,
                isDisabledDel: false,
                isDisabledSave: false,
                isDisabledReset: false,
                isDisabledResetB: false,
                isDisabledAutoB: false,
                isDisabledSPL: false,
                isDisabledOCR: false,
                isDisabledRegion: false,
                isDisabledMove: false,
                isDisabledSelected: false,
                isDisabledManual: false,
                isDisabledUnSelected:false,
            });
        }
    }

    componentWillUnmount() {
        window.removeEventListener("resize", this.windowResized);
    }

    updateWindowWidth() {
        let _this = this;
        setTimeout(function () {
            _this.setState({
                width: window.innerWidth,
                height: window.innerHeight
            });
        });
    }

    windowResized() {
        let _this = this;
        if (this.timer) {
            clearTimeout(this.timer);
        }
        this.timer = setTimeout(function () {
            _this.updateWindowWidth();
        }, 500);
    }

    handleMouseOverAuto() { this.setState({ isHoveringAuto: true }); }
    handleMouseOutAuto() { this.setState({ isHoveringAuto: false }); }

    handleMouseOverResetB() { this.setState({ isHoveringResetB: true }); }
    handleMouseOutResetB() { this.setState({ isHoveringResetB: false }); }

    handleMouseOverUnSelected() { this.setState({ isHoveringUnSelected: true }); }
    handleMouseOutUnSelected() { this.setState({ isHoveringUnSelected: false }); }

    handleMouseOverSelected() { this.setState({ isHoveringSelected: true }); }
    handleMouseOutSelected() { this.setState({ isHoveringSelected: false }); }

    handleMouseOverManual() { this.setState({ isHoveringManual: true }); }
    handleMouseOutManual() { this.setState({ isHoveringManual: false }); }

    handleMouseOverACT() { this.setState({ isHoveringACT: true });  }
    handleMouseOutACT() { this.setState({ isHoveringACT: false }); }

    handleMouseOverFIT() { this.setState({ isHoveringFIT: true }); }
    handleMouseOutFIT() { this.setState({ isHoveringFIT: false }); }

    handleMouseOverZoomOut() { this.setState({ isHoveringZoomOut: true }); }
    handleMouseOutZoomOut() { this.setState({ isHoveringZoomOut: false }); }

    handleMouseOverZoomIn() { this.setState({ isHoveringZoomIn: true }); }
    handleMouseOutZoomIn() { this.setState({ isHoveringZoomIn: false }); }

    handleMouseOverCCW() { this.setState({ isHoveringCCW: true }); }
    handleMouseOutCCW() { this.setState({ isHoveringCCW: false }); }

    handleMouseOverCW() { this.setState({ isHoveringCW: true }); }
    handleMouseOutCW() { this.setState({ isHoveringCW: false }); }

    handleMouseOverMAG() { this.setState({ isHoveringMAG: true }); }
    handleMouseOutMAG() { this.setState({ isHoveringMAG: false }); }

    handleMouseOverFH() { this.setState({ isHoveringFH: true }); }
    handleMouseOutFH() { this.setState({ isHoveringFH: false }); }

    handleMouseOverDel() { this.setState({ isHoveringDel: true }); }
    handleMouseOutDel() { this.setState({ isHoveringDel: false }); }

    handleMouseOverSave() { this.setState({ isHoveringSave: true }); }
    handleMouseOutSave() { this.setState({ isHoveringSave: false }); }

    handleMouseOverReset() { this.setState({ isHoveringReset: true }); }
    handleMouseOutReset() { this.setState({ isHoveringReset: false }); }

    handleMouseOverSPL() { this.setState({ isHoveringSPL: true }); }
    handleMouseOutSPL() { this.setState({ isHoveringSPL: false }); }

    toggleTopbar() {
        this.setState({
            topbarIsOpen: !this.state.topbarIsOpen
        });
    }
    propertyclick() {
        this.setState({
            topbarIsOpen: !this.state.topbarIsOpen
        });

    }
 
    changeRegion = (e) => {
        e.preventDefault();
        let selectedRegion = e.currentTarget.getAttribute('data-value');
        let state = useStore.getState();
        if (state.selectedRegion === selectedRegion) {
            useStore.setState({ selectedRegion: "" })
        } else {
            useStore.setState({ selectedRegion: selectedRegion })
        }
    };

    selectedRegion = (e) => {
        let selectedRegion = e.target.value;
        let state = useStore.getState();
        if (state.selectedRegion === selectedRegion) {
            useStore.setState({ selectedRegion: "Manual Drawn" })
        } else {
            useStore.setState({ selectedRegion: selectedRegion })
        }
    }

    makeAutoballoon = (e) => {
        e.preventDefault();
        useStore.setState({ selectedRegion: "Full Image" })

        const state = useStore.getState();
        if (state.isErrImage) {
            return;
        }
        const {
            originalRegions,
            drawingHeader,
            partial_image,
            ItemView,
            drawingDetails,
            aspectRatio,
            bgImgX,
            bgImgY,
            bgImgW,
            bgImgH,
            selectedRegion
            } = state;
        let CurrentItem = drawingDetails[ItemView].annotation;
        let CdrawingNo = drawingHeader[0].drawingNo;
        let CrevNo = drawingHeader[0].revision_No
        let pageNo = 0;
        let totalPage = 0;
        let rotation = 0;
        let rotate_properties = [];
        let origin = [];

        if (drawingDetails.length > 0 && ItemView != null) {
            pageNo = Object.values(drawingDetails)[parseInt(ItemView)].currentPage;
            totalPage = Object.values(drawingDetails)[parseInt(ItemView)].totalPage;
            rotation = Object.values(drawingDetails)[parseInt(ItemView)].rotation;
            let rotate = drawingDetails.map(s => parseInt(s.rotation));
            rotate_properties = JSON.stringify(rotate);
            origin = Object.values(partial_image)[parseInt(ItemView)];
        }
        
        const oldDraw = originalRegions.map((item, i) => {
            if (item.hasOwnProperty("newarr") && parseInt(pageNo) !== parseInt(item.Page_No)) {
                const id = uuid();
                let w = parseInt(item.newarr.Crop_Width * 1);
                let h = parseInt(item.newarr.Crop_Height * 1);
                let x = parseInt(item.newarr.Crop_X_Axis * 1);
                let y = parseInt(item.newarr.Crop_Y_Axis * 1);
                let cx = parseInt(item.newarr.Circle_X_Axis * 1);
                let cy = parseInt(item.newarr.Circle_Y_Axis * 1);
                item.Crop_Width = w;
                item.Crop_Height = h;
                item.Crop_X_Axis = x;
                item.Crop_Y_Axis = y;
                item.Circle_X_Axis = cx;
                item.Circle_Y_Axis = cy;
                item.height = h;
                item.width = w;
                item.x = x;
                item.y = y;
                item.id = id;
                item.isballooned = true;
                item.selectedRegion = "";
                if (item.hasOwnProperty("DrawLineID"))
                    delete item.DrawLineID;

                return item;
            }
            return false;
        }).filter(item => item !== false);

        let resetOverData = [...oldDraw];

        let resetOverSingle = resetOverData.reduce((res, item) => {
            if (!res[parseInt(item.Balloon)]) {
                res[parseInt(item.Balloon)] = item;
            }
            return res;
        }, []);

        let qtyi = 0;
        // get all quantity parent
        let Qtyparent = resetOverData.reduce((res, item) => {
            if (item.hasOwnProperty("subBalloon") && item.subBalloon.length >= 0 && item.Quantity > 1) {
                res[qtyi] = item;
                qtyi++;
            }
            return res;
        }, []);

        let unique = Object.values(resetOverSingle);
        //console.log(unique)


        let newitems = [];

        unique.reduce((prev, curr, index) => {
            const id = uuid();
            let newarr = [];
            let Balloon = index + 1;
            Balloon = Balloon.toString();
            if (curr.Quantity === 1 && curr.subBalloon.length === 0) {
                prev.push({ b: (Balloon), c: prev.length + 1 })
                let i = prev.length;
                newarr.push({ ...curr, newarr: { ...curr.newarr, Balloon: Balloon }, id: id, DrawLineID: i, Balloon: Balloon });
            }
            if (curr.Quantity === 1 && curr.subBalloon.length > 0) {
                let pb = parseInt(Balloon).toString() + ".1";
                prev.push({ b: pb, c: prev.length + 1 })
                let i = prev.length;
                newarr.push({ ...curr, newarr: { ...curr.newarr, Balloon: pb }, id: id, DrawLineID: i, Balloon: pb, selectedRegion: "" });
                curr.subBalloon.filter((x) => x.isDeleted === false).map(function (e, ei) {
                    let sno = ei + 2;
                    const sid = uuid();
                    let b = parseInt(Balloon).toString() + "." + sno.toString();
                    prev.push({ b: b, c: prev.length + 1 })
                    let i = prev.length;
                    if (e.hasOwnProperty("Isballooned"))
                        delete e.Isballooned;
                    if (e.hasOwnProperty("Id"))
                        delete e.Id;

                    let setter = { ...e, newarr: { ...e.newarr, Balloon: b }, id: sid, DrawLineID: i, Balloon: b, selectedRegion:"" };
                    newarr.push(setter);
                    return e;
                })
            }
            if (curr.Quantity > 1 && curr.subBalloon.length === 0) {
                for (let qi = 1; qi <= curr.Quantity; qi++) {
                    const qid = uuid();
                    let b = parseInt(Balloon).toString() + "." + qi.toString();
                    prev.push({ b: b, c: prev.length + 1 })
                    let i = prev.length;
                    newarr.push({ ...curr, newarr: { ...curr.newarr, Balloon: b }, id: qid, DrawLineID: i, Balloon: b, selectedRegion: "" });
                }
            }
            if (curr.Quantity > 1 && curr.subBalloon.length > 0) {

                for (let qi = 1; qi <= curr.Quantity; qi++) {
                    let newMainItem = [];
                    let pb = parseInt(curr.Balloon).toString() + "." + qi.toString();
                    newMainItem = Qtyparent.map(item => {
                        if (pb === item.Balloon) {
                            return item;
                        }
                        return false;
                    }).filter(x => x !== false);
                    // console.log("sd", Qtyparent, newMainItem, pb, qi)
                    if (newMainItem.length > 0) {

                        newMainItem.map((nmi) => {
                            const qid = uuid();
                            let pb = parseInt(Balloon).toString() + "." + (qi).toString();
                            prev.push({ b: pb, c: prev.length + 1 })
                            let i = prev.length;

                            newarr.push({ ...nmi, newarr: { ...nmi.newarr, Balloon: pb }, id: qid, DrawLineID: i, Balloon: pb, selectedRegion: "" });
                            let newSubItem = nmi.subBalloon.filter(a => {
                                return a.isDeleted === false;
                            });
                            newSubItem.filter((x) => x.isDeleted === false).map(function (e, ei) {
                                let sqno = ei + 1;
                                const sqid = uuid();
                                let b = pb + "." + sqno.toString();
                                prev.push({ b: b, c: prev.length + 1 })
                                let i = prev.length;
                                if (e.hasOwnProperty("Isballooned"))
                                    delete e.Isballooned;
                                if (e.hasOwnProperty("Id"))
                                    delete e.Id;
                                let setter = { ...e, newarr: { ...e.newarr, Balloon: b }, id: sqid, DrawLineID: i, Balloon: b, selectedRegion: "" };
                                newarr.push(setter);
                                return e;
                            })
                            return nmi;
                        })
                    }

                }



            }

            newitems = newitems.slice();
            newitems.splice(newitems.length, 0, ...newarr);

            return prev;
        }, []);

        
       // setInterval(() =>
        let requestData = {
            ItemView: ItemView,
            CdrawingNo: CdrawingNo,
            CrevNo: CrevNo,
            drawingDetails: CurrentItem,
            aspectRatio: aspectRatio,
            bgImgW: bgImgW,
            bgImgH: bgImgH,
            bgImgX: bgImgX,
            bgImgY: bgImgY,
            pageNo: pageNo,
            totalPage: totalPage,
            annotation: [],
            originalRegions: newitems,
            selectedRegion: selectedRegion,
            drawingRegions: [],
            balloonRegions: [],
            rotate: rotate_properties,
            origin: [origin],
            bgImgRotation: rotation

        };
        useStore.setState({ selectedRowIndex: null });
        if (config.console)
            console.log("auto req", requestData);
        //return false; 
        useStore.setState({
             originalRegions: oldDraw,
            drawingRegions: [],
            balloonRegions: [], isLoading: true, loadingText: "Processing Auto Balloon..." })
         
        makeAutoballoonApi(requestData)
            .then(r => {
                return r.data;
            })
            .then(r => {
                if (config.console)
                    console.log(r, "Auto  balloon res")
                useStore.setState({ isLoading: false, is_BalloonDrawingSaved: true });
                useStore.setState({ selectedRegion: "" })

                if (r.length > 0) {
                    if (config.console)
                        console.log("saved data", r)
                    r = r.map((item, index) => {
                        if (item.hasOwnProperty("drawLineID")) {
                            delete item.drawLineID;
                        }
                        item.balloon = item.balloon.replaceAll("-", ".");
                        return item;
                    });

                    //clone a array of object
                   // const oversearchData = JSON.parse(JSON.stringify(r));
                    const oversearchData = [...r];

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

                    // console.log("oversearchDataSingle",  unique, groupOverSingle  )
                    // useStore.setState({ isLoading: false })
                    // return;
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
                     

                    if (config.console)
                        console.log("auto", newitems)
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
                        console.log("auto balloon page" + pageNo, newrects)


                    useStore.setState({
                        originalRegions: newrects,
                        draft: newrects,
                        savedDetails: ((newrects.length > 0) ? true : false),
                        drawingRegions: [],
                        balloonRegions: []
                    });
                    const newstate = useStore.getState();
                    if (newstate.savedDetails) {
                        let originalRegions = newstate.originalRegions;
                        let newrect = newBalloonPosition(originalRegions, newstate);
                        useStore.setState({
                            savedDetails: false,
                            drawingRegions: newrect,
                            balloonRegions: newrect,
                            isDisabledAutoB: false
                        });

                    }
  
                }

  
            }, (e) => {
                console.log("Error", e);
                useStore.setState({ isLoading: false });
            }).catch(e => {
                console.log("catch",e);
                useStore.setState({ isLoading: false });
            })
 
          // , 100000);  
    }

    saveBalloons = (e) => {
        const state = useStore.getState();
        const {
            draft,
            originalRegions,
            drawingRegions,
            drawingHeader,
            ItemView,
            drawingDetails,
            aspectRatio,
            bgImgX,
            bgImgY,
            bgImgW,
            bgImgH,
            selectedRegion,
            bgImgRotation
            } = state;
        let CurrentItem = drawingDetails[ItemView].annotation;
        let CdrawingNo = drawingHeader[0].drawingNo;
        let CrevNo = drawingHeader[0].revision_No;
        let pageNo = 0;
        let totalPage = 0;
        if (drawingDetails.length > 0 && ItemView != null) {
            pageNo = Object.values(drawingDetails)[parseInt(ItemView)].currentPage;
            totalPage = Object.values(drawingDetails)[parseInt(ItemView)].totalPage;
        }
        
        useStore.setState({ isLoading: true, loadingText: "Saving Balloon... Please Wait..." })
        let req = {
            ItemView: ItemView,
            CdrawingNo: CdrawingNo,
            CrevNo: CrevNo,
            drawingDetails: CurrentItem,
            aspectRatio: aspectRatio,
            bgImgW: bgImgW,
            bgImgH: bgImgH,
            bgImgX: bgImgX,
            bgImgY: bgImgY,
            pageNo: pageNo,
            totalPage: totalPage,
            annotation: [],
            selectedRegion: selectedRegion,
            drawingRegions: drawingRegions,
            balloonRegions: [],
            originalRegions: originalRegions,
            bgImgRotation: bgImgRotation

        }
        setTimeout(() =>
            saveBalloonsApi(req).then(response => {
                return response.json();
            })
            .then(Response => {
                
                const newrects = originalRegions.map((item) => {
                    if (!item.isballooned) {
                        return { ...item, isballooned: true};
                    }
                    return item;
                })
                const draw = drawingRegions.map((item) => {
                    if (!item.isballooned) {
                        return { ...item, isballooned: true };
                    }
                    return item;
                })
                useStore.setState({
                    isLoading: false,
                    is_BalloonDrawingSaved: true,
                    drawingRegions: draw,
                    originalRegions: newrects,
                    draft: draft
                });
                //console.log(Response);
                return Response;
                //drawingRegions.push(Response)
                // useStore.setState({ drawingRegions: {drawingRegions[ItemView] : Response } });
            }, (error) => {
                console.log("Error", error);
                useStore.setState({ isLoading: false });
            }).catch(error => {
                console.log(error);
                useStore.setState({ isLoading: false });
            })
            , 500);  
        return false;

    }
    
    handleZoomIn = (e) => {
        e.preventDefault();
        let state = useStore.getState();
  
        if (state.isErrImage) {
            return;
        }
        useStore.setState({ zoomed: true });
        useStore.setState({ scrollPosition: 0 });
       // return;
       

        let scale = 0;
        if (state.fitscreen) {
            useStore.setState({ fitscreen: false });
            setTimeout(function (e) { 
                actualSize();
            }, 10);
            state = useStore.getState();
        }
       
        scale = 0.1 + state.zoomingfactor;
        //console.log(scale)
        
        useStore.setState({ zoomingfactor : scale});
        let w = parseInt(state.bgImgW * scale, 10);
        let h = parseInt(state.bgImgH * scale, 10);
        // console.log(w, h, state.scaleStep , state.InitialScale)
        if (state.imageWidth > w && state.imageHeight > h) {
          //  return;
            useStore.setState({ isDisabledZoomIn: true });
            setTimeout(function () { 
            let x = 0;
            let y = 0;
            if (w > state.win.width || h > state.win.height) {
                //let newwin = { width: w, height: h }

                var padding = state.pad;
                let newwin = {
                    width: (w > state.win.width ? (w + (2 * padding)) : (state.win.width)),
                    height: (h > state.win.height ? (h + (2 * padding)) : (state.win.height))
                }
                useStore.setState({ win: newwin });
            }
            let newstate = useStore.getState();
           // if (w < newstate.win.width) {
                x = (newstate.win.width - w) / 2;
           // }
          //  if (h < newstate.win.height) {
                y = (newstate.win.height - h) / 2;
          //  }

                let zobj = {  bgImgScale: scale, bgImgW: w, bgImgH: h, bgImgX: x, bgImgY: y };
                // console.log(zobj)
                useStore.setState({ history: [...state.history, zobj] });
               
              //  useStore.setState({ zoomedstate: true });
                useStore.setState(zobj);
                let nstate = useStore.getState();
                //console.log(nstate.history)
                let originalRegions = nstate.originalRegions;
                let newrect = newBalloonPosition(originalRegions, nstate);
                useStore.setState({
                    drawingRegions: newrect,
                    balloonRegions: newrect,
                });


              //  let zstate = useStore.getState();
              //  let zoomFactorWidth = zstate.imageWidth / zstate.bgImgW;
              //  let zoomFactorHeight = zstate.imageHeight / zstate.bgImgH;

              //  console.log("Zoom factor for width:", zoomFactorWidth);
               // console.log("Zoom factor for height:", zoomFactorHeight);


                let scrollElement = document.querySelector('#konvaMain');
                scrollElement.scrollLeft = (scrollElement.scrollWidth - scrollElement.clientWidth) / 2;
                useStore.setState({ isDisabledZoomIn: false });
            }, 200);
           
        } else {
            let zobj = { isDisabledZoomIn: true };
            useStore.setState(zobj);
        }
    }

    handleZoomOut = (e) => {
        e.preventDefault();
        useStore.setState({ isDisabledZoomOut: true });
        let props = useStore.getState();
        if (props.isErrImage) {
            return;
        }
        useStore.setState({ zoomed: true });
        useStore.setState({ scrollPosition: 0 });
        
      //  return;
       // console.log(props.history)
        if (props.history.length > 1) {
            useStore.setState({ fitscreen: false});
            let rem = props.history.slice(0, -1);
            let zobj = rem[rem.length - 1];
            useStore.setState({ history: rem });
            useStore.setState(zobj);
            if (zobj.bgImgW > initialState.win.width || zobj.bgImgH > initialState.win.height) {
                let newwin = { width: zobj.bgImgW, height: zobj.bgImgH }
                useStore.setState({ win: newwin });
            } else {
                useStore.setState({ win: { width: initialState.win.width, height: initialState.win.height } });
            }
            let nstate = useStore.getState();
            let originalRegions = nstate.originalRegions;
            let newrect = newBalloonPosition(originalRegions, nstate);
            useStore.setState({
                drawingRegions: newrect,
                balloonRegions: newrect,
            });
            let scrollElement = document.querySelector('#konvaMain');
            scrollElement.scrollLeft = (scrollElement.scrollWidth - scrollElement.clientWidth) / 2;
        } else {
            
            let nstate = useStore.getState();
            //var padding = nstate.pad;
            let nscale = nstate.scaleStep - nstate.InitialScale;
            let nw = parseInt(nstate.bgImgW * Math.abs(nscale ), 10);
            let nh = parseInt(nstate.bgImgH * Math.abs(nscale ), 10);
            let x1 = 0;
            let y1 = 0;
          //  console.log(nw, initialState.win.width, nh, initialState.win.height, window.innerWidth, window.innerHeight)

            if (nw > window.innerWidth || nh > window.innerHeight) {
                    let newwin = {
                        width: (nw > window.innerWidth ? (nw  ) : (window.innerWidth)),
                        height: (nh > window.innerHeight ? (nh  ) : (window.innerHeight))
                    }
                    useStore.setState({ win: newwin });
                    x1 = (window.innerWidth - nw) / 2;
                    y1 = (window.innerHeight - nh) / 2;

                    let zobj = { bgImgScale: nscale, bgImgW: nw, bgImgH: nh, bgImgX: x1, bgImgY: y1 };
                    useStore.setState(zobj);
                    let scrollElement = document.querySelector('#konvaMain');
                    scrollElement.scrollLeft = (scrollElement.scrollWidth - scrollElement.clientWidth) / 2;
                }
                
            this.fitToActualsize(e);
 
            let scrollElement = document.querySelector('#konvaMain');
            if (scrollElement !== null) {

                scrollElement.scrollLeft = (scrollElement.scrollWidth - scrollElement.clientWidth) / 2;
            }
        }
        useStore.setState({ isDisabledZoomOut: false });
    }

    rotateRightBackgroundImage = (e) => {
        e.preventDefault();
        let { isErrImage, drawingDetails, drawingHeader, originalRegions, ItemView, bgImgRotation, sessionId } = useStore.getState();
        if (isErrImage) {
            return;
        }
        let view = ItemView;
        const ROTATE = 90;
        let CurrentItem = drawingDetails[ItemView].annotation;
        let drawingNo = drawingHeader[0].drawingNo;
        let revNo = drawingHeader[0].revision_No;
        let pageNo = 0;
        let totalPage = 0;
        let obj;
        if (drawingDetails.length > 0 && ItemView != null) {
            obj = Object.values(drawingDetails)[parseInt(ItemView)];
            pageNo = parseInt(obj.currentPage);
            totalPage = obj.totalPage;
        }
        const newrects = originalRegions.map((item) => {
            if (item.Page_No === pageNo) {
                return item;
            }
            return false;
        }).filter(item => item !== false);
        // if there is no ballloon on current page then process
        let rotation = bgImgRotation + ROTATE;
        if (newrects.length > 0) {
           // if (rotation === 360) { rotation = 0; }
         //   useStore.setState({
          //      bgImgRotation: rotation
         //   })
          //  console.log(useStore.getState() )
            return false;
        }
   
        let nrotation = drawingDetails[ItemView].rotation;
        rotation = parseInt(nrotation) + ROTATE;
        if (Math.sign(rotation) === -1 && Math.abs(rotation) === 90) { rotation = 270; }
        if (rotation === 360) { rotation = 0;  }
        let requestData = {
            ItemView: ItemView,
            drawingNo: drawingNo,
            revNo: revNo,
            drawingDetails: CurrentItem,
            pageNo: pageNo,
            totalPage: totalPage,
            sessionUserId: sessionId,
            rotation: rotation
        };
        //console.log(requestData)
        useStore.setState({ isLoading: true, loadingText: "Rotating an Image...", ItemView: null });
        rotateProcessApi(requestData).then(r => {
                return r.data;
            })
            .then(r => {
                let draw = drawingDetails.map((item) => {
                    if (item.currentPage.toString() === pageNo.toString() ) {
                        return { ...item, rotation: rotation };
                    }
                    return item;
                })
                //useStore.setState({ drawingDetails: draw, isLoading: true, loadingText: "Loading an Image...", ItemView: null });
                useStore.setState({ drawingDetails: draw, isLoading: true, loadingText: "Loading an Image...", ItemView: view });

                   // console.log(r,view)
                    
                return r; 

                }, (e) => {
                console.log("Error", e);
                useStore.setState({ isLoading: false });
            }).catch (e => {
                console.log("catch",e);
                useStore.setState({ isLoading: false });
            })
 
        // console.log(useStore.getState())

        let scrollElement = document.querySelector('#konvaMain');
        scrollElement.scrollLeft = (scrollElement.scrollWidth - scrollElement.clientWidth) / 2;
        return false;
    };

    rotateLeftBackgroundImage = () => {
        let {isErrImage, drawingDetails, drawingHeader, originalRegions, ItemView, bgImgRotation, sessionId } = useStore.getState();
        if (isErrImage) {
            return;
        }
        let view = ItemView;
        const ROTATE = -90;
        let CurrentItem = drawingDetails[ItemView].annotation;
        let drawingNo = drawingHeader[0].drawingNo;
        let revNo = drawingHeader[0].revision_No;
        let pageNo = 0;
        let totalPage = 0;
        let obj;
        if (drawingDetails.length > 0 && ItemView != null) {
            obj = Object.values(drawingDetails)[parseInt(ItemView)];
            pageNo = parseInt(obj.currentPage);
            totalPage = obj.totalPage;
        }
        const newrects = originalRegions.map((item) => {
            if (item.Page_No === pageNo) {
                return item;
            }
            return false;
        }).filter(item => item !== false);
        // if there is no ballloon on current page then process
        let rotation = bgImgRotation + ROTATE;
        if (newrects.length > 0) {
            // if (rotation === 360) { rotation = 0; }
            //   useStore.setState({
            //      bgImgRotation: rotation
            //   })
            //  console.log(useStore.getState() )
            return false;
        }
        let nrotation = drawingDetails[ItemView].rotation;
        rotation = parseInt(nrotation) + ROTATE;
       
        if (Math.sign(rotation) === -1 && Math.abs(rotation) === 90) { rotation = 270; }
        if (rotation === 360) { rotation = 0; }
        let requestData = {
            ItemView: ItemView,
            drawingNo: drawingNo,
            revNo: revNo,
            drawingDetails: CurrentItem,
            pageNo: pageNo,
            totalPage: totalPage,
            sessionUserId: sessionId,
            rotation: rotation
        };
        //console.log(requestData)
        useStore.setState({ isLoading: true, loadingText: "Rotating an Image...", ItemView: null });
        rotateProcessApi(requestData).then(r => {
            return r.data;
        })
            .then(r => {
               // console.log(r, "left")
                let draw = drawingDetails.map((item) => {
                    if (item.currentPage.toString() === pageNo.toString()) {
                        return { ...item, rotation: rotation };
                    }
                    return item;
                })

                //useStore.setState({ drawingDetails: draw, isLoading: true, loadingText: "Loading an Image...", ItemView: null });
                useStore.setState({ drawingDetails: draw, isLoading: true, loadingText: "Loading an Image...", ItemView: view });

                //   console.log(r,view)

                return r;

            }, (e) => {
                console.log("Error", e);
                useStore.setState({ isLoading: false });
            }).catch(e => {
                console.log("catch", e);
                useStore.setState({ isLoading: false });
            })

       // console.log(useStore.getState())

        let scrollElement = document.querySelector('#konvaMain');
        scrollElement.scrollLeft = (scrollElement.scrollWidth - scrollElement.clientWidth) / 2;
        return false;
    };

    fitToActualsize = (e) => {
        e.preventDefault();
        if (config.console)
            console.log("fit to actual size")
        useStore.setState({ zoomingfactor: 0, zoomed: false });
        useStore.setState({ scrollPosition: 0 });
        actualSize();
    };

    fitToFullSize = (e) => {
        e.preventDefault();
        useStore.setState({ zoomingfactor: 0, zoomed: false });
        useStore.setState({ isDisabledZoomOut: false });
        useStore.setState({ isDisabledZoomIn: false });
        useStore.setState({ scrollPosition: 0 });
        // console.log(this.state.width)
        //  useStore.setState({ history: [], win: initialState.win });
        useStore.setState({ history: [], fitscreen: true, win: { width: window.innerWidth -100, height: window.innerHeight } });
        let props = useStore.getState();
        if (props.isErrImage) {
            return;
        }
         
        const width = props.win.width;
      //  const width = window.innerWidth;
        const height = props.win.height;
      //  const height = window.innerHeight;
        const aspectRatio = width / height;

        let newWidth;
        let newHeight;

        const imageRatio = props.imageWidth / props.imageHeight;

        if (aspectRatio >= imageRatio) {
            newWidth = width;
            newHeight = width / aspectRatio;
        } else {
            newWidth = height * aspectRatio;
            newHeight = height;
        }
        let win = { width: newWidth, height: newHeight };
        //console.log(newWidth, newHeight)
        useStore.setState({ win: win });
        setTimeout(function () {
            let state = useStore.getState();
            let w = state.imageWidth;
            let h = state.imageHeight;
 
            let scaleX = state.win.width / w;
            let scaleY = state.win.height / h;

            let scale = Math.min(scaleX, scaleY);
            let nw = 0;
            let nh = 0;
            let x = 0;
            let y = 0;
            nw = w * scale;
            nh = h * scale;
            // let newScaleX = newWidth / nw;
           // let newScaleY = newHeight / nh;
           // nw = nw * (newScaleX / 2);
           // nh = nh * (newScaleY / 2);
           // console.log(scale , newScaleX, newScaleY)
            //scale = scale * (newScaleX / 4); 
           // let sub = (1 - scale);
            //const step = sub / 5; // 5 step to view
            console.log("fit",nw, nh, initialState.win)
            let winwidth = state.win.width;
          //  let winwidth = window.innerWidth;
            
            if (nw < state.win.width) {
                x = (winwidth - nw) / 2;
            }
            let win = { width: winwidth, height: nh };
          //  let win = { width: window.innerWidth, height: window.innerHeight };
            let rpobj = { win: win, bgImgScale: scale, bgImgW: nw, bgImgH: nh, bgImgX: x, bgImgY: y };
            useStore.setState(rpobj);/*
           // console.log(" raja", rpobj)
            let nstate1 = useStore.getState();
            let pageNo = 0;
            let resize = "false";
            let superScale = [];
            let reScale = 0;
            if (nstate1.drawingDetails.length > 0 && nstate1.ItemView != null) {
                pageNo = parseInt(Object.values(nstate1.drawingDetails)[parseInt(nstate1.ItemView)].currentPage);
                resize = nstate1.drawingDetails.length > 0 ? Object.values(nstate1.drawingDetails)[parseInt(nstate1.ItemView)].resize : "false";
                superScale = nstate1.partial_image.filter((a) => { return a.item === parseInt(nstate1.ItemView); });
            }
     
            let rescale = 1;
            if (resize === "true") {
                // console.log(superScale,pageNo,reScale)
              //  rescale = superScale[0].scale + 0.5;
            }
            let nscale = nstate1.scaleStep + nstate1.InitialScale;
            let nw1 = parseInt(nstate1.bgImgW * (nscale * rescale), 10);
            let nh1 = parseInt(nstate1.bgImgH * (nscale * rescale), 10);
            let x1 = 0;
            let y1 = 0;
            //if (config.console)
                console.log(superScale, pageNo, reScale, window.innerWidth, window.innerHeight)
            if (nstate1.imageWidth > nw1 && nstate1.imageHeight > nh1) {

                if (nw1 > nstate1.win.width || nh1 > nstate1.win.height) {
                    let newwin = {
                        width: (nw1 > nstate1.win.width ? (nw1 ) : (nstate1.win.width)),
                        height: (nh1 > nstate1.win.height ? (nh1 ) : (nstate1.win.height))
                    }
                    useStore.setState({ win: newwin });
                }
                let newstate = useStore.getState();
                // if (nw < newstate.win.width) {
                x1 = (newstate.win.width - nw1) / 2;
                // }
                // if (nh < newstate.win.height) {
                y1 = (newstate.win.height - nh1) / 2;
                // }
                // console.log('resized')
            }
            // console.log(nw, nh, x1, y1, scale, nscale, nstate.win, initialState.win, resize)
            let zobj = { bgImgScale: nscale, bgImgW: nw1, bgImgH: nh1, bgImgX: x1, bgImgY: y1 };
            useStore.setState(zobj);
            */

            let nstate = useStore.getState();
            let originalRegions = nstate.originalRegions;
            let newrect = newBalloonPosition(originalRegions, nstate);
            useStore.setState({
                savedDetails: false,
                drawingRegions: newrect,
                balloonRegions: newrect,
                //    isDisabledAutoB: true

            });

            //console.log("fitToActualsize ")
            let scrollElement = document.querySelector('#konvaMain');
           // console.log(scrollElement.scrollWidth)
            scrollElement.scrollLeft = (scrollElement.scrollWidth - scrollElement.clientWidth) / 2;

        }, 200);
    };

    resetBalloons = (e) => {
        e.preventDefault();
        const { originalRegions, ItemView, drawingDetails } = useStore.getState();
        let pageNo = 0;
        if (drawingDetails.length > 0 && ItemView !== null) {
            pageNo = parseInt(Object.values(drawingDetails)[parseInt(ItemView)].currentPage);
        }
        useStore.setState({ selectedRegion: "" })
        const check = originalRegions.filter(
            r => r.Page_No === pageNo
        );
        if (check.length === 0) {
            return;
        }

        Swal.fire({
            title: 'Are you sure want to reset balloons?',
            showCancelButton: true,
            confirmButtonText: 'Yes',
            allowOutsideClick: false,
            allowEscapeKey: false
        }).then((result) => {
            /* Read more about isConfirmed */
            if (result.isConfirmed) {
               
               resetBalloonsProcess();
            }
        });
    }

    deleteBalloon = (e) => {
        e.preventDefault();
        const state = useStore.getState();
        let selectAnnotation = state.selectAnnotation;
        if (selectAnnotation === null) {
            Swal.fire({
                icon: '',
                title: 'Oops...',
                text: 'Select a Balloon to Remove',
                footer: ''
            })
        } else {
            const selected = state.drawingRegions.filter(
                annotation => annotation.id === selectAnnotation
            );
            let ss = selected[0].ballonNo;
            Swal.fire({
                title: `Are you want to delete Balloon (${ss})?`,
                showCancelButton: true,
                confirmButtonText: 'Yes',
                allowOutsideClick: false,
                allowEscapeKey: false
            }).then((result) => {
                /* Read more about isConfirmed */
                if (result.isConfirmed) {
 
                    useStore.setState({ selectAnnotation: null })
                    const newAnnotations = state.drawingRegions.filter(
                        annotation => annotation.id !== selectAnnotation
                    );
                    const newannota = newAnnotations.map((item, i) => {
                        if (item.id !== i) {
                            // Update balloon property when removing/altering an element for UI
                            return { ...item, ballonNo: i + 1 };
                        }
                        return item;
                    })

                    const newRegions = state.originalRegions.filter(
                        reg => reg.id !== selectAnnotation
                    );
                    const newOrg = newRegions.map((item, i) => {
                        if (item.id !== i) {
                            // Update balloon property when removing/altering an element for Backend
                            return { ...item, ballonNo: i + 1 };
                        }
                        return item;
                    })
   
                    //console.log("originalRegions","handleKeyDown")
                    
                  //  useStore.setState({ draft: newOrg, is_BalloonDrawingSaved: false })
                    useStore.setState({ originalRegions: newOrg, is_BalloonDrawingSaved: false })
                    useStore.setState({ drawingRegions: newannota })
                    useStore.setState({ balloonRegions: newannota })
               
                } else {
                    
                }
            })
        }
    };

    reset = (e) => {
        e.preventDefault();
        let props = useStore.getState();
        const newrects = props.originalRegions.map((item) => {
            if (!item.isballooned) {
                return item;
            }
            return false;
        }).filter(item => item !== false);
        if ( newrects.length > 0) {
            if (!props.is_BalloonDrawingSaved) this.showAlertOnReset(props);
        } else {
            useStore.setState({
                ...initialState,
                draft: [],
                originalRegions: [], user: props.user, sessionId: props.sessionId
            });
        }
        return true;
    };

    showAlertOnReset = (props) => {
        Swal.fire({
            title: 'Are you want to Save changes?',
            showCancelButton: true,
            confirmButtonText: 'Save',
            allowOutsideClick: false,
            allowEscapeKey: false
        }).then((result) => {
            /* Read more about isConfirmed */
            if (result.isConfirmed) {
                useStore.setState({ originalRegions: props.originalRegions });
                this.saveBalloons();
            } else {
                useStore.setState({
                    ...initialState, originalRegions: [],
                    draft: []
                });
            }
        })
    }

    render() {
        let state = useStore.getState();
        let { drawingDetails, ItemView, originalRegions } = state;
        let pageNo = 0;
        if (drawingDetails.length > 0 && ItemView != null) {
            pageNo = parseInt(Object.values(drawingDetails)[parseInt(ItemView)].currentPage);
        }
        const newrects = originalRegions.map((item) => {
            if (item.Page_No === pageNo) {
                return item;
            }
            return false;
        }).filter(item => item !== false);
       // console.log(state.selectedRegion)

        return (
            <>
                <>
                    { /*
                <Navbar className="d-none navbar-expand-sm revisioninput navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" container="fluid" light>
                    <NavbarToggler onClick={this.toggleTopbar} className="mr-2" />
                    <Collapse className={classNames("input", { "show": !this.state.topbarIsOpen })} isOpen={!this.state.topbarIsOpen} navbar>
                        <Container className="d-none" >
                            <Row className=" pb-2">
                                <ul className="navbar-nav flex-grow">
                                    <li>
                                        <label >Production Order No&nbsp;</label>
                                        <input type="text" size="20" name="ProdOrdNum" className="ProdOrdNum" />
                                        <Button className="btn btn-primary buttons">Reterive</Button>
                                    </li>
                                </ul>
                            </Row>
                            <Row className=" pb-2">
                                <div className="navbar-nav flex-grow d-flex revisioninput">
                                    <Container  >
                                        <Row>
                                            <Col
                                                className="bg-light "

                                            ><div className="four-col-grid">
                                                    <div className="grid-fifty mb-3">
                                                        <label >Drawing No</label> <input type="text" size="6" name="DrawingNo" className="DrawingNo" />
                                                    </div>
                                                    <div className="grid-fifty mb-3">
                                                        <label >D.Revision No</label> <input type="text" size="6" name="RevNo" className="RevNo" />
                                                    </div>
                                                    <div className="grid-fifty mb-3">
                                                        <label >Part No</label> <input type="text" size="6" name="PartNo" className="PartNo" />
                                                    </div>
                                                    <div className="grid-fifty mb-3">
                                                        <label >P.Revision No</label> <input type="text" size="6" name="PartRevNo" className="PartRevNo" />
                                                    </div>
                                                    <div className="grid-fifty mb-3">
                                                        <label >&nbsp;</label>
                                                        <select name="RegionTypes" 
                                                            onChange={this.selectedRegion}
                                                            multiple={false}
                                                            defaultValue={state.selectedRegion}>
                                                            <option value="Multiple Selected Region">Multiple Selected Region</option>
                                                            <option value="Single Selected Region">Single Selected Region</option>
                                                            <option value="Unselected Region">Unselected Region</option>
                                                            <option value="Manual Drawn">Manual Drawn</option>
                                                            <option value="Full Image" >Full Image</option>

                                                        </select>
                                                    </div>
                                                </div>
                                            </Col>
                                        </Row>
                                        <Row>
                                            <Col
                                                className="bg-light  text-right">
                                                <div className="bg-light  text-right" style={{
                                                    float: "right",
                                                    "paddingTop": "4px"
                                                }}>
                                                    <Button className="btn btn-primary buttons">Auto Balloon</Button>
                                                    <Button className="btn btn-primary buttons">Save</Button>
                                                    <Button className="btn btn-primary buttons">Delete</Button>
                                                    <Button className="btn btn-primary buttons">Reset Balloons</Button>
                                                </div>
                                            </Col>
                                        </Row>

                                    </Container>
                                </div>
                            </Row>
                        </Container>
                    </Collapse>
                </Navbar>
               */}
               </>

                <div className="side-menu-tools " >
                    <Nav vertical className={this.state.width > 500 ? "p-1  justify-content-between" : "p-1  justify-content-center "}  >

                        <Nav vertical className=" " style={{padding:"0px 4px"} }>
                            <NavItem className="box d-none p-1 regions">
                                <select name="RegionTypes"
                                    onChange={this.selectedRegion}
                                    multiple={false}
                                    disabled={state.isDisabledRegion}
                                    value={state.selectedRegion}>
                                    <option value="Selected Region">Selected Region</option>
                                    <option value="Unselected Region">Unselected Region</option>
                                    <option value="Manual Drawn">Manual Drawn</option>
                                    <option value="Full Image" >Full Image</option>
                                    <option value="Spl" >SPL</option>

                                </select>
                            </NavItem>

                            <NavItem className="box">
                                <Button color="light"
                                    disabled={state.isDisabledAutoB}
                                    onClick={this.makeAutoballoon}
                                    onMouseOver={this.handleMouseOverAuto}
                                    onMouseOut={this.handleMouseOutAuto}
                                    className="light-btn FullProcess buttons primary"
                                    data-value="Full Image"
                                    active={state.selectedRegion === "Full Image" ? true : false}
                                >
                                    <div style={{ position: "relative" }}>
                                        <span className="PySCBInfoleft  EI48Lc" >
                                            Process
                                        </span>
                                    </div>
                                    <div className="play-button" style={{ position: "relative" }}>
                                        <> <Image name='play-button.svg' className="icon" alt="Process" />  </>
                                    </div>
                                    <div className="play-button-white" style={{ position: "relative" }}>
                                        <><Image name='play-button-white.svg' className="icon" alt="Process" />  </>
                                    </div>

                                </Button>
                                <div className="apW" role="heading" aria-level="2">
                                    {"Process"}
                                </div>
                            </NavItem>

                            <NavItem className="box">
                                <Button color="light"
                                    disabled={state.isDisabledSelected}
                                    onClick={this.changeRegion}
                                    onMouseOver={this.handleMouseOverSelected}
                                    onMouseOut={this.handleMouseOutSelected}
                                    className="light-btn SelectedRegion buttons primary"
                                    data-value="Selected Region"
                                    active={state.selectedRegion === "Selected Region" ? true : false}
                                >
                                    <div style={{ position: "relative" }}>
                                        <span className="PySCBInfoleft EI48Lc" aria-hidden={this.state.isHoveringSelected} style={{ display: this.state.isHoveringSelected ? "block" : "none" }} >
                                            {this.state.isHoveringSelected && (
                                                "Balloon Region(s)"
                                            )}
                                        </span>
                                    </div>
                                    <div className="dotted-square" style={{ position: "relative" }}>
                                        <> <Image name='dotted-square.svg' className="icon" alt="Balloon Region(s)" /> </>
                                    </div>
                                    <div className="dotted-square-white" style={{ position: "relative" }}>
                                        <><Image name='dotted-square-white.svg' className="icon" alt="Balloon Region(s)" /> </>
                                    </div>
                                </Button>
                                <div className="apW" role="heading" aria-level="2">
                                    {"Selected"}
                                </div>
                            </NavItem>

                            <NavItem className="box">
                                <Button color="light"
                                    disabled={state.isDisabledUnSelected}
                                    onClick={this.changeRegion}
                                    onMouseOver={this.handleMouseOverUnSelected}
                                    onMouseOut={this.handleMouseOutUnSelected}
                                    className="light-btn UnselectedRegion buttons primary"
                                    data-value="Unselected Region"
                                    active={state.selectedRegion === "Unselected Region" ? true : false}
                                >
                                    <div style={{ position: "relative" }}>
                                        <span className="PySCBInfoleft EI48Lc"  >
                                                Unselected Region
                                        </span>
                                    </div>
                                    <div className="square-cross" style={{ position: "relative" }}>
                                        <>  <Image name='square-cross.svg' className="icon" alt="Unselected Region" /> </>
                                    </div>
                                    <div className="square-cross-white" style={{ position: "relative" }}>
                                        <>  <Image name='square-cross-white.svg' className="icon" alt="Unselected Region" /> </>
                                    </div>
                                </Button>
                                <div className="apW" role="heading" aria-level="2">
                                    {"Unselected"}
                                </div>
                            </NavItem>
                            <NavItem className="box">
                                <Button color="light "
                                    onClick={this.changeRegion}
                                    disabled={state.isDisabledSPL}
                                    onMouseOver={this.handleMouseOverSPL}
                                    onMouseOut={this.handleMouseOutSPL}
                                    className="light-btn SPLBalloon primary buttons"
                                    data-value="Spl"
                                    active={state.selectedRegion === "Spl" ? true : false}
                                >
                                    <div style={{ position: "relative" }}>
                                        <span className="PySCBInfoleft EI48Lc" >
                                                SPL Balloon
                                        </span>
                                    </div>
         
                                    <div className="spl" style={{ position: "relative" }}>
                                        <Image name='spl.svg' className="icon" alt="SPL Balloon" />
                                    </div>
                                    <div className="spl-white" style={{ position: "relative" }}>
                                        <Image name='spl-white.svg' className="icon"  alt="SPL Balloon" />
                                    </div>


                                </Button>
                                <div className="apW" role="heading" aria-level="2">
                                    {"SPL"}
                                </div>
                            </NavItem>

                            <NavItem className="box">
                                <Button color="light "
                                    onClick={this.resetBalloons}
                                    disabled={state.isDisabledResetB}
                                    onMouseOver={this.handleMouseOverResetB}
                                    onMouseOut={this.handleMouseOutResetB}
                                    className="light-btn ResetBalloons primary buttons"
                                >
                                    <div style={{ position: "relative" }}>
                                        <span className="PySCBInfoleft EI48Lc">
                                            Reset Balloons
                                        </span>
                                    </div>
                                    <div className="reset" style={{ position: "relative" }}>
                                        <Image name='reset.svg' className="icon" alt="Reset Balloons" />
                                         </div>
                                    <div className="reset-white" style={{ position: "relative" }}>
                                        <Image name='reset-white.svg' className="icon" alt="Reset Balloons" />
                                     </div>


                                </Button>
                                <div className="apW" role="heading" aria-level="2">
                                    {"Reset"}
                                </div>
                            </NavItem>

                        </Nav>

                        <Nav className="d-none">
                        <Row>
                            <Col className="   text-right">
                                <div className="   text-right" style={{
                                    float: "right",
                                    "paddingTop": "4px"
                                    }}>
                                    <Nav>
                                        
                                          
                                            <Button color="light"
                                                disabled={state.isDisabledManual}
                                                onClick={this.changeRegion}
                                                onMouseOver={this.handleMouseOverManual}
                                                onMouseOut={this.handleMouseOutManual}
                                                className="light-btn buttons primary"
                                                data-value="Manual Drawn"
                                            >
                                                <div style={{ position: "relative" }}>
                                                    <span className="PySCBInfo EI48Lc" aria-hidden={this.state.isHoveringManual} style={{ display: this.state.isHoveringManual ? "block" : "none" }} >
                                                        {this.state.isHoveringManual && (
                                                            "Manual Drawn"
                                                        )}
                                                    </span>
                                                </div>
                                                {!this.state.isHoveringManual && (
                                                    <>
                                                        <div className="d-flex" style={{ fontSize: "16px" }}>
                                                        <Image name='manual_new.svg' className="icon-manual" alt="Manual Drawn" />
                                                         {"+"} 
                                                         </div>
                                                    </>
                                                )}
                                                {this.state.isHoveringManual && (
                                                    <>
                                                        <div className="d-flex" style={{ fontSize: "16px" }}>
                                                            <Image name='manual-new-white.svg' className="icon-manual" alt="Manual Drawn" />
                                                             {"+"} 
                                                        </div>
                                                    </>
                                                )}
                                                

                                            </Button>

                                            <Button color="light" className="light-btn buttons primary"
                                                onClick={this.saveBalloons}
                                            disabled={state.isDisabledSave}
                                            onMouseOver={this.handleMouseOverSave}
                                            onMouseOut={this.handleMouseOutSave}>
                                            <div style={{ position: "relative" }}>
                                                <span className="PySCBInfo EI48Lc" aria-hidden={this.state.isHoveringSave} style={{ display: this.state.isHoveringSave ? "block" : "none" }} >
                                                    {this.state.isHoveringSave && (
                                                        "Save"
                                                    )}
                                                </span>
                                            </div>
                                            {!this.state.isHoveringSave && (<Image name='save.svg' className="icon" alt="Save" />)}
                                            {this.state.isHoveringSave && (<Image name='save-white.svg' className="icon" alt="Save" />)}

                                            </Button>

                                            <Button color="light" className="light-btn buttons primary"
                                                onClick={this.deleteBalloon}
                                            disabled={state.isDisabledDel}
                                            onMouseOver={this.handleMouseOverDel}
                                            onMouseOut={this.handleMouseOutDel}>
                                            <div style={{ position: "relative" }}>
                                                <span className="PySCBInfo EI48Lc" aria-hidden={this.state.isHoveringDel} style={{ display: this.state.isHoveringDel ? "block" : "none" }} >
                                                    {this.state.isHoveringDel && (
                                                        "Delete"
                                                    )}
                                                </span>
                                            </div>
                                            {!this.state.isHoveringDel && (<Image name='delete.svg' className="icon" alt="Delete"  />)}
                                            {this.state.isHoveringDel && (<Image name='delete-white.svg' className="icon" alt="Delete"   />)}
                                        </Button>
                                            
                                           
                                    </Nav>
                                </div>
                            </Col>
                        </Row>
                        </Nav>

                        <Nav vertical className="border-top" style={{ paddingLeft: "0px", marginTop:"10px" }}>

                            <NavItem className="box">
                                <Button color="light" className="light-btn screen-size buttons primary"
                                    disabled={state.isDisabledACT}
                                    onMouseOver={this.handleMouseOverACT}
                                    onMouseOut={this.handleMouseOutACT}
                                    onClick={this.fitToActualsize}
                                >
                                    <div style={{ position: "relative" }}>
                                        <span className="PySCBInfoleft EI48Lc"   >
                                                Screen Size
                                        </span>
                                    </div>
                                    
                                    <div className="actualsize" style={{ position: "relative" }}>
                                        <><Image name='actual-size.svg' className="icon" alt="ActualSize" /></>
                                    </div>
                                    <div className="actualsizewhite" style={{ position: "relative" }}>
                                    <><Image name='actual-size-white.svg' className="icon" alt="ActualSize" /></>
                                    </div>
                                </Button>
                                <div className="apW" role="heading" aria-level="2">
                                    {"Screen Size"}
                                </div>
                            </NavItem>

                            <NavItem className="box">
                                <Button color="light" className="light-btn FitImage buttons primary"
                                    disabled={state.isDisabledFIT}
                                    onMouseOver={this.handleMouseOverFIT}
                                    onMouseOut={this.handleMouseOutFIT}
                                    onClick={this.fitToFullSize}
                                >
                                    <div style={{ position: "relative" }}>
                                        <span className="PySCBInfoleft EI48Lc"  >
                                                FitImage
                                        </span>
                                    </div>
                                    <div className="fit-size" style={{ position: "relative" }}>
                                        <><Image name='fit-size.svg' className="icon" alt="FitImage" /></>
                                    </div>
                                    <div className="fit-size-white" style={{ position: "relative" }}>
                                        <><Image name='fit-size-white.svg' className="icon" alt="FitImage" /></>
                                    </div>
                                </Button>
                                <div className="apW" role="heading" aria-level="2">
                                    {"Fit Screen"}
                                </div>
                            </NavItem>


                            <NavItem className="box">
                                <Button color="light" className="light-btn ZoomIn buttons primary"
                                    onClick={this.handleZoomIn}
                                    disabled={state.isDisabledZoomIn}
                                    onMouseOver={this.handleMouseOverZoomIn}
                                    onMouseOut={this.handleMouseOutZoomIn}
                                >
                                    <div style={{ position: "relative" }}>
                                        <span className="PySCBInfoleft EI48Lc"  >
                                                ZoomIn
                                        </span>
                                    </div>
                                    <div className="magnifier-plus" style={{ position: "relative" }}>
                                        <><Image name='magnifier-plus.svg' className="icon" alt="ZoomIn" /></>
                                    </div>
                                    <div className="magnifier-plus-white" style={{ position: "relative" }}>
                                        <><Image name='magnifier-plus-white.svg' className="icon" alt="ZoomIn" /></>
                                    </div>

                                </Button>
                                <div className="apW" role="heading" aria-level="2">
                                    {"Zoom In"}
                                </div>
                            </NavItem>

                            <NavItem className="box">
                                <Button color="light" className="light-btn ZoomOut buttons primary"
                                    onClick={this.handleZoomOut}
                                    disabled={state.isDisabledZoomOut}
                                    onMouseOver={this.handleMouseOverZoomOut}
                                    onMouseOut={this.handleMouseOutZoomOut}
                                >
                                    <div style={{ position: "relative" }}>
                                        <span className="PySCBInfoleft EI48Lc"  >
                                                ZoomOut
                                        </span>
                                    </div>
                                    <div className="magnifier-minus" style={{ position: "relative" }}>
                                        <><Image name='magnifier-minus.svg' className="icon" alt="ZoomOut" /></>
                                    </div>
                                    <div className="magnifier-minus-white" style={{ position: "relative" }}>
                                        <><Image name='magnifier-minus-white.svg' className="icon" alt="ZoomOut" /></>
                                    </div>
                                </Button>
                                <div className="apW" role="heading" aria-level="2">
                                    {"Zoom Out"}
                                </div>
                            </NavItem>

                            <NavItem className="box d-none">
                                <Button color="light" className="light-btn RotateLeft buttons primary"
                                    disabled={state.isDisabledCCW}
                                    onMouseOver={this.handleMouseOverCCW}
                                    onMouseOut={this.handleMouseOutCCW}
                                    onClick={this.rotateLeftBackgroundImage}
                                >
                                    <div style={{ position: "relative" }}>
                                        <span className="PySCBInfoleft EI48Lc" >
                                            {newrects.length === 0 && (
                                                "Rotate Left"
                                            )}
                                            {newrects.length > 0  && (
                                                "After Balloon Process Rotate Left is not allowed"
                                            )}
                                        </span>
                                    </div>
                                    <div className="RotateCCW" style={{ position: "relative" }}>
                                        <><Image name='RotateCCW.svg' className="icon" alt="Rotate Left" /></>
                                    </div>
                                    <div className="RotateCCW-white" style={{ position: "relative" }}>
                                    <><Image name='RotateCCW-white.svg' className="icon" alt="Rotate Left" /></>
                                    </div>
                                </Button>
                                <div className="apW" role="heading" aria-level="2">
                                    {"Rotate Left"}
                                </div>
                            </NavItem>
                            <NavItem className="box d-none">
                                <Button color="light" className="light-btn RotateRight buttons primary"
                                    disabled={state.isDisabledCW}
                                    onMouseOver={this.handleMouseOverCW}
                                    onMouseOut={this.handleMouseOutCW}
                                    onClick={this.rotateRightBackgroundImage}

                                >
                                    <div style={{ position: "relative" }}>
                                        <span className="PySCBInfoleft EI48Lc"  >
                                            {newrects.length === 0 && this.state.isHoveringCW && (
                                                "Rotate Right"
                                            )}
                                            {newrects.length > 0 && this.state.isHoveringCW && (
                                                "After Balloon Process Rotate Right is not allowed"
                                            )}
                                        </span>
                                    </div>
                                    <div className="RotateCW" style={{ position: "relative" }}>
                                        <><Image name='RotateCW.svg' className="icon" alt="Rotate Right" /></>
                                    </div>
                                    <div className="RotateCW-white" style={{ position: "relative" }}>
                                        <><Image name='RotateCW-white.svg' className="icon" alt="Rotate Right" /></>
                                    </div>
                                </Button>
                                <div className="apW" role="heading" aria-level="2">
                                    {"Rotate Right"}
                                </div>
                            </NavItem>
                            <>
                                {/** 
                        <NavItem className="box">
                                <Button color="light" className="light-btn"
                                   disabled={state.isDisabledMAG}
                                onMouseOver={this.handleMouseOverMAG}
                                onMouseOut={this.handleMouseOutMAG}
                            >
                                <div style={{ position: "relative" }}>
                                    <span className="PySCBInfo EI48Lc" aria-hidden={this.state.isHoveringMAG} style={{ display: this.state.isHoveringMAG ? "block" : "none" }} >
                                        {this.state.isHoveringMAG && (
                                            "Magnifier"
                                        )}
                                    </span>
                                </div>
                                <Image name='magnifying.svg' className="icon" alt="Magnifier"   />

                            </Button>
                        </NavItem>

                        <NavItem className="box">
                            <Button color="light" className="light-btn"
                                disabled={state.isDisabledFH}
                                onMouseOver={this.handleMouseOverFH}
                                onMouseOut={this.handleMouseOutFH}
                            >
                            <div style={{ position: "relative" }}>
                                <span className="PySCBInfo EI48Lc" aria-hidden={this.state.isHoveringFH} style={{ display: this.state.isHoveringFH ? "block" : "none" }} >
                                    {this.state.isHoveringFH && (
                                        "Freehand"
                                    )}
                                </span>
                            </div>
                            <Image name='hand-free.svg' className="icon" alt="Freehand"   />

                        </Button>
                        </NavItem>
                        **/ }
                            </>
                        </Nav>
                        <>
                            {/** 
                    <Nav>
                        <Row>
                            <Col className="   text-right">
                                <div className="   d-flex "  >
                                    <NavItem className="box p-1">
                                            <Button color="light" disabled={state.isDisabledOCR} className="light-btn buttons primary" >OCR</Button>
                                    </NavItem>
                                    <NavItem className="box p-1">
                                            <Button color="light" disabled={state.isDisabledMove}  className="light-btn buttons primary" >Move</Button>
                                        </NavItem>
                                        
                                </div>
                            </Col>
                            </Row>
                        </Nav>
                        **/ }
                        </>

                    </Nav>
                     
                </div>
            </>
        );
    }
}
