import React, { Component } from "react";
import { Modal, ModalHeader, ModalBody, Button, ModalFooter, Table, Form, FormGroup, Label, Input, Row, Col } from "reactstrap";
import useStore from "../Store/store";
import Image from './Image';
import { Tab, Tabs, TabList, TabPanel } from 'react-tabs';
import 'react-tabs/style/react-tabs.css';
import Swal from 'sweetalert2'
import { v1 as uuid } from "uuid";
import Draggable from "react-draggable";
//import { shortBalloon, newBalloonPosition, deleteBalloonProcessApi, saveBalloonsApi, recKey, orgKey, specificationUpdateApi } from '../Common/Common';
import { config, newBalloonPosition, specificationUpdateApi, specAutoPopulateApi } from '../Common/Common';

 
class PopupModal extends Component {
    constructor(props) {
        super(props);
    
        this.state = {
            popupShown: false,
            modalData: null,
            modal: false,
            nestedModal: false,
            closeAll:false,
            backdrop: "static",
            selectedType: 6,
            selectedSubType: "others",
            selectedUnit: 0,
            selectedQuantity: 0,
            selectedTolerance: "",
            pTolerance: 0,
            mTolerance: 0,
            maxValue: 0,
            dynamicMinStepValue: 0.1,
            minValue:0,
            start:0,
            end: 0,
            Specification: "",
            popSpecification: "",
            isHoveringDel: false,
            isHoveringSave: false,
            isballooned: false,
            issubBalloon: false,

            tolerance_symbol: "",
            tolerance_check: false,
            cb_tolerance_1: "",
            cb_tolerance_2: "",
            cb_datum_a: "",
            cb_datum_1: "",
            cb_datum_b: "",
            cb_datum_2: "",
            cb_datum_c: "",
            cb_datum_3: ""
        }
        this.handleMouseOverDel = this.handleMouseOverDel.bind(this);
        this.handleMouseOutDel = this.handleMouseOutDel.bind(this);

        this.handleMouseOverSave = this.handleMouseOverSave.bind(this);
        this.handleMouseOutSave = this.handleMouseOutSave.bind(this);
        this.onButtonClickHandler = this.onButtonClickHandler.bind(this);
        this.toggleNested = this.toggleNested.bind(this);
   

    }
    inputref = ref => (this.inputref = ref)
 
    componentDidUpdate(oldProps) {
        //console.log(this.onFirstDataRendered, "update")
        if (oldProps.selectedBalloon !== this.props.selectedBalloon && this.props.selectedBalloon === null) {
            this.onHidePopup();
        }
        if (oldProps.selectedBalloon !== this.props.selectedBalloon && this.props.selectedBalloon !== null) {
            this.setState({ modalData: null });
            this.setState({ modalData: this.props.selectedBalloon, modal: true, popupShown: true });
            const state = useStore.getState();
            let originalRegions = state.originalRegions;
            let lmtype = state.lmtype;
            let lmsubtype = state.lmsubtype;
            let units = state.units;

            let newrects = originalRegions.map((item) => {
                if (parseInt(item.Balloon) === parseInt(this.props.selectedBalloon) ) {
                    return item;
                }
                return false;
            }).filter(item => item !== false);
            //console.log(newrects, "didupdate")
            this.setState({ isballooned: newrects[0].isballooned });
            if (newrects.length > 1) {
                newrects = originalRegions.map((item) => {
                     //console.log(item.Balloon, this.props.selectedBalloon)
                    if (this.props.selectedBalloon !== null && item.Balloon.toString() === this.props.selectedBalloon.toString()) {
                        return item;
                    }
                    return false;
                }).filter(item => item !== false);
                //console.log(newrects, "didupdate")
                this.setState({ isballooned: newrects[0].isballooned });
                this.setState({ issubBalloon: (!newrects[0].hasOwnProperty("subBalloon")) ? true :false });
            }
            //console.log(newrects, originalRegions, this.props.selectedBalloon)
             //console.log(lmsubtype, newrects[0])
            if (newrects[0].Type === "") {

                let st = lmtype.filter((item, i) => { return 6 === item.type_ID; });
                let newType = st[0].type_ID;
                this.setState({ selectedType: newType });
            } else {
                let st = lmtype.filter((item, i) => { return newrects[0].Type === item.type_Name; });
                let newType = st[0].type_ID;
                this.setState({ selectedType: newType });
            }
            if (newrects[0].SubType !== "" && newrects[0].SubType !== "Default") {
               
                let sst = lmsubtype.filter((item, i) => { return newrects[0].SubType === item.subType_Name; });
                this.setState({ selectedSubType: sst[0].subType_ID });
            } else {
                
              //  let sst = lmsubtype.filter((item, i) => { return "others" === item.subType_ID; });
               // console.log("else", sst)
                this.setState({ selectedSubType: "others" });
            }
            this.setState({ selectedUnit: units[0] });

            if (newrects[0].Quantity !== "") {
                this.setState({ selectedQuantity: newrects[0].Quantity });
            } else {

                this.setState({ selectedQuantity: 1 });
            }

            if (newrects[0].Spec !== "") {
                this.setState({ Specification: newrects[0].Spec, popSpecification: newrects[0].Spec });
            } else {
                this.setState({ Specification: "", popSpecification: "" });
            }

            if (newrects[0].ToleranceType !== "") {
                let cmbTolerance = state.cmbTolerance;
                let tolerance_select = "";
                 cmbTolerance.filter((item, i) => {
                     if (item === newrects[0].ToleranceType)
                         tolerance_select = i;
                     return item
                 });
                //console.log(tolerance_select)
                this.setState({ selectedTolerance: tolerance_select });
            } else {
                this.setState({ selectedTolerance: 1 });
            }

            if (newrects[0].PlusTolerance !== "") {
                this.setState({ pTolerance: newrects[0].PlusTolerance.toString() });
            } else {
                this.setState({ pTolerance: "+0" });
            }

            if (newrects[0].MinusTolerance !== "") {
                this.setState({ mTolerance: newrects[0].MinusTolerance.toString() });
            } else {
                this.setState({ mTolerance: "-0" });
            }

            if (newrects[0].Maximum !== "") {
                this.setState({ maxValue: newrects[0].Maximum });
            } else {
                this.setState({ maxValue: "0" });
            }

            if (newrects[0].Minimum !== "") {
                this.setState({ minValue: newrects[0].Minimum });
            } else {
                this.setState({ minValue: "0" });
            }

            //console.log("init",this.state)
           }
    }

    onHidePopup = (e) => {
        const state = useStore.getState();
        //console.log(state.scrollPosition)
        let s = this.props.selectedBalloon;
        //console.log(s)
        //const resetOverData = JSON.parse(JSON.stringify(state.originalRegions));
        const resetOverData = [...state.originalRegions];
        let newrects = resetOverData.map((item) => {
            // console.log(item.Balloon, this.props.selectedBalloon)
            if (!item.isballooned && s !== null && item.Balloon.toString() === s.toString()) {
                return item;
            }
            return false;
        }).filter(item => item !== false);
         
        if (newrects.length > 0) {
            //console.log(newrects)
            let deletedOrg = resetOverData.map((item) => {
                if (parseInt(item.Balloon) !== parseInt(s)) {
                    return item;
                }
                return false;
            }).filter(item => item !== false);
 
            //return true;
            var newStore = deletedOrg.slice();
            var overData = Object.values(resetOverData)[0];
            if (parseInt(s) - 1 > 0) {
                let overTemp = resetOverData.filter((item) => { return parseInt(item.Balloon) === parseInt(s) - 1; });
                overData = Object.values(overTemp)[overTemp.length -1];
            }

            let qtyi = 0;
            // get all quantity parent
            let Qtyparent = resetOverData.reduce((res, item) => {
                if (item.hasOwnProperty("subBalloon") && item.subBalloon.length >= 0 && item.Quantity > 1) {
                    res[qtyi] = item;
                    qtyi++;
                }
                return res;
            }, []);

            var fromIndex = deletedOrg.indexOf(overData);
            var count = state.originalRegions.filter(function (item) {
                return parseInt(item.Balloon) === parseInt(s);
            });
           // console.log(count)
            let changedsingle = [];
            if (count.length > 1) {
                var clone = state.originalRegions.filter(function (item) {
                    return parseInt(item.Balloon) === parseInt(s);
                });
 
                var cloneFirst = clone[0];
                if (cloneFirst.subBalloon.length > 0)
                    cloneFirst.subBalloon.pop();

               
                const id = uuid();
                if (cloneFirst.Quantity === 1 && cloneFirst.subBalloon.length === 0) {
                    let pb = parseInt(cloneFirst.Balloon).toString() ;
                    let newarr = { ...cloneFirst.newarr, Balloon: pb };
                    changedsingle.push({ ...cloneFirst, newarr: newarr, id: id, DrawLineID: 0, Balloon: pb });
                }
                if (cloneFirst.Quantity === 1 && cloneFirst.subBalloon.length > 0) {
                    let pb = parseInt(cloneFirst.Balloon).toString() + ".1";
                    let newarr = { ...cloneFirst.newarr, Balloon: pb };
                    changedsingle.push({ ...cloneFirst, newarr: newarr, id: id, DrawLineID: 0, Balloon: pb });

                    let newSubItem = cloneFirst.subBalloon.filter(a => {
                        return a.isDeleted === false && a.isballooned === true;
                    });
                    newSubItem.map(function (e, ei) {
                        let sno = ei + 2;
                        const sid = uuid();
                        let b = parseInt(cloneFirst.Balloon).toString() + "." + sno.toString();
                        let newarr = { ...e.newarr, Balloon: b };
                        changedsingle.push({ ...e, newarr: newarr, id: sid, DrawLineID: 0, Balloon:b  });
                        return e;
                    })
                }
                if (cloneFirst.Quantity > 1 && cloneFirst.subBalloon.length === 0) {
                    for (let qi = 1; qi <= cloneFirst.Quantity; qi++) {
                        const qid = uuid();
                        let pb = parseInt(cloneFirst.Balloon).toString() + "." + qi.toString();
                        let newarr = { ...cloneFirst.newarr, Balloon: pb };
                        changedsingle.push({ ...cloneFirst, newarr: newarr, id: qid, DrawLineID: 0, Balloon: pb });
          
                    }
                }
                if (cloneFirst.Quantity > 1 && cloneFirst.subBalloon.length > 0) {
                    for (let qi = 1; qi <= cloneFirst.Quantity; qi++) {
                        const qid = uuid();
                        let pb = parseInt(cloneFirst.Balloon).toString() + "." + qi.toString();
                        let newMainItem = Qtyparent.map(item => {
                            if (parseInt(cloneFirst.Balloon) === parseInt(item.Balloon) && pb === item.Balloon) {
                                return item;
                            }

                            return false;
                        }).filter(x => x !== false);
                        if (config.console)
                            console.log("newMainItem", newMainItem)
                        if (newMainItem.length > 0) {
                            let nmi = newMainItem[0];
                            nmi.subBalloon.pop()
                            let newarr = { ...nmi.newarr, Balloon: pb };
                            changedsingle.push({ ...nmi, newarr: newarr, id: qid, DrawLineID: 0, Balloon: pb });
                            let newSubItem = nmi.subBalloon.filter(a => {
                                return a.isDeleted === false && a.isballooned === true;
                            });
                            newSubItem.reduce(function (p, e, ei) {
                                let sqno = ei + 1;
                                const sqid = uuid();
                                let b = pb + "." + sqno.toString();
                                    p.push(b);
                                let newarr = { ...e.newarr, Balloon: b };
                                changedsingle.push({ ...e, newarr: newarr, id: sqid, DrawLineID: 0, Balloon: b });
                                return p;
                            }, [])

                        }

                    }
                }
                newStore.splice(fromIndex+1, 0, ...changedsingle);
               // console.log(newStore, fromIndex, changedsingle, cloneFirst);
            }
            
            //return true;
            newStore = newStore.map((item, i) => {

                const id = uuid();
                item.intBalloon = parseInt(item.Balloon);
                const isInteger = item.Balloon % 1 === 0;
                if (isInteger) {
                    item.hypenBalloon = item.Balloon;
                } else {
                    item.hypenBalloon = item.Balloon.replaceAll(".", "-");
                }
                item.id = id;
                if (item.hasOwnProperty("newarr")) {
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
            }
                item.DrawLineID = i + 1;
                return item;
            });
             if (config.console)
            console.log(newStore)
            useStore.setState({
                originalRegions: newStore,
                draft: newStore,
                savedDetails: ((newStore.length > 0) ? true : false),
                drawingRegions: [],
                balloonRegions: []
            });
            const newstate = useStore.getState();
            if (newstate.savedDetails) {
                let originalRegions = newstate.originalRegions;
                let newrect = newBalloonPosition(originalRegions, newstate);
                useStore.setState({
                    drawingRegions: newrect,
                    balloonRegions: newrect,
                });
            }
            
             
        }



        this.setState({ modalData: null, popupShown: false, modal: !this.state.modal });
        useStore.setState({ selectedBalloon: null });
        const new1state = useStore.getState();
        //console.log(new1state.scrollPosition)

        setTimeout(function () {
            let scrollElement = document.querySelector('#konvaMain');
            if (scrollElement !== null) {
                scrollElement.scrollLeft = new1state.scrollPosition;
            }

        }, 500);

    };
  

    handleMouseOverDel() { this.setState({ isHoveringDel: true }); }
    handleMouseOutDel() { this.setState({ isHoveringDel: false }); }

    handleMouseOverSave() { this.setState({ isHoveringSave: true }); }
    handleMouseOutSave() { this.setState({ isHoveringSave: false }); }

    autoPopulateDetails = (e) => {
        e.preventDefault();
        
        if (this.props.selectedBalloon !== null) {
            setTimeout(() => {
                const { lmtype, lmsubtype, cmbTolerance, units, originalRegions } = useStore.getState();
            let st = lmtype.filter((item, i) => {
                return parseInt(item.type_ID) === parseInt(this.state.selectedType);
            });
            let type = st[0].type_Name;
            let sst = lmsubtype.filter((item, i) => {
                //console.log(item, this.state.selectedSubType )
                if (this.state.selectedSubType !== "others") {
                    return parseInt(item.subType_ID) === parseInt(this.state.selectedSubType);
                } else {
                    return "others" === item.subType_ID;
                }
            });

                let _this = this.state;
                if (config.console)
                console.log("before this", _this)
                let quantity = (_this.selectedQuantity);
            let Spec = _this.Specification;
            let subType = sst[0].subType_Name;
            let tolerance_select = cmbTolerance.filter((item, i) => { return parseInt(i) === parseInt(_this.selectedTolerance); });
            let toleranceType = tolerance_select[0];
            let unit = _this.selectedUnit;

            let plusTolerance = _this.pTolerance;
            let presult = /\+(:?\s+)?\d+(\.\d+)?/i.test(plusTolerance);
            plusTolerance = !presult ? "+" + plusTolerance : plusTolerance.replace(/\s\s+/g, '');
            this.setState({ pTolerance: plusTolerance });

            let minusTolerance = _this.mTolerance;
            let mresult = /-(:?\s+)?\d+(\.\d+)?/i.test(minusTolerance);
            minusTolerance = !mresult ? "-" + minusTolerance : minusTolerance.replace(/\s\s+/g, '');
                this.setState({ mTolerance: minusTolerance });
                let maximum = (typeof _this.maxValue === "undefined") ? "0":(_this.maxValue);
                let minimum = (typeof _this.minValue === "undefined") ? "0" : (_this.minValue);

                if (maximum !== "") {
                    this.setState({ maxValue: maximum });
                } else {
                    this.setState({ maxValue: "0" });
                }

                if (minimum !== "") {
                    this.setState({ minValue: minimum });
                } else {
                    this.setState({ minValue: "0" });
                }

            if (typeof toleranceType === "undefined") {
                toleranceType = cmbTolerance[1];
            }
            if (Spec.trim().length === 0) {
                return false;
            }
                let req = {};
                _this = this.state;
                if (config.console)
                console.log("after this", _this)
            const newrects = originalRegions.map((item) => {
                if (item.Balloon.toString() === this.props.selectedBalloon.toString()) {
                    let x = item.newarr.Crop_X_Axis;
                    let y = item.newarr.Crop_Y_Axis;
                    let width = item.newarr.Crop_Width;
                    let height = item.newarr.Crop_Height;
                    item.newarr.Type = type;
                    item.newarr.SubType = subType;
                    item.newarr.Unit = unit;
                    item.newarr.ToleranceType = toleranceType;
                    item.newarr.Quantity = parseInt(quantity);
                    item.newarr.Spec = Spec;
                    item.newarr.PlusTolerance = plusTolerance;
                    item.newarr.MinusTolerance = minusTolerance;
                    item.newarr.Maximum = maximum;
                    item.newarr.Minimum = minimum;
                    return { ...item, ...item.newarr, x, y, width, height, selectedRegion:"" };
                }
                return false;
            }).filter(x=> x!==false);
            if (config.console)
                    console.log("Balloon set state", newrects)
                let newSpec = Spec;
                if (parseInt(quantity) > 1 ) {
                    newSpec = quantity + "X " + Spec;
                }
            req = {
                spec: newSpec,
                originalRegions: newrects,
                plusTolerance: plusTolerance,
                toleranceType: toleranceType,
                minusTolerance: minusTolerance,
                maximum: maximum,
                minimum: minimum
            };
            if (config.console)
                console.log("req    ", req)

                setTimeout(() =>
                    specAutoPopulateApi(req).then(r => {
                        return r.data;
                    })
                        .then(r => {
                            if (config.console)
                            console.log(r, "spec Modal Box")
                            const o = r.reduce((acc, curr) => {
                                acc[curr.key] = curr.value;
                                return acc;
                            }, {});

                            if (o.Type === "") {
                                let st = lmtype.filter((item, i) => { return 6 === item.type_ID; });
                                let newType = st[0].type_ID;
                                this.setState({ selectedType: newType });
                            } else {
                                let st = lmtype.filter((item, i) => { return o.Type === item.type_Name; });
                                let newType = st[0].type_ID;
                                this.setState({ selectedType: newType });
                            }

                            if (o.SubType !== "" && o.SubType !== "Default") {
                                let sst = lmsubtype.filter((item, i) => { return o.SubType === item.subType_Name; });
                                this.setState({ selectedSubType: sst[0].subType_ID });
                            } else {
                                this.setState({ selectedSubType: "others" });
                            }

                            this.setState({ selectedUnit: units[0] });

                            if (o.Quantity !== "") {
                                this.setState({ selectedQuantity: (o.Quantity) });
                            } else {
                                this.setState({ selectedQuantity: 1 });
                            }
                            
                            if (Spec !== "") {
                              // this.setState({ Specification: Spec });
                                this.setState({ popSpecification: Spec });
 
                            } else {
                               // this.setState({ Specification: "" });
                                this.setState({ popSpecification: "" });
                            }

                            if (o.ToleranceType !== "") {
                                let tolerance_select = "";
                                cmbTolerance.filter((item, i) => {
                                    if (item === o.ToleranceType)
                                        tolerance_select = i;
                                    return item
                                });
                                this.setState({ selectedTolerance: tolerance_select });
                            } else {
                                this.setState({ selectedTolerance: 1 });
                            }

                            if (o.PlusTolerance !== "") {
                                this.setState({ pTolerance: o.PlusTolerance.toString() });
                            } else {
                                this.setState({ pTolerance: "+0" });
                            }

                            if (o.MinusTolerance !== "") {
                                this.setState({ mTolerance: o.MinusTolerance.toString() });
                            } else {
                                this.setState({ mTolerance: "-0" });
                            }

                            if (o.Max !== "") {
                                this.setState({ maxValue: (o.Max) });
                            } else {
                                this.setState({ maxValue: ("0") });
                            }

                            if (o.Min !== "") {
                                this.setState({ minValue: (o.Min) });
                                
                            } else {
                                this.setState({ minValue: ("0") });
                            }
                            
                            if (config.console)
                            console.log(o, this.state, "spec Modal Box")
                        }, (e) => {
                            console.log("Error", e);
                        }).catch(e => {
                            console.log("catch", e);
                        })

                    , 80);
                
            }, 100);
            
        }
        
        return false;
    };


    saveBalloon = (e) => {
        e.preventDefault();
        if (config.console)
            console.log("save")
        if (this.props.selectedBalloon !== null) {
            const state = useStore.getState();
            const { lmtype, lmsubtype, cmbTolerance, originalRegions, draft } = state;

            //console.log(this.state)
            let st = lmtype.filter((item, i) => {
                return parseInt(item.type_ID) === parseInt(this.state.selectedType);
            });

            let type = st[0].type_Name;
            let sst = lmsubtype.filter((item, i) => {
                //console.log(item, this.state.selectedSubType )
                if (this.state.selectedSubType !== "others") {
                    return parseInt(item.subType_ID) === parseInt(this.state.selectedSubType);
                } else {
                    return "others" === item.subType_ID;
                }

            });

            let _this = this.state;
            let quantity = _this.selectedQuantity;
            let Spec = _this.Specification;
            let subType = sst[0].subType_Name;
            let tolerance_select = cmbTolerance.filter((item, i) => { return parseInt(i) === parseInt(_this.selectedTolerance); });
            let toleranceType = tolerance_select[0];
            let unit = _this.selectedUnit;

            let plusTolerance = _this.pTolerance;
            let presult = /\+(:?\s+)?\d+(\.\d+)?/i.test(plusTolerance.trim());
            plusTolerance = !presult ? "+" + plusTolerance.trim() : plusTolerance.trim().replace(/\s\s+/g, '');
            this.setState({ pTolerance: plusTolerance });

            let minusTolerance = _this.mTolerance;
            let mresult = /-(:?\s+)?\d+(\.\d+)?/i.test(minusTolerance.trim());
            minusTolerance = !mresult ? "-" + minusTolerance.trim() : minusTolerance.trim().replace(/\s\s+/g, '');
            this.setState({ mTolerance: minusTolerance });

            let maximum = _this.maxValue;
            let minimum = _this.minValue;

            if (typeof toleranceType === "undefined") {
                toleranceType = cmbTolerance[1];
            }
            if (Spec.trim().length === 0) {
                Swal.fire({
                    title: 'Alert!',
                    icon: "",
                    html: "Specification should not be blank.",
                    showConfirmButton: false,
                    timer: 2500
                });
                return false;
            }
            //const overData = JSON.parse(JSON.stringify(originalRegions));
            const overData = [...originalRegions];
            const dup = overData.map((i) => {
                return i;
            });
            // console.log(this.state, originalRegions)
            // return false;
            // let presult = /^(\+?\s)/i.test(plusTolerance);
            // let pt = presult ? plusTolerance.replace(/(\+?\s)/mg, "") : plusTolerance;
            // let mresult = /^(\-?\s)/i.test(minusTolerance);
            // let mt = mresult ? minusTolerance.replace(/(\-?\s)/mg, "") : minusTolerance;

            // const newrects = originalRegions.map((item) => {
            //     //console.log(item.Balloon.toString(), this.props.selectedBalloon.toString(), item.isballooned)
            //     if (item.Balloon.toString() === this.props.selectedBalloon.toString()) {
            //         let x = item.newarr.Crop_X_Axis;
            //         let y = item.newarr.Crop_Y_Axis;
            //         let width = item.newarr.Crop_Width;
            //         let height = item.newarr.Crop_Height;
            //         item.newarr.Type = type;
            //         item.newarr.SubType = subType;
            //         item.newarr.Unit = unit;
            //         item.newarr.ToleranceType = toleranceType;
            //         item.newarr.Quantity = parseInt(quantity);
            //         item.newarr.Spec = Spec;
            //         item.newarr.PlusTolerance = plusTolerance;
            //         item.newarr.MinusTolerance = minusTolerance;
            //         item.newarr.Maximum = maximum;
            //         item.newarr.Minimum = minimum;
            //         return { ...item, Type: type, SubType: subType, Unit: unit, ToleranceType: toleranceType, Quantity: parseInt(quantity), Spec, PlusTolerance: plusTolerance, MinusTolerance: minusTolerance, Maximum: maximum, Minimum: minimum, x, y, width, height };
            //     }
            //     return item;
            // });
        const newrects = originalRegions.map((item) => {
        const selected = this.props.selectedBalloon.toString();
        const selectedParts = selected.split('.');
        const itemParts = item.Balloon.toString().split('.');

        const selectedSpec = Spec; // the new spec you're trying to apply
        const selectedParent = selectedParts[0]; // "23" from "23-1" or "23-1-1"

        // Helper: check if spec is same
        const isSameSpec = (a, b) => {
            console.log("related a b",a,b);
            return a == b
        };

        // Level 1 - direct match, update only that balloon
        if (selectedParts.length === 1) {
            if (item.Balloon.toString() === selected) {
            return updateBalloon(item); // Update single
            }
            return item;
        }

        // Level 2 - Handle 23-1, 23-2, 23-3
        if (selectedParts.length === 2) {
            const selectedGroup = selectedParts[0]; // "23"

            // Filter all in same group
            const groupItems = originalRegions.filter(i => 
            i.Balloon.toString().startsWith(selectedGroup + '.') &&
            i.Balloon.toString().split('.').length === 2
            );
            const firstSpec =groupItems.length > 0 && groupItems[0]?.Spec;
            console.log("related groupItems",groupItems,firstSpec);
            
            const allSameSpec = groupItems.every(i => isSameSpec(i.Spec, firstSpec));
            console.log("related allSameSpec",allSameSpec);
            

            if (
            item.Balloon.toString().startsWith(selectedGroup + '.') &&
            item.Balloon.toString().split('.').length === 2
            ) {
            if (allSameSpec) {
                return updateBalloon(item); // Update all in group
            } else if (item.Balloon.toString() === selected) {
                return updateBalloon(item); // Only selected if specs differ
            }
            }

            return item;
        }

        // Level 3 - Handle 23-1-1, 23-2-1, etc.
        if (selectedParts.length === 3) {
            const selectedRoot = selectedParts[0]; // "23"
            const selectedChildIndex = selectedParts[2]; // e.g., "1" from "23-1-1"
            console.log("related selectedRoot ",selectedRoot,"selectedChildIndex",selectedChildIndex);
            

            const matchingItems = originalRegions.filter(i => {
            const parts = i.Balloon.toString().split('.');
            return parts.length === 3 &&
                parts[0] === selectedRoot &&
                parts[2] === selectedChildIndex 
                // && isSameSpec(i.Spec, selectedSpec);
            });
            console.log("related matchingItems",matchingItems);
            
            const shouldUpdate = matchingItems.some(i => i.Balloon.toString() === item.Balloon.toString());
            console.log("related should update",shouldUpdate);
            
            if (shouldUpdate) {
            return updateBalloon(item); // update all same spec + subindex
            }

            // Also update parent balloons like 23-1 or 23-2 if selected is 23-1
            const parentLevel2Match = (
            item.Balloon.toString().split('.').length === 2 &&
            item.Balloon.toString().startsWith(selectedRoot + '.') &&
            isSameSpec(item.Spec, selectedSpec) &&
            selectedParts[1] === item.Balloon.toString().split('.')[1]
            );

            if (parentLevel2Match) {
            return updateBalloon(item);
            }

            return item;
        }

        return item; // default, no match
        });

        // Helper to update a balloon object
        function updateBalloon(item) {
        let { Crop_X_Axis: x, Crop_Y_Axis: y, Crop_Width: width, Crop_Height: height } = item.newarr;
        item.newarr.Type = type;
        item.newarr.SubType = subType;
        item.newarr.Unit = unit;
        item.newarr.ToleranceType = toleranceType;
        item.newarr.Quantity = parseInt(quantity);
        item.newarr.Spec = Spec;
        item.newarr.PlusTolerance = plusTolerance;
        item.newarr.MinusTolerance = minusTolerance;
        item.newarr.Maximum = maximum;
        item.newarr.Minimum = minimum;

        return {
            ...item,
            Type: type,
            SubType: subType,
            Unit: unit,
            ToleranceType: toleranceType,
            Quantity: parseInt(quantity),
            Spec,
            PlusTolerance: plusTolerance,
            MinusTolerance: minusTolerance,
            Maximum: maximum,
            Minimum: minimum,
            x, y, width, height
        };
        }

            if (config.console)
                console.log("Balloon set state", newrects)
            //return false;
            useStore.setState({ originalRegions: newrects });
            useStore.setState({ isLoading: true, loadingText: "Saving Balloon... Please Wait..." });
            let nstate = useStore.getState();

            let isNew = nstate.originalRegions.map((item) => {
                //console.log(item.Balloon.toString(), this.props.selectedBalloon.toString(), item.isballooned)
                if (item.Balloon.toString() === this.props.selectedBalloon.toString() && !item.isballooned) {

                    return item;
                }
                return false;
            }).filter((i) => i !== false);
            if (config.console)
                console.log(isNew, isNew.length, dup);
            //useStore.setState({ isLoading: false });
            //return false;
            const oldRegions = dup.map((item) => {
                if (config.console)
                    console.log(item.Balloon, this.props.selectedBalloon)
                if (item.Balloon === this.props.selectedBalloon) {
                    return item;
                }
                return false;
            }).filter((i) => i !== false);

            let single = Object.assign({}, oldRegions[0].newarr);
            let dummy = false;
            let subballoon = false;
            let update = false;
            const isInteger = isNew.Balloon % 1 === 0;
            if (isNew.length === 1 && isInteger) {
                dummy = true;
                if (config.console)
                    console.log("dummy balloon", single)
            }

            if (isNew.length === 1 && !isInteger) {
                quantity = 1;
                subballoon = true;
                if (config.console)
                    console.log("sub balloon", single)
            }
            if (isNew.length === 0) {
                update = true;
                if (config.console)
                    console.log("old data update", oldRegions)
            }

            const id = uuid();
            single = { ...single, id: id, selectedRegion: "" }
            let oldvalue = [];
            oldvalue.push(single);
            if (config.console)
                console.log("oldvalue ", oldvalue)

            let newSpec = Spec;
            if (quantity > 1 && isNew.length === 0) {
                newSpec = quantity + "X " + Spec;
            }

            let req = {
                spec: newSpec,
                originalRegions: oldvalue,
                plusTolerance: plusTolerance,
                toleranceType: toleranceType,
                minusTolerance: minusTolerance,
                maximum: maximum,
                minimum: minimum
            };
            /*
            if (Spec.trim().length > 0 && isNew.length > 0) {
                const xcheck = Spec.toLowerCase().includes("x");
                const box = Spec.toLowerCase().indexOf("box") !== -1;
                if (!box && xcheck) {
                    Swal.fire({
                        title: 'Alert!',
                        icon: "",
                        html: "The sub balloon should be single quantity.",
                        showConfirmButton: false,
                        timer: 2500
                    });
                    useStore.setState({ isLoading: false });
                    return false;
                }
            }
            */
            if (config.console)
                console.log("req    ", req)
            // useStore.setState({ isLoading: false });

            //return false;
            setTimeout(() =>
                specificationUpdateApi(req).then(r => {
                    return r.data;
                })
                    .then(r => {
                        if (config.console)
                            console.log(r, "Save Modal Box")

                        let nominal = "";
                        let mainType = "";
                        let datetime = "";
                        r.map((a) => {
                            if (a.key === "Min") { minimum = a.value; }
                            if (a.key === "Max") { maximum = a.value; }
                            if (a.key === "Nominal") { nominal = a.value; }
                            if (a.key === "Type") { mainType = a.value; }
                            if (a.key === "SubType") { subType = a.value; }
                            if (a.key === "Unit") { unit = a.value; }
                            if (a.key === "ToleranceType") { toleranceType = a.value; }
                            if (a.key === "PlusTolerance") { plusTolerance = a.value; }
                            if (a.key === "MinusTolerance") { minusTolerance = a.value; }
                            if (a.key === "Num_Qty") { quantity = parseInt(a.value); }
                            if (a.key === "Date") { datetime = a.value; }
                            return a;
                        });
                        let updatedsingle = [];
                        const { ItemView, drawingDetails, originalRegions } = useStore.getState();
                        const resetOverDataparent = JSON.parse(JSON.stringify(originalRegions));
                        let resetOverData = [...originalRegions];
                        let qtyi = 0;
                        // get all quantity parent
                        let Qtyparentid = resetOverDataparent.reduce((res, item) => {
                            if (item.hasOwnProperty("subBalloon") && item.subBalloon.length >= 0 && item.Quantity > 1) {
                                res[qtyi] = item.Balloon;
                                qtyi++;
                            }
                            return res;
                        }, []);
                        let Qtyparent = resetOverData.map((item) => {
                            if (Qtyparentid.includes(item.Balloon)) {
                                return item;
                            }
                            return false;
                        }).filter(a => a !== false);
                        if (config.console)
                            console.log(Qtyparent)

                        if (dummy) {
                            if (config.console)
                                console.log(single.Quantity, quantity, r);

                            updatedsingle = resetOverData.map((item) => {
                                if (parseInt(item.Balloon) === parseInt(this.props.selectedBalloon)) {
                                    let news = {}
                                    news.Minimum = minimum;
                                    news.Maximum = maximum;
                                    news.Nominal = nominal;
                                    news.Type = mainType;
                                    news.SubType = subType;
                                    news.Unit = unit;
                                    news.ToleranceType = toleranceType;
                                    news.PlusTolerance = plusTolerance;
                                    news.MinusTolerance = minusTolerance;
                                    news.ModifiedDate = datetime;
                                    news.Quantity = parseInt(quantity);
                                   
                                    news.isDeleted = false;
                                    news.isballooned = true;

                                    if (item.hasOwnProperty("newarr")) {
                                        item.newarr.Minimum = minimum;
                                        item.newarr.Maximum = maximum;
                                        item.newarr.Nominal = nominal;
                                        item.newarr.Type = mainType;
                                        item.newarr.SubType = subType;
                                        item.newarr.Unit = unit;
                                        item.newarr.ToleranceType = toleranceType;
                                        item.newarr.PlusTolerance = plusTolerance;
                                        item.newarr.MinusTolerance = minusTolerance;
                                        item.newarr.ModifiedDate = datetime;
                                        item.newarr.Quantity = parseInt(quantity);
                                        item.newarr.isDeleted = false;
                                        item.newarr.isballooned = true;

                                    }
                                    return { ...item, ...news };
                                }
                                return item;
                            });
                            if (single.Quantity !== parseInt(quantity)) {

                                let deletableOrg = updatedsingle.map((item) => {
                                    if (parseInt(item.Balloon) === parseInt(this.props.selectedBalloon)) {
                                        return item;
                                    }
                                    return false;
                                }).filter(item => item !== false);
                                let deletedOrg = updatedsingle.map((item) => {
                                    if (parseInt(item.Balloon) !== parseInt(this.props.selectedBalloon)) {
                                        return item;
                                    }
                                    return false;
                                }).filter(item => item !== false);
                                let changedsingle = [];
                                let cloneFirst = deletableOrg[0];
                                if (quantity > 1) {
                                    for (let qi = 1; qi <= quantity; qi++) {
                                        const qid = uuid();
                                        let pb = parseInt(cloneFirst.Balloon).toString() + "." + qi.toString();
                                        let newarr = { ...cloneFirst.newarr, Balloon: pb };
                                        changedsingle.push({ ...cloneFirst, newarr: newarr, id: qid, DrawLineID: 0, Balloon: pb });
                                    }

                                } else {
                                    changedsingle = [cloneFirst].map((item) => {
                                        item.Balloon = parseInt(item.Balloon).toString();
                                        item.newarr.Balloon = parseInt(item.Balloon).toString();
                                        return item;
                                    })
                                }

                                var overData = Object.values(nstate.originalRegions)[0];
                                if (parseInt(this.props.selectedBalloon) - 1 > 0) {
                                    let overTemp = nstate.originalRegions.filter((item) => { return parseInt(item.Balloon) === parseInt(this.props.selectedBalloon) - 1; });
                                    overData = Object.values(overTemp)[overTemp.length - 1];

                                }
                                var newStore = deletedOrg.slice();
                                var fromIndex = deletedOrg.indexOf(overData);
                                newStore.splice(fromIndex + 1, 0, ...changedsingle);
                                updatedsingle = newStore;
                            }
                            if (config.console)
                                console.log(updatedsingle)

                            updatedsingle = updatedsingle.map((item, i) => {
                                let w = parseInt(item.newarr.Crop_Width * 1);
                                let h = parseInt(item.newarr.Crop_Height * 1);
                                let x = parseInt(item.newarr.Crop_X_Axis * 1);
                                let y = parseInt(item.newarr.Crop_Y_Axis * 1);
                                let cx = parseInt(item.newarr.Circle_X_Axis * 1);
                                let cy = parseInt(item.newarr.Circle_Y_Axis * 1);
                                const id = uuid();
                                item.intBalloon = parseInt(item.Balloon);
                                const isInteger = item.Balloon % 1 === 0;
                                if (isInteger) {
                                    item.hypenBalloon = item.Balloon;
                                } else {
                                    item.hypenBalloon = item.Balloon.replaceAll(".", "-");
                                }
                                item.id = id;
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
                                item.DrawLineID = i + 1;
                                return item;
                            });

                        }
                        if (subballoon) {
                           // if (config.console)
                                console.log("subballoon", resetOverData, Qtyparent)

                            let found = false;
                            let mainsub = originalRegions.map((item) => {
                                if (parseInt(item.Balloon) === parseInt(this.props.selectedBalloon) && !found) {
                                    found = true;
                                    item.subBalloon.map((news) => {
                                        if (!news.isballooned) {
                                            news.Minimum = minimum;
                                            news.Maximum = maximum;
                                            news.Nominal = nominal;
                                            news.Type = mainType;
                                            news.SubType = subType;
                                            news.Unit = unit;
                                            news.Spec = this.state.Specification;
                                            news.ToleranceType = toleranceType;
                                            news.PlusTolerance = plusTolerance;
                                            news.MinusTolerance = minusTolerance;
                                            news.ModifiedDate = datetime;
                                            news.Quantity = 1;
                                            news.isballooned = true;
                                            news.isDeleted = false;
 

                                            if (news.hasOwnProperty("newarr")) {
                                                news.newarr.Minimum = minimum;
                                                news.newarr.Maximum = maximum;
                                                news.newarr.Nominal = nominal;
                                                news.newarr.Type = mainType;
                                                news.newarr.SubType = subType;
                                                news.newarr.Unit = unit;
                                                news.newarr.Spec = this.state.Specification;
                                                news.newarr.ToleranceType = toleranceType;
                                                news.newarr.PlusTolerance = plusTolerance;
                                                news.newarr.MinusTolerance = minusTolerance;
                                                news.newarr.ModifiedDate = datetime;
                                                news.newarr.Quantity = 1;
                                                news.newarr.isballooned = true;
                                                news.newarr.isDeleted = false;
                                            }
                                            return news;
                                        }
                                        return news;
                                    });
                                    return item;
                                }
                                return false;
                            }).filter((i) => i !== false);

                            // useStore.setState({ isLoading: false });
                            // return false;
                            if (config.console)
                                console.log("subballoon", mainsub)

                            if (mainsub[0].Quantity > 1) {

                                let subballoonItem = [];
                                for (let qi = 1; qi <= mainsub[0].Quantity; qi++) {
                                    let pb = parseInt(mainsub[0].Balloon).toString() + "." + qi.toString();
                                    Qtyparent.map(item => {

                                        if (parseInt(mainsub[0].Balloon) === parseInt(item.Balloon) && pb === item.Balloon) {
                                            subballoonItem.push(parseInt(mainsub[0].Balloon).toString() + "." + + qi.toString() + "." + item.subBalloon.length.toString());
                                            item.subBalloon.map((news) => {
                                                if (!news.isballooned) {
                                                    let minimum = "", maximum = "", nominal = "", mainType = "", subType = "", unit = "", toleranceType = "", plusTolerance = "", minusTolerance = "", quantity = "", datetime = "";
                                                    r.map((a) => {
                                                        if (a.key === "Min") { minimum = a.value; }
                                                        if (a.key === "Max") { maximum = a.value; }
                                                        if (a.key === "Nominal") { nominal = a.value; }
                                                        if (a.key === "Type") { mainType = a.value; }
                                                        if (a.key === "SubType") { subType = a.value; }
                                                        if (a.key === "Unit") { unit = a.value; }
                                                        if (a.key === "ToleranceType") { toleranceType = a.value; }
                                                        if (a.key === "PlusTolerance") { plusTolerance = a.value; }
                                                        if (a.key === "MinusTolerance") { minusTolerance = a.value; }
                                                        if (a.key === "Num_Qty") { quantity = a.value; }
                                                        if (a.key === "Date") { datetime = a.value; }
                                                        return a;
                                                    });
                                                    news.Minimum = minimum;
                                                    news.Maximum = maximum;
                                                    news.Nominal = nominal;
                                                    news.Type = mainType;
                                                    news.SubType = subType;
                                                    news.Unit = unit;
                                                    news.Spec = this.state.Specification;
                                                    news.ToleranceType = toleranceType;
                                                    news.PlusTolerance = plusTolerance;
                                                    news.MinusTolerance = minusTolerance;
                                                    news.ModifiedDate = datetime;
                                                    news.Quantity = quantity;
                                                    news.isballooned = true;
                                                    news.isDeleted = false;
                                                    if (news.hasOwnProperty("newarr")) {
                                                        news.newarr.Minimum = minimum;
                                                        news.newarr.Maximum = maximum;
                                                        news.newarr.Nominal = nominal;
                                                        news.newarr.Type = mainType;
                                                        news.newarr.SubType = subType;
                                                        news.newarr.Unit = unit;
                                                        news.newarr.Spec = this.state.Specification;
                                                        news.newarr.ToleranceType = toleranceType;
                                                        news.newarr.PlusTolerance = plusTolerance;
                                                        news.newarr.MinusTolerance = minusTolerance;
                                                        news.newarr.ModifiedDate = datetime;
                                                        news.newarr.Quantity = quantity;
                                                        news.newarr.isballooned = true;
                                                        news.newarr.isDeleted = false;
                                                    }
                                                    return news;
                                                }
                                                return news;
                                            });

                                            return item;
                                        }
                                        return item;
                                    });
                                }
                                updatedsingle = resetOverData.map((item) => {
                                    if (subballoonItem.includes(item.Balloon)) {

                                        item.Minimum = minimum;
                                        item.Maximum = maximum;
                                        item.Nominal = nominal;
                                        item.Type = mainType;
                                        item.SubType = subType;
                                        item.Unit = unit;
                                        item.Spec = this.state.Specification;
                                        item.ToleranceType = toleranceType;
                                        item.PlusTolerance = plusTolerance;
                                        item.MinusTolerance = minusTolerance;
                                        item.ModifiedDate = datetime;
                                        item.Quantity = parseInt(quantity);
                                        item.isballooned = true;
                                        item.isDeleted = false;
                                        if (item.hasOwnProperty("newarr")) {
                                            item.newarr.Minimum = minimum;
                                            item.newarr.Maximum = maximum;
                                            item.newarr.Nominal = nominal;
                                            item.newarr.Type = mainType;
                                            item.newarr.SubType = subType;
                                            item.newarr.Unit = unit;
                                            item.newarr.Spec = this.state.Specification;
                                            item.newarr.ToleranceType = toleranceType;
                                            item.newarr.PlusTolerance = plusTolerance;
                                            item.newarr.MinusTolerance = minusTolerance;
                                            item.newarr.ModifiedDate = datetime;
                                            item.newarr.Quantity = parseInt(quantity);
                                            item.newarr.isballooned = true;
                                            item.newarr.isDeleted = false;
                                        }
                                        return item;
                                    }

                                    return item;
                                });

                            } else {

                                updatedsingle = resetOverData.map((item) => {
                                    if (item.Balloon.toString() === this.props.selectedBalloon.toString() && !item.isballooned) {
                                        item.Minimum = minimum;
                                        item.Maximum = maximum;
                                        item.Nominal = nominal;
                                        item.Type = mainType;
                                        item.SubType = subType;
                                        item.Unit = unit;
                                        item.Spec = this.state.Specification;
                                        item.ToleranceType = toleranceType;
                                        item.PlusTolerance = plusTolerance;
                                        item.MinusTolerance = minusTolerance;
                                        item.ModifiedDate = datetime;
                                        item.Quantity = parseInt(quantity);
                                        item.isballooned = true;
                                        item.isDeleted = false;
                                        if (item.hasOwnProperty("newarr")) {
                                            item.newarr.Minimum = minimum;
                                            item.newarr.Maximum = maximum;
                                            item.newarr.Nominal = nominal;
                                            item.newarr.Type = mainType;
                                            item.newarr.SubType = subType;
                                            item.newarr.Unit = unit;
                                            item.newarr.Spec = this.state.Specification;
                                            item.newarr.ToleranceType = toleranceType;
                                            item.newarr.PlusTolerance = plusTolerance;
                                            item.newarr.MinusTolerance = minusTolerance;
                                            item.newarr.ModifiedDate = datetime;
                                            item.newarr.Quantity = parseInt(quantity);
                                            item.newarr.isballooned = true;
                                            item.newarr.isDeleted = false;
                                        }
                                        return item;
                                    }
                                    return item;
                                });

                            }

                        }
                        if (update) {
                            let found = false;
                            let mainsub = resetOverData.map((item) => {
                                if (parseInt(item.Balloon) === parseInt(this.props.selectedBalloon) && !found) {
                                    found = true;
                                    return item;
                                }
                                return false;
                            }).filter((i) => i !== false);
                            if (config.console)
                                console.log("balloon update", resetOverData)

                            // sub balloon update
                            if (!oldRegions[0].hasOwnProperty("subBalloon") && mainsub[0].Quantity === 1) {
                                if (config.console)
                                    console.log("sub balloon update single")
                                const lastNumber = oldRegions[0].Balloon.match(/\d+(?=\D*$)/)[0];
                                found = false;
                                // update the main subBalloon
                                originalRegions.map((item) => {
                                    if (parseInt(item.Balloon) === parseInt(this.props.selectedBalloon) && !found) {
                                        found = true;
                                        item.subBalloon.map((news, index) => {
                                            if (index === lastNumber - 2) {
                                                news.Minimum = minimum;
                                                news.Maximum = maximum;
                                                news.Nominal = nominal;
                                                news.Type = mainType;
                                                news.SubType = subType;
                                                news.Unit = unit;
                                                news.Spec = this.state.Specification;

                                                news.ToleranceType = toleranceType;
                                                news.PlusTolerance = plusTolerance;
                                                news.MinusTolerance = minusTolerance;
                                                news.ModifiedDate = datetime;
                                                news.Quantity = 1;
                                                news.isballooned = true;
                                                news.newarr.Minimum = minimum;
                                                news.newarr.Maximum = maximum;
                                                news.newarr.Nominal = nominal;
                                                news.newarr.Type = mainType;
                                                news.newarr.SubType = subType;
                                                news.newarr.Unit = unit;
                                                news.newarr.Spec = this.state.Specification;

                                                news.newarr.ToleranceType = toleranceType;
                                                news.newarr.PlusTolerance = plusTolerance;
                                                news.newarr.MinusTolerance = minusTolerance;
                                                news.newarr.Spec = this.state.Specification;
                                                return news;
                                            }
                                            return news;
                                        });
                                        return item;
                                    }
                                    return false;
                                }).filter((i) => i !== false);
                                // current item update
                                updatedsingle = originalRegions.map((item) => {
                                    if ([oldRegions[0].Balloon].includes(item.Balloon)) {
                                        let news = {}
                                        news.Minimum = minimum;
                                        news.Maximum = maximum;
                                        news.Nominal = nominal;
                                        news.Type = mainType;
                                        news.SubType = subType;
                                        news.Unit = unit;
                                        news.Spec = this.state.Specification;
                                        news.ToleranceType = toleranceType;
                                        news.PlusTolerance = plusTolerance;
                                        news.MinusTolerance = minusTolerance;
                                        news.ModifiedDate = datetime;
                                        news.Quantity = parseInt(quantity);
                                        news.isballooned = true;

                                        news.isDeleted = false;
                                        if (item.hasOwnProperty("newarr")) {
                                            item.newarr.Minimum = minimum;
                                            item.newarr.Maximum = maximum;
                                            item.newarr.Nominal = nominal;
                                            item.newarr.Type = mainType;
                                            item.newarr.SubType = subType;
                                            item.newarr.Unit = unit;
                                            item.newarr.Spec = this.state.Specification;
                                            item.newarr.ToleranceType = toleranceType;
                                            item.newarr.PlusTolerance = plusTolerance;
                                            item.newarr.MinusTolerance = minusTolerance;
                                            item.newarr.ModifiedDate = datetime;
                                            item.newarr.Quantity = parseInt(quantity);
                                            item.newarr.isballooned = true;
                                            item.newarr.isDeleted = false;
                                        }
                                        return { ...item, ...news };
                                    }
                                    return item;
                                });
                            }
                            if (!oldRegions[0].hasOwnProperty("subBalloon") && mainsub[0].Quantity > 1) {
                                if (config.console)
                                    console.log("sub balloon update multi qty")
                                const lastNumber = oldRegions[0].Balloon.match(/\d+(?=\D*$)/)[0];
                                let regex = /^(\d*\.?\d*)/;
                                let prefix = oldRegions[0].Balloon.match(regex);
                                // update the main of subBalloon
                                for (let qi = 1; qi <= mainsub[0].Quantity; qi++) {
                                    let pb = parseInt(mainsub[0].Balloon).toString() + "." + qi.toString();
                                    let newMainItem = Qtyparent.map(item => {
                                        console.log("updating sub before", item)
                                        if (pb === item.Balloon && item.Balloon === prefix[0].toString()) {
                                            console.log("updating sub", item)
                                            /*
                                            item.subBalloon.map((news, index) => {
                                                if (index === lastNumber - 1) {
                                                    let minimum = "", maximum = "", nominal = "", mainType = "", subType = "", unit = "", toleranceType = "", plusTolerance = "", minusTolerance = "", quantity = "", datetime = "";
                                                    r.map((a) => {
                                                        if (a.key === "Min") { minimum = a.value; }
                                                        if (a.key === "Max") { maximum = a.value; }
                                                        if (a.key === "Nominal") { nominal = a.value; }
                                                        if (a.key === "Type") { mainType = a.value; }
                                                        if (a.key === "SubType") { subType = a.value; }
                                                        if (a.key === "Unit") { unit = a.value; }
                                                        if (a.key === "ToleranceType") { toleranceType = a.value; }
                                                        if (a.key === "PlusTolerance") { plusTolerance = a.value; }
                                                        if (a.key === "MinusTolerance") { minusTolerance = a.value; }
                                                        if (a.key === "Num_Qty") { quantity = a.value; }
                                                        if (a.key === "Date") { datetime = a.value; }
                                                        return a;
                                                    });
                                                    news.Minimum = minimum;
                                                    news.Maximum = maximum;
                                                    news.Nominal = nominal;
                                                    news.Type = mainType;
                                                    news.SubType = subType;
                                                    news.Unit = unit;
                                                    news.Spec = this.state.Specification;
                                                    news.ToleranceType = toleranceType;
                                                    news.PlusTolerance = plusTolerance;
                                                    news.MinusTolerance = minusTolerance;
                                                    news.ModifiedDate = datetime;
                                                    news.Quantity = quantity;
                                                    news.isballooned = true;
                                                    if (news.hasOwnProperty("newarr")) {
                                                        news.newarr.Minimum = minimum;
                                                        news.newarr.Maximum = maximum;
                                                        news.newarr.Nominal = nominal;
                                                        news.newarr.Type = mainType;
                                                        news.newarr.SubType = subType;
                                                        news.newarr.Unit = unit;
                                                        news.newarr.Spec = this.state.Specification;
                                                        news.newarr.ToleranceType = toleranceType;
                                                        news.newarr.PlusTolerance = plusTolerance;
                                                        news.newarr.MinusTolerance = minusTolerance;
                                                        news.newarr.ModifiedDate = datetime;
                                                        news.newarr.Quantity = quantity;
                                                    }
                                                    return news;
                                                }
                                                return news;
                                            });
                                            */
                                            return item;
                                        }
                                        return false;
                                    }).filter(x => x !== false);
                                    if (config.console)
                                        console.log("sub balloon update multi qty", newMainItem)
                                }
                                // update the main subBalloon
                                const deletableOrg = originalRegions.map((item) => {
                                    if ((item.Balloon) === (prefix[0])) {
                                        return item;
                                    }
                                    return false;
                                }).filter(item => item !== false);
                                let deletedOrg = originalRegions.map((item) => {
                                    if ((item.Balloon) !== (prefix[0])) {
                                        return item;
                                    }
                                    return false;
                                }).filter(item => item !== false);

                                const changedsingle = deletableOrg.map((mitem) => {
                                    const subBalloon = JSON.parse(JSON.stringify(mitem.subBalloon));
                                    subBalloon.map((item, i) => {
                                        if (i === lastNumber - 1) {
                                            item.Minimum = minimum;
                                            item.Maximum = maximum;
                                            item.Nominal = nominal;
                                            item.Type = mainType;
                                            item.SubType = subType;
                                            item.Unit = unit;
                                            item.Spec = this.state.Specification;
                                            item.ToleranceType = toleranceType;
                                            item.PlusTolerance = plusTolerance;
                                            item.MinusTolerance = minusTolerance;
                                            item.ModifiedDate = datetime;
                                            item.Quantity = parseInt(quantity);
                                            item.isballooned = true;
                                            item.isDeleted = false;
                                            if (item.hasOwnProperty("newarr")) {
                                                item.newarr.Minimum = minimum;
                                                item.newarr.Maximum = maximum;
                                                item.newarr.Nominal = nominal;
                                                item.newarr.Type = mainType;
                                                item.newarr.SubType = subType;
                                                item.newarr.Unit = unit;
                                                item.newarr.Spec = this.state.Specification;
                                                item.newarr.ToleranceType = toleranceType;
                                                item.newarr.PlusTolerance = plusTolerance;
                                                item.newarr.MinusTolerance = minusTolerance;
                                                item.newarr.ModifiedDate = datetime;
                                                item.newarr.Quantity = parseInt(quantity);
                                                item.newarr.isballooned = true;
                                                item.newarr.isDeleted = false;
                                            }
                                        }
                                        return item;
                                    });
                                    mitem.subBalloon = subBalloon;
                                    return mitem;
                                });

                                let overData = Object.values(originalRegions)[0];
                                if (parseInt(prefix[0]) - 1 > 0) {
                                    let overTemp = originalRegions.filter((item) => { return (item.Balloon) === (prefix[0]); });
                                    overData = Object.values(overTemp)[0];

                                }

                                let newitems = deletedOrg.slice();
                                let fromIndex = originalRegions.indexOf(deletableOrg[0]);
                                let toIndex = originalRegions.indexOf(overData);
                                newitems.splice(fromIndex, 0, ...changedsingle);
                                //let subBalloonm = [...sub];
                                console.log("sub balloon sssss", deletableOrg, changedsingle, fromIndex, toIndex, newitems)

                                //this.api.setFocusedCell(rowNo, 'start', 'top');
                                //const gridApi = agGridRef.current.api;
                                // Ensure the row at the specified index is visible
                                //gridApi.ensureIndexVisible(index, 'middle');

                                updatedsingle = resetOverData.map((item) => {
                                    if ((item.Balloon) === (this.props.selectedBalloon)) {
                                        item.Minimum = minimum;
                                        item.Maximum = maximum;
                                        item.Nominal = nominal;
                                        item.Type = mainType;
                                        item.SubType = subType;
                                        item.Unit = unit;
                                        item.Spec = this.state.Specification;
                                        item.ToleranceType = toleranceType;
                                        item.PlusTolerance = plusTolerance;
                                        item.MinusTolerance = minusTolerance;
                                        item.ModifiedDate = datetime;
                                        item.Quantity = 1;
                                        item.isballooned = true;
                                        item.isDeleted = false;
                                        if (item.hasOwnProperty("newarr")) {
                                            item.newarr.Minimum = minimum;
                                            item.newarr.Maximum = maximum;
                                            item.newarr.Nominal = nominal;
                                            item.newarr.Type = mainType;
                                            item.newarr.SubType = subType;
                                            item.newarr.Unit = unit;
                                            item.newarr.Spec = this.state.Specification;
                                            item.newarr.ToleranceType = toleranceType;
                                            item.newarr.PlusTolerance = plusTolerance;
                                            item.newarr.MinusTolerance = minusTolerance;
                                            item.newarr.ModifiedDate = datetime;
                                            item.newarr.Quantity = 1;
                                            item.newarr.isballooned = true;
                                            item.newarr.isDeleted = false;
                                        }
                                        return item;
                                    }
                                    return item;
                                });
                                updatedsingle = newitems;
                                if (config.console)
                                    console.log("balloon update af", updatedsingle)

                                /*
                                 originalRegions.map((item) => {
                                     if (parseInt(item.Balloon) === parseInt(this.props.selectedBalloon) && !found) {
                                         found = true;
                                         item.subBalloon.map((news, index) => {
                                             if (index === lastNumber - 1) {
                                                 news.Minimum = minimum;
                                                 news.Maximum = maximum;
                                                 news.Nominal = nominal;
                                                 news.Type = mainType;
                                                 news.SubType = subType;
                                                 news.Unit = unit;
                                                 news.Spec = this.state.Specification;
                                                 news.ToleranceType = toleranceType;
                                                 news.PlusTolerance = plusTolerance;
                                                 news.MinusTolerance = minusTolerance;
                                                 news.ModifiedDate = datetime;
                                                 news.Quantity = 1;
                                                 news.isballooned = true;
                                                 return news;
                                             }
                                             return news;
                                         });
                                         return item;
                                     }
                                     return false;
                                 }).filter((i) => i !== false);
                                  
 
                                 let subballoonItem = [];
                                 for (let qi = 1; qi <= mainsub[0].Quantity; qi++) {
                                     subballoonItem.push( parseInt(mainsub[0].Balloon).toString() + "." + qi.toString() + "." + lastNumber.toString());
                                 }
 
                         
                                 [mainsub[0]].map((item) => {
                                     item.subBalloon.map((news, index) => {
                                         if (index === lastNumber - 1) {
                                             news.Minimum = minimum;
                                             news.Maximum = maximum;
                                             news.Nominal = nominal;
                                             news.Type = mainType;
                                             news.SubType = subType;
                                             news.Unit = unit;
                                             news.Spec = this.state.Specification;
                                             news.ToleranceType = toleranceType;
                                             news.PlusTolerance = plusTolerance;
                                             news.MinusTolerance = minusTolerance;
                                             news.ModifiedDate = datetime;
                                             news.Quantity = 1;
                                             news.isballooned = true;
                                             if (news.hasOwnProperty("newarr")) {
                                                 news.newarr.Minimum = minimum;
                                                 news.newarr.Maximum = maximum;
                                                 news.newarr.Nominal = nominal;
                                                 news.newarr.Type = mainType;
                                                 news.newarr.SubType = subType;
                                                 news.newarr.Unit = unit;
                                                 news.newarr.Spec = this.state.Specification;
                                                 news.newarr.ToleranceType = toleranceType;
                                                 news.newarr.PlusTolerance = plusTolerance;
                                                 news.newarr.MinusTolerance = minusTolerance;
                                                 news.newarr.ModifiedDate = datetime;
                                                 news.newarr.Quantity = 1;
                                             }
                                             return news;
                                         }
                                         return news;
                                     });
                                     return item;
                                 });
                                 
                                 updatedsingle = originalRegions.map((item) => {
                                     if (subballoonItem.includes(item.Balloon)) {
                                        
                                         let news = {}
                                         news.Minimum = minimum;
                                         news.Maximum = maximum;
                                         news.Nominal = nominal;
                                         news.Type = mainType;
                                         news.SubType = subType;
                                         news.Unit = unit;
                                         news.Spec = this.state.Specification;
                                         news.ToleranceType = toleranceType;
                                         news.PlusTolerance = plusTolerance;
                                         news.MinusTolerance = minusTolerance;
                                         news.ModifiedDate = datetime;
                                         news.Quantity = 1;
                                         news.isballooned = true;
                                         if (item.hasOwnProperty("newarr")) {
                                             item.newarr.Minimum = minimum;
                                             item.newarr.Maximum = maximum;
                                             item.newarr.Nominal = nominal;
                                             item.newarr.Type = mainType;
                                             item.newarr.SubType = subType;
                                             item.newarr.Unit = unit;
                                             item.newarr.Spec = this.state.Specification;
                                             item.newarr.ToleranceType = toleranceType;
                                             item.newarr.PlusTolerance = plusTolerance;
                                             item.newarr.MinusTolerance = minusTolerance;
                                             item.newarr.ModifiedDate = datetime;
                                             item.newarr.Quantity = 1;
                                         }
                                         return { ...item, ...news };
                                     }
                                     return item;
                                 });
                                 */

                            }

                            // main item 
                            if (oldRegions[0].hasOwnProperty("subBalloon"))
                                if (config.console)
                                    console.log("main balloon update")
                            let changedsingle = [];

                            let pageNo = 0;

                            if (drawingDetails.length > 0 && ItemView != null) {
                                pageNo = Object.values(drawingDetails)[parseInt(ItemView)].currentPage;
                            }
                            let PageData = originalRegions.map((item) => {
                                if (parseInt(item.Page_No) === parseInt(pageNo)) {
                                    return item;
                                }
                                return false;
                            }).filter(item => item !== false);

                            let deletedOrg = originalRegions.map((item) => {
                                return item;
                            })



                            let fromIndex = PageData.indexOf(mainsub[0]);


                            let oldStore = draft.map((item) => {
                                if ((item.Balloon).toString() === (this.props.selectedBalloon).toString()) {

                                    return item;
                                }
                                return false;
                            }).filter(x => x !== false);
                            let orgStore = originalRegions.map((item) => {
                                if ((item.Balloon).toString() === (this.props.selectedBalloon).toString()) {

                                    return item;
                                }
                                return false;
                            }).filter(x => x !== false);

                            if (oldStore[0].Quantity !== orgStore[0].Quantity) {
                                deletedOrg = originalRegions.map((item) => {
                                    if (parseInt(item.Balloon) !== parseInt(this.props.selectedBalloon)) {
                                        return item;
                                    }
                                    return false;
                                }).filter(item => item !== false);
                            }

                            let newStore = [];
                             let addsubballoons=false

                            if (oldStore[0].Quantity === orgStore[0].Quantity) {
                                newStore = originalRegions.map((item) => {
                                    if ((item.Balloon).toString() === (this.props.selectedBalloon).toString()) {
                                        const o = r.reduce((acc, curr) => {
                                            acc[curr.key] = curr.value;
                                            return acc;
                                        }, {});

                                        item.Minimum = o.Min;
                                        item.Maximum = o.Max;
                                        item.Nominal = o.Nominal;
                                        item.Type = o.Type;
                                        item.SubType = o.SubType;
                                        item.Unit = o.Unit;
                                        item.Spec = this.state.Specification;
                                        item.ToleranceType = o.ToleranceType;
                                        item.PlusTolerance = o.PlusTolerance;
                                        item.MinusTolerance = o.MinusTolerance;
                                        item.ModifiedDate = o.Date;
                                        item.Quantity = parseInt(o.Num_Qty);
                                        item.isballooned = true;
                                        item.isDeleted = false;
                                        if (item.hasOwnProperty("newarr")) {
                                            item.newarr.Minimum = o.Min;
                                            item.newarr.Maximum = o.Max;
                                            item.newarr.Nominal = o.Nominal;
                                            item.newarr.Type = o.Type;
                                            item.newarr.SubType = o.SubType;
                                            item.newarr.Unit = o.Unit;
                                            item.newarr.Spec = this.state.Specification;
                                            item.newarr.ToleranceType = o.ToleranceType;
                                            item.newarr.PlusTolerance = o.PlusTolerance;
                                            item.newarr.MinusTolerance = o.MinusTolerance;
                                            item.newarr.ModifiedDate = o.Date;
                                            item.newarr.Quantity = parseInt(o.Num_Qty);
                                            item.newarr.isballooned = true;
                                            item.newarr.isDeleted = false;
                                        }
                                        return item;
                                    }
                                    return item;
                                });
                                let decreasedOrg = originalRegions.map((item) => {
                                    if (parseInt(item.Balloon) === parseInt(this.props.selectedBalloon)) {
                                        return item;
                                    }
                                    return false;
                                }).filter(item => item !== false);
                                const o = r.reduce((acc, curr) => {
                                    acc[curr.key] = curr.value;
                                    return acc;
                                }, {});
                                let c = {};
                                c.Minimum = o.Min;
                                c.Maximum = o.Max;
                                c.Nominal = o.Nominal;
                                c.Type = o.Type;
                                c.SubType = o.SubType;
                                c.Unit = o.Unit;
                                c.Spec = this.state.Specification;
                                c.ToleranceType = o.ToleranceType;
                                c.PlusTolerance = o.PlusTolerance;
                                c.MinusTolerance = o.MinusTolerance;
                                c.ModifiedDate = o.Date;
                                c.Quantity = parseInt(o.Num_Qty);


                                for (let qi = 1; qi <= orgStore[0].Quantity; qi++) {
                                    let pb = parseInt(mainsub[0].Balloon).toString() + "." + qi.toString();
                                    const qid = uuid();
                                    let newItem = decreasedOrg.filter((item) => { return item.Balloon === pb })
                                    if (newItem.length > 0) {
                                        let newSubItem = newItem[qi-1].subBalloon.filter(a => { return a.isDeleted === false; });
                                        if (parseInt(o.Num_Qty) > 1) {
                                            pb = parseInt(mainsub[0].Balloon).toString() + "." + qi.toString();
                                        } else {
                                            pb = parseInt(mainsub[0].Balloon).toString();
                                            if (newSubItem.length > 0) {
                                                pb = parseInt(mainsub[0].Balloon).toString() + ".1";
                                            }
                                        }
                                        let newarr = { ...newItem[qi - 1].newarr, Balloon: pb, ...c }
                                        changedsingle.push({ ...newItem[qi - 1], newarr: newarr, id: qid, DrawLineID: newItem[qi - 1].DrawLineID, Balloon: pb, ...c });

                                        newSubItem.map((e, ei) => {
                                            let sqno = ei + 1;
                                            let b = pb + "." + sqno.toString();
                                            if (parseInt(o.Num_Qty) === 1) {
                                                sqno = ei + 2;
                                                b = parseInt(mainsub[0].Balloon) + "." + sqno.toString();
                                            }

                                            const qid = uuid();
                                            changedsingle.push({ ...e, newarr: { ...e.newarr, Balloon: b }, id: qid, DrawLineID: newItem[qi - 1].DrawLineID, Balloon: b });
                                            return e;
                                        });
                                    }
                                }
                            }
                            if (oldStore[0].Quantity < orgStore[0].Quantity) {
                                 addsubballoons=true
                                // increased
                                if (config.console)
                                    console.log("final pop increased", orgStore[0].Quantity)

                                const o = r.reduce((acc, curr) => {
                                    acc[curr.key] = curr.value;
                                    return acc;
                                }, {});
                                let c = {};
                                c.Minimum = o.Min;
                                c.Maximum = o.Max;
                                c.Nominal = o.Nominal;
                                c.Type = o.Type;
                                c.SubType = o.SubType;
                                c.Unit = o.Unit;
                                c.Spec = this.state.Specification;
                                c.ToleranceType = o.ToleranceType;
                                c.PlusTolerance = o.PlusTolerance;
                                c.MinusTolerance = o.MinusTolerance;
                                c.ModifiedDate = o.Date;
                                c.Quantity = parseInt(o.Num_Qty);

                                for (let qi = 1; qi <= orgStore[0].Quantity; qi++) {
                                    let pb = parseInt(mainsub[0].Balloon).toString() + "." + qi.toString();
                                    const qid = uuid();
                                    let newarr = { ...mainsub[0].newarr, Balloon: pb, ...c };
                                    changedsingle.push({ ...mainsub[0], newarr: newarr, id: qid, DrawLineID: mainsub[0].DrawLineID, Balloon: pb, ...c });
                                    let newSubItem = mainsub[0].subBalloon.filter(a => { return a.isDeleted === false; });
                                    newSubItem.map((e, ei) => {
                                        let sqno = ei + 1;
                                        let b = pb + "." + sqno.toString();
                                        const qid = uuid();
                                        changedsingle.push({ ...e, newarr: { ...e.newarr, Balloon: b }, id: qid, DrawLineID: mainsub[0].DrawLineID, Balloon: b });
                                        return e;
                                    });
                                }


                            }
                            if (oldStore[0].Quantity > orgStore[0].Quantity) {
                                addsubballoons=true;
                                // decreased
                                if (config.console)
                                    console.log("final pop decreased", mainsub[0].Quantity)
                                let decreasedOrg = originalRegions.map((item) => {
                                    if (parseInt(item.Balloon) === parseInt(this.props.selectedBalloon)) {
                                        return item;
                                    }
                                    return false;
                                }).filter(item => item !== false);
                                const o = r.reduce((acc, curr) => {
                                    acc[curr.key] = curr.value;
                                    return acc;
                                }, {});
                                let c = {};
                                c.Minimum = o.Min;
                                c.Maximum = o.Max;
                                c.Nominal = o.Nominal;
                                c.Type = o.Type;
                                c.SubType = o.SubType;
                                c.Unit = o.Unit;
                                c.Spec = this.state.Specification;
                                c.ToleranceType = o.ToleranceType;
                                c.PlusTolerance = o.PlusTolerance;
                                c.MinusTolerance = o.MinusTolerance;
                                c.ModifiedDate = o.Date;
                                c.Quantity = parseInt(o.Num_Qty);


                                for (let qi = 1; qi <= orgStore[0].Quantity; qi++) {
                                    let pb = parseInt(mainsub[0].Balloon).toString() + "." + qi.toString();
                                    const qid = uuid();
                                    let newItem = decreasedOrg.filter((item) => { return item.Balloon === pb })
                                    if (newItem.length > 0) {
                                        let newSubItem = newItem[0].subBalloon.filter(a => { return a.isDeleted === false; });
                                        if (parseInt(o.Num_Qty) > 1) {
                                            pb = parseInt(mainsub[0].Balloon).toString() + "." + qi.toString();
                                        } else {
                                            pb = parseInt(mainsub[0].Balloon).toString();
                                            if (newSubItem.length > 0) {
                                                pb = parseInt(mainsub[0].Balloon).toString() + ".1";
                                            }
                                        }
                                        let newarr = { ...newItem[0].newarr, Balloon: pb, ...c }
                                        changedsingle.push({ ...newItem[0], newarr: newarr, id: qid, DrawLineID: newItem[0].DrawLineID, Balloon: pb, ...c });

                                        newSubItem.map((e, ei) => {
                                            let sqno = ei + 1;
                                            let b = pb + "." + sqno.toString();
                                            if (parseInt(o.Num_Qty) === 1) {
                                                sqno = ei + 2;
                                                b = parseInt(mainsub[0].Balloon) + "." + sqno.toString();
                                            }

                                            const qid = uuid();
                                            changedsingle.push({ ...e, newarr: { ...e.newarr, Balloon: b }, id: qid, DrawLineID: newItem[0].DrawLineID, Balloon: b });
                                            return e;
                                        });
                                    }
                                }
                            }
                            // newStore = deletedOrg.slice();
                            // newStore.splice(fromIndex, 0, ...changedsingle);
                              console.log("addsubballoons",addsubballoons);
                            
                             if(addsubballoons){
                                newStore = deletedOrg.slice();
                                newStore.splice(fromIndex, 0, ...changedsingle);
                            }
                            let selectedRowIndex = newStore[fromIndex].Balloon
                            useStore.setState({ selectedRowIndex: selectedRowIndex.toString() });

                            if (config.console)
                                console.log("final pop save", newStore, changedsingle)
                            //return false;
                            /*
                            found = false;
                            let update_org = originalRegions.map((item) => {
                                //console.log("main balloon update item", parseInt(item.Balloon), parseInt(this.props.selectedBalloon) , this.state)
                                if (parseInt(item.Balloon) === parseInt(this.props.selectedBalloon) && !found) {
                                    found = true;
                                    //console.log("main balloon update item", item, this.state)
                                    item.Minimum = minimum;
                                    item.Maximum = maximum;
                                    item.Nominal = nominal;
                                    item.Type = mainType;
                                    item.SubType = subType;
                                    item.Unit = unit;
                                    item.Spec = this.state.Specification;
                                    item.ToleranceType = toleranceType;
                                    item.PlusTolerance = plusTolerance;
                                    item.MinusTolerance = minusTolerance;
                                    item.ModifiedDate = datetime;
                                    item.Quantity = parseInt(quantity);
                                    item.isballooned = true;
                                    if (item.hasOwnProperty("newarr")) {
                                        item.newarr.Minimum = minimum;
                                        item.newarr.Maximum = maximum;
                                        item.newarr.Nominal = nominal;
                                        item.newarr.Type = mainType;
                                        item.newarr.SubType = subType;
                                        item.newarr.Unit = unit;
                                        item.newarr.Spec = this.state.Specification;
                                        item.newarr.ToleranceType = toleranceType;
                                        item.newarr.PlusTolerance = plusTolerance;
                                        item.newarr.MinusTolerance = minusTolerance;
                                        item.newarr.ModifiedDate = datetime;
                                        item.newarr.Quantity = parseInt(quantity);
                                    }
                                    return item;
                                }
                                return item;
                            })

                            let deletableOrg = update_org.map((item) => {
                                if (parseInt(item.Balloon) === parseInt(this.props.selectedBalloon)) {
                                    return item;
                                }
                                return false;
                            }).filter(item => item !== false);
                            let deletedOrg = update_org.map((item) => {
                                if (parseInt(item.Balloon) !== parseInt(this.props.selectedBalloon)) {
                                    return item;
                                }
                                return false;
                            }).filter(item => item !== false);
                            let changedsingle = [];
                            let cloneFirst = deletableOrg[0];
                            //if (config.console)
                            console.log(cloneFirst)
                            const id = uuid();
                            if (cloneFirst.Quantity === 1 && cloneFirst.subBalloon.length === 0) {
                                let pb = parseInt(cloneFirst.Balloon).toString();
                                let newarr = { ...cloneFirst.newarr, Balloon: pb };
                                changedsingle.push({ ...cloneFirst, newarr: newarr, id: id, DrawLineID: 0, Balloon: pb });
                            }
                            if (cloneFirst.Quantity === 1 && cloneFirst.subBalloon.length > 0) {
                                let pb = parseInt(cloneFirst.Balloon).toString() + ".1";
                                let newarr = { ...cloneFirst.newarr, Balloon: pb };
                                changedsingle.push({ ...cloneFirst, newarr: newarr, id: id, DrawLineID: 0, Balloon: pb });

                                cloneFirst.subBalloon.filter((x) => x.isDeleted === false).map(function (e, ei) {
                                    let sno = ei + 2;
                                    const sid = uuid();
                                    let b = parseInt(cloneFirst.Balloon).toString() + "." + sno.toString();
                                    let newarr = { ...e.newarr, Balloon: b };
                                    changedsingle.push({ ...e, newarr: newarr, id: sid, DrawLineID: 0, Balloon: b });
                                    return e;
                                })
                            }
                            if (cloneFirst.Quantity > 1 && cloneFirst.subBalloon.length === 0) {
                                for (let qi = 1; qi <= cloneFirst.Quantity; qi++) {
                                    const qid = uuid();
                                    let pb = parseInt(cloneFirst.Balloon).toString() + "." + qi.toString();
                                    let newarr = { ...cloneFirst.newarr, Balloon: pb };
                                    changedsingle.push({ ...cloneFirst, newarr: newarr, id: qid, DrawLineID: 0, Balloon: pb });

                                }
                            }
                            if (cloneFirst.Quantity > 1 && cloneFirst.subBalloon.length > 0) {
                                console.log("main balloon update multi")
                                for (let qi = 1; qi <= cloneFirst.Quantity; qi++) {
                                    const qid = uuid();
                                    let pb = parseInt(cloneFirst.Balloon).toString() + "." + qi.toString();

                                    let newarr = { ...cloneFirst.newarr, Balloon: pb };
                                    changedsingle.push({ ...cloneFirst, newarr: newarr, id: qid, DrawLineID: 0, Balloon: pb });

                                    cloneFirst.subBalloon.filter((x) => x.isDeleted === false).map(function (e, ei) {
                                        let sqno = ei + 1;
                                        const sqid = uuid();
                                        let b = parseInt(cloneFirst.Balloon).toString() + "." + qi.toString() + "." + sqno.toString();
                                        let newarr = { ...e.newarr, Balloon: b };
                                        changedsingle.push({ ...e, newarr: newarr, id: sqid, DrawLineID: 0, Balloon: b });
                                        return e;
                                    })

                                }
                            }
                            let overData = Object.values(originalRegions)[0];
                            if (parseInt(this.props.selectedBalloon) - 1 > 0) {
                                let overTemp = originalRegions.filter((item) => { return parseInt(item.Balloon) === parseInt(this.props.selectedBalloon) - 1; });
                                overData = Object.values(overTemp)[overTemp.length - 1];

                            }
                            let newStore = deletedOrg.slice();
                            let fromIndex = deletedOrg.indexOf(overData);
                            newStore.splice(fromIndex + 1, 0, ...changedsingle);
                            newStore = newStore.map((item, i) => {
                                item.DrawLineID = i + 1;
                                return item;
                            });
                            */
                            updatedsingle = newStore;

                        }

                        //if (config.console)
                            console.log("final pop save", updatedsingle)

                        //return false;
                        useStore.setState({
                            originalRegions: updatedsingle,
                            draft: updatedsingle,
                            savedDetails: ((updatedsingle.length > 0) ? true : false),
                            selectedRowIndex: this.props.selectedBalloon,
                            drawingRegions: [],
                            balloonRegions: []
                        });
                        const newstate = useStore.getState();
                        if (newstate.savedDetails) {
                            let originalRegions = newstate.originalRegions;
                            let newrect = newBalloonPosition(originalRegions, newstate);
                            useStore.setState({
                                drawingRegions: newrect,
                                balloonRegions: newrect,
                            });
                        }
                        this.setState({ isHoveringDel: false, isHoveringSave: false });
                        this.onHidePopup();
                        useStore.setState({ isLoading: false });
                    }, (e) => {
                        console.log("Error", e);
                        useStore.setState({ isLoading: false });
                         this.onHidePopup();
                    }).catch(e => {
                        console.log("catch", e);
                        useStore.setState({ isLoading: false });
                         this.onHidePopup();
                    })

                , 500);

        }
    };

    deleteBalloon = (e) => {
        e.preventDefault();
        let deleteItem = (this.props.selectedBalloon).replaceAll(".", "-");

        Swal.fire({
            title: `Are you want to delete Balloon (${deleteItem})?`,
            showCancelButton: true,
            confirmButtonText: 'Yes',
            allowOutsideClick: false,
            allowEscapeKey: false
        }).then((result) => {
            /* Read more about isConfirmed */
            if (result.isConfirmed) {
                useStore.setState({ isLoading: true, loadingText: "Delete a Balloon... Please Wait..." })
                const state = useStore.getState();
                let deleteItem1 = (this.props.selectedBalloon);
                setTimeout(() => {

                    const { ItemView, drawingDetails, originalRegions } = useStore.getState();
                    let pageNo = 0;

                    if (drawingDetails.length > 0 && ItemView != null) {
                        pageNo = Object.values(drawingDetails)[parseInt(ItemView)].currentPage;
                    }

                    let PageData = originalRegions.map((item) => {
                        if (parseInt(item.Page_No) === parseInt(pageNo) && (item.Balloon) !== (deleteItem1)) {
                            return item;
                        }
                        return false;
                    }).filter(item => item !== false);


                    state.originalRegions.map((item) => {
                        if (parseInt(item.Balloon) !== parseInt(deleteItem1)) {
                            return item;
                        }
                        return false;
                    }).filter(item => item !== false);

                    //const resetOverData = JSON.parse(JSON.stringify(state.originalRegions));
                    const resetOverData = [...state.originalRegions];

                    let resetOverSingle = resetOverData.reduce((res, item) => {
                        if (!res[parseInt(item.Balloon)]) {
                            res[parseInt(item.Balloon)] = item;
                        }
                        return res;
                    }, []);


                    let unique = Object.values(resetOverSingle);
                    if (config.console)
                        console.log(unique)
                    let removable = resetOverData.map((item) => {
                        if (parseInt(item.Balloon) === parseInt(deleteItem1)) {
                            return item;
                        }
                        return false;
                    }).filter(item => item !== false);

                    let deletableItem = resetOverData.map((item) => {
                        if ((item.Balloon) === (deleteItem1)) {
                            return item;
                        }
                        return false;
                    }).filter(item => item !== false);

                    let qty = parseInt(removable[0].Quantity);

                    let qtyi = 0;
                    // get all quantity parent
                    let Qtyparent = resetOverData.reduce((res, item) => {
                        if (item.hasOwnProperty("subBalloon") && item.subBalloon.length >= 0 && item.Quantity > 1) {
                            res[qtyi] = item;
                            qtyi++;
                        }
                        return res;
                    }, []);
                    if (config.console)
                        console.log(resetOverData, Qtyparent, qty)
                    // return false;
                    let newitems = [];
                    // if (qty > 1) {
                    if (!deletableItem[0].hasOwnProperty("subBalloon")) {

                        unique.reduce((prev, curr, index) => {
                            const id = uuid();
                            let newarr = [];
                            let Balloon = index + 1;
                            Balloon = Balloon.toString();

                            if (curr.Quantity === 1 && curr.subBalloon.length === 0) {
                                prev.push({ b: (Balloon), c: prev.length + 1 })
                                let i = prev.length;
                                newarr.push({ ...curr, x: curr.newarr.Crop_X_Axis, y: curr.newarr.Crop_Y_Axis, newarr: { ...curr.newarr, Balloon: Balloon }, id: id, DrawLineID: i, Balloon: Balloon });
                            }

                            if (curr.Quantity === 1 && curr.subBalloon.length > 0) {

                                let suffix = deletableItem[0].Balloon.match(/\d+(?=\D*$)/)[0];
                                let regex = /^(\d*\.?\d*)/;
                                let prefix = deletableItem[0].Balloon.match(regex);
                                if (config.console)
                                    console.log(suffix, prefix[0].toString(), deletableItem[0])

                                let newSubItem = curr.subBalloon;

                                if (curr.Balloon === parseInt(deletableItem[0].intBalloon).toString() + ".1") {
                                    newSubItem = curr.subBalloon.filter(x => x.isDeleted === false).map(function (e, ei) {
                                        let sqno = ei + 2;
                                        if (suffix.toString() === sqno.toString()) {
                                            if (config.console)
                                                console.log("new", suffix, sqno)
                                            e.isDeleted = true;
                                            e.newarr.isDeleted = true;
                                        }
                                        return e;
                                    })
                                }

                                newSubItem = newSubItem.filter(a => {
                                    return a.isDeleted === false;
                                });
                                let pb = parseInt(Balloon).toString()
                                if (newSubItem.length > 0) {
                                    pb = parseInt(Balloon).toString() + ".1";
                                }

                                prev.push({ b: pb, c: prev.length + 1 })
                                let i = prev.length;
                                newarr.push({ ...curr, newarr: { ...curr.newarr, Balloon: pb }, id: id, DrawLineID: i, Balloon: pb });


                                newSubItem.map(function (e, ei) {
                                    let sno = ei + 2;
                                    const sid = uuid();
                                    let b = parseInt(Balloon).toString() + "." + sno.toString();
                                    prev.push({ b: b, c: prev.length + 1 })
                                    let i = prev.length;
                                    if (e.hasOwnProperty("Isballooned"))
                                        delete e.Isballooned;
                                    if (e.hasOwnProperty("Id"))
                                        delete e.Id;

                                    let setter = { ...e, newarr: { ...e.newarr, Balloon: b }, id: sid, DrawLineID: i, Balloon: b };
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
                                    newarr.push({ ...curr, newarr: { ...curr.newarr, Balloon: b }, id: qid, DrawLineID: i, Balloon: b });
                                }
                            }

                            if (curr.Quantity > 1 && curr.subBalloon.length > 0) {
                                for (let qi = 1; qi <= curr.Quantity; qi++) {
                                    let newMainItem = [];
                                    let oldMainItem = [];
                                    let pb = parseInt(curr.Balloon).toString() + "." + qi.toString();
                                    prev.push({ b: pb, c: prev.length + 1 })
                                    let i = prev.length;


                                    newMainItem = Qtyparent.map(item => {
                                        let suffix = deletableItem[0].Balloon.match(/\d+(?=\D*$)/)[0];
                                        let regex = /^(\d*\.?\d*)/;
                                        let prefix = deletableItem[0].Balloon.match(regex);
                                        const id = uuid();
                                        if (config.console)
                                            console.log("new", suffix, pb, item.Balloon, prefix[0])
                                        if (pb === item.Balloon && item.Balloon === prefix[0].toString()) {
                                            let subBalloon = JSON.parse(JSON.stringify(item.subBalloon));
                                            // let subBalloon = [...item.subBalloon];

                                            subBalloon.map(function (e, ei) {
                                                let sqno = ei + 1;
                                                if (suffix.toString() === sqno.toString()) {
                                                    if (config.console)
                                                        console.log("new", suffix, sqno, e.Spec)
                                                    e.isDeleted = true;
                                                    e.newarr.isDeleted = true;
                                                }
                                                return e;
                                            })
                                            item.subBalloon = subBalloon;
                                            item.id = id;
                                            item.DrawLineID = i;
                                            return item;
                                        }
                                        return false;
                                    }).filter(x => x !== false);

                                    oldMainItem = Qtyparent.map(item => {
                                        let regex = /^(\d*\.?\d*)/;
                                        let prefix = deletableItem[0].Balloon.match(regex);
                                        if (pb === item.Balloon && item.Balloon !== prefix[0].toString()) {
                                            // const subBalloon = JSON.parse(JSON.stringify(item.subBalloon));
                                            const subBalloon = [...item.subBalloon];
                                            item.subBalloon = subBalloon;
                                            item.id = id;
                                            item.DrawLineID = i;
                                            return item;
                                        }
                                        return false;
                                    }).filter(x => x !== false);
                                    console.log("main items", newMainItem, oldMainItem);

                                    if (newMainItem.length > 0) {

                                        newMainItem.map((nmi) => {
                                            newarr.push(nmi);
                                            let newSubItem = nmi.subBalloon.filter(a => {
                                                return a.isDeleted === false;
                                            });

                                            newSubItem.map(function (e, ei) {
                                                let sqno = ei + 1;
                                                const sqid = uuid();
                                                let b = nmi.Balloon + "." + sqno.toString();
                                                prev.push({ b: b, c: prev.length + 1 })
                                                let i = prev.length;
                                                if (e.hasOwnProperty("Isballooned"))
                                                    delete e.Isballooned;
                                                if (e.hasOwnProperty("Id"))
                                                    delete e.Id;
                                                let setter = { ...e, newarr: { ...e.newarr, Balloon: b }, id: sqid, DrawLineID: i, Balloon: b };
                                                newarr.push(setter);
                                                return e;
                                            });
                                            return nmi;
                                        });

                                    } else {

                                        oldMainItem.map((omi) => {
                                            newarr.push(omi);
                                            let OldSubItem = omi.subBalloon.filter(a => {
                                                return a.isDeleted === false;
                                            });

                                            OldSubItem.map(function (e, ei) {
                                                let sqno = ei + 1;
                                                const sqid = uuid();
                                                let b = omi.Balloon + "." + sqno.toString();
                                                prev.push({ b: b, c: prev.length + 1 })
                                                let i = prev.length;
                                                if (e.hasOwnProperty("Isballooned"))
                                                    delete e.Isballooned;
                                                if (e.hasOwnProperty("Id"))
                                                    delete e.Id;
                                                let setter = { ...e, newarr: { ...e.newarr, Balloon: b }, id: sqid, DrawLineID: i, Balloon: b };
                                                newarr.push(setter);
                                                return e;
                                            });
                                            return omi;
                                        });

                                    }
                                    //console.log("main items",  newMainItem, oldMainItem);
                                    // newarr.push({ ...curr, newarr: { ...curr.newarr, Balloon: pb }, id: qid, DrawLineID: i, Balloon: pb });
                                }

                            }

                            newitems = newitems.slice();
                            newitems.splice(newitems.length, 0, ...newarr);

                            return prev;
                        }, []);

                    }
                    // }
                    let selectedRowIndex = "";
                    PageData = newitems.map((item) => {
                        if (parseInt(item.Page_No) === parseInt(pageNo)) {
                            return item;
                        }
                        return false;
                    }).filter(item => item !== false);

                    if (PageData.length > 0) {
                        var overData = [];
                        let key = deleteItem1.toString().split('.')
                        if (key.length > 2) {
                            key.pop();
                            let se = key.join('.');
                            let overTemp = PageData.filter((item) => { return (item.Balloon) === (se); });
                            overData = Object.values(overTemp)[0];
                        } else {
                            let se = key[0] + ".1";
                            let se1 = key[0];
                            let overTemp = PageData.filter((item) => { return ((item.Balloon) === (se) || (item.Balloon) === (se1)); });
                            overData = Object.values(overTemp)[0];
                        }
                        var prenxtData = PageData.indexOf(overData);
                        selectedRowIndex = PageData[prenxtData].Balloon
                        //console.log(selectedRowIndex)
                        useStore.setState({ selectedRowIndex: selectedRowIndex.toString() });
                    } else {
                        useStore.setState({ selectedRowIndex: null });
                    }



                    const newstate = useStore.getState();
                    let newrect = newBalloonPosition(newitems, newstate);
                    useStore.setState({
                        originalRegions: newitems,
                        draft: newitems,
                        drawingRegions: newrect,
                        balloonRegions: newrect
                    });
                    if (config.console)
                        console.log("pop up delete", newitems)
                    // useStore.setState({ isLoading: false });
                    //console.log(removable, deletableItem, Qtyparent, newitems)
                    //  return false;
                    useStore.setState({ isLoading: false });
                    this.setState({ isHoveringDel: false });
                    this.onHidePopup();
                }, 300);

                const dstate = useStore.getState();
                setTimeout(function () {
                    let scrollElement = document.querySelector('#konvaMain');
                    if (scrollElement !== null) {
                        scrollElement.scrollLeft = dstate.scrollPosition;
                    }

                }, 100);
            }
        });
        return false;

    }

    toggleNested = (e) => {
        e.preventDefault();
 
        this.setState({ popSpecification: this.state.Specification });
        this.setState({ nestedModal: !this.state.nestedModal });
        this.setState({ closeAll: false });
        window.setTimeout(() => {
            const element = document.getElementById("gdt_input");
            const end = element.value.length;
            element.setSelectionRange(end, end);
            element.focus();
        }, 200);
       // window.setTimeout(() => element.focus(), 0);
    };
    toggleAll = () => {
 
        this.setState({ nestedModal: !this.state.nestedModal });
        this.setState({ closeAll: true });
    };
    addSpecification = (e) => {
        e.preventDefault();
        let value = "";
        if (this.state.tolerance_symbol !== "") { value = value + this.state.tolerance_symbol; }
        if (this.state.tolerance_check) { value = value + "ëàí"; }
        if (this.state.cb_tolerance_1 !== "") { value = value + this.state.cb_tolerance_1; }
        if (this.state.cb_tolerance_2 !== "") { value = value + this.state.cb_tolerance_2; }
        if (this.state.cb_datum_a !== "") { value = value + this.state.cb_datum_a; }
        if (this.state.cb_datum_1 !== "") { value = value + this.state.cb_datum_1; }
        if (this.state.cb_datum_b !== "") { value = value + this.state.cb_datum_b; }
        if (this.state.cb_datum_2 !== "") { value = value + this.state.cb_datum_2; }
        if (this.state.cb_datum_c !== "") { value = value + this.state.cb_datum_c; }
        if (this.state.cb_datum_3 !== "") { value = value + this.state.cb_datum_3; }
        let ovalue = this.state.popSpecification;
        let newValue = ovalue.substring(0, this.state.start) + value + ovalue.substring(this.state.end, ovalue.length);
        this.setState({ start: this.state.start + value.length, end: this.state.end + value.length });
        this.setState({
            popSpecification: newValue,
            tolerance_symbol: "",
            tolerance_check: false,
            cb_tolerance_1: "",
            cb_tolerance_2: "",
            cb_datum_a: "",
            cb_datum_1: "",
            cb_datum_b: "",
            cb_datum_2: "",
            cb_datum_c: "",
            cb_datum_3: ""
        });
      //  console.log("addSpecification")
    }
    onButtonClickHandler = (e) => {
        e.preventDefault();
        let text = e.target.dataset.value;
        let value = this.state.popSpecification;
        let newValue = value.substring(0, this.state.start) + text + value.substring(this.state.end, value.length);
        this.setState({ start: this.state.start + text.length , end: this.state.end + text.length  });
        this.setState({ popSpecification: newValue });
        //console.log("onButtonClickHandler")
    }
    handleSelect = (e) => {
        e.preventDefault();
       // console.log("handleSelect")
        this.setState({ start: e.target.selectionStart, end: e.target.selectionEnd });
    }
    saveSpecification = (e) => {
        e.preventDefault();
        this.setState({ Specification: this.state.popSpecification });
        //console.log("popSpecification")
        this.toggleAll();
    }
    
     
    render() {

        //console.log(this.state)


        let tol_symbol = ["", "ëûí", "ëüí", "ëáí", "ëâí", "ëãí", "ëäí", "ëåí", "ëæí", "ëçí", "ëèí", "ë²í", "ë³í", "ëÿí", "ëºí" ];
        let cb_tolerance_1 = [ "", "ëùîîïí", "ëùîîðí", "ëùîîòí", "ëùîîóí", "ëùîïîí", "ëùîïóí", "ëùîðîí"];
        let cb_tolerance_2 = ["", "ëÝí", "ëÞí", "ëßí", "ë…í", "ëŸí", "ëží", "ëχí"];
        let cb_datum_a = ["", "ëÀí", "ëÁí", "ëÂí", "ëÃí", "ëÄí",  "ëÆí", "ëÇí", "ëÈí", "ëÉí", "ëÊí", "ëËí", "ëÌí", "ëÍí", "ëÎí", "ëÏí", "ëÐí", "ëÑí", "ëÒí", "ëÓí", "ëÔí", "ëÕí", "ëÖí", "ë×í", "ëØí", "ëÙí"];
        let cb_datum_1 = ["", "ëÝí", "ëÞí", "ëßí", "ë…í", "ëŸí", "ëží", "ëχí"];


        const state = useStore.getState();
        let originalRegions = state.originalRegions;
        let newrects = originalRegions.map((item) => {
            //console.log(item.Balloon, this.props.selectedBalloon)
            if (this.props.selectedBalloon !== null && parseInt(item.Balloon) === parseInt(this.props.selectedBalloon)) {
                return item;
            }
            return false;
        }).filter(item => item !== false);
        if (newrects.length > 1) {
              newrects = originalRegions.map((item) => {
                  // console.log(item.Balloon, this.props.selectedBalloon)
                  if (this.props.selectedBalloon !== null && item.Balloon.toString() === this.props.selectedBalloon.toString()) {
                    return item;
                }
                return false;
            }).filter(item => item !== false);
        }
        if (config.console)
        console.log(originalRegions,newrects, this.props.selectedBalloon)
        let lmtype = state.lmtype;
        let lmsubtype = state.lmsubtype;
        
        let units = state.units;
        let cmbTolerance = state.cmbTolerance;

        let type1 = this.state.selectedType;
        let type2 = this.state.selectedSubType;
        let type_unit = this.state.selectedUnit;
        let typeTolerance = this.state.selectedTolerance;
        let quantity = parseInt(this.state.selectedQuantity);
       // let Spec = this.state.Specification
        let plusTolerance = this.state.pTolerance;
        let minusTolerance = this.state.mTolerance;
        let maxValue = (this.state.maxValue);
        let minValue = (this.state.minValue);
        const decimalPlaces = (minValue.toString().split('.')[1] || []).length;
        let dynamicStepValue = [];
        dynamicStepValue.push(".")
        for (let i = 0; i < decimalPlaces; i++) {
            dynamicStepValue.push("0")
        }
        dynamicStepValue.push("1")
       
        //console.log(minValue, dynamicStepValue.join(''))
        
        if (this.state.popupShown === true) {
           // console.log(this.state)
            let nspec = this.state.popSpecification;
            let spec = this.state.Specification;
            let tolerance_symbol = this.state.tolerance_symbol;
            let tolerance_check = this.state.tolerance_check;
            let tolerance_1 = this.state.cb_tolerance_1;
            let tolerance_2 = this.state.cb_tolerance_2
            let datum_a = this.state.cb_datum_a;
            let datum_1 = this.state.cb_datum_1;
            let datum_b = this.state.cb_datum_b;
            let datum_2 = this.state.cb_datum_2;
            let datum_c = this.state.cb_datum_c;
            let datum_3 = this.state.cb_datum_3;
           // console.log(Spec, nspec,spec)
            let hypenBalloon;
            let h = this.props.selectedBalloon;
            const isInteger = h % 1 === 0;
            if (isInteger) {
                hypenBalloon = h;
            } else {
                hypenBalloon = h.replaceAll(".", "-");
            }
            return (
                <Draggable
                    cancel=".fieldmove"
                    handle=".handlemain"
                    defaultPosition = {{ x: 0, y: 0 }}
                    scale = {1}
                    bounds={{ left: - window.innerWidth / 2, top: - window.innerHeight/2, right: window.innerWidth/2, bottom: window.innerHeight/2 }}
                >
                <Modal isOpen={this.state.modal} toggle={this.onHidePopup} backdrop={this.state.backdrop} >
                        <div className="modal-header handlemain"   >
                        <h2>
                            Balloon No: {hypenBalloon}
                        </h2>
                        <div>
                        <Button color="light" className="light-btn Savebtn buttons primary"
                            onClick={this.saveBalloon}
                            onMouseOver={this.handleMouseOverSave}
                            onMouseOut={this.handleMouseOutSave}
                        >
                            <div style={{ position: "relative" }}>
                                <span className="PySCBInfo EI48Lc"  >
                                    Save
                                </span>
                                </div>
                                <div className="save" style={{ position: "relative" }}>
                                    <Image name='save.svg' className="icon" alt="Save" />
                                </div>
                                <div className="save-white" style={{ position: "relative" }}>
                                    <Image name='save-white.svg' className="icon" alt="Save" />
                                </div>

                                </Button>
                                {this.state.isballooned && (
                                    <Button color="light" className="light-btn Deletebtn buttons primary pr-5"
                                        onClick={(e) => {
                                            if (this.state.issubBalloon) {
                                                 this.deleteBalloon(e)
                                            }
                                        }}
                                        onMouseOver={(e) => {
                                            if (this.state.issubBalloon) {
                                                this.handleMouseOverDel()
                                            }
                                        }}
                                        onMouseOut={(e) => {
                                                if (this.state.issubBalloon) {
                                                    this.handleMouseOutDel()
                                                }
                                            }}
                                         
                        >
                            <div style={{ position: "relative" }}>
                                <span className="PySCBInfo EI48Lc" aria-hidden={this.state.isHoveringDel} style={{ display: this.state.isHoveringDel ? "block" : "none" }} >
                                    {this.state.isHoveringDel && (
                                        "Delete"
                                    )}
                                </span>
                                </div>
                                <div className="delete" style={{ position: "relative" }}>
                                   <Image name='delete.svg' className="icon" alt="Delete" />
                                </div>
                                <div className="delete-white" style={{ position: "relative" }}>
                                    <Image name='delete-white.svg' className="icon" alt="Delete" />
                                </div>
                            </Button>
                                )}
                            <Button color="light"
                                onClick={this.onHidePopup}

                                className="light-btn p-0"
                            >
                                <Image name='close-fill.svg' className="icon" alt="close" />
                            </Button>

                        </div>

                        </div>
                    <ModalBody>
                            <Draggable
                                cancel=".fieldmove"
                                handle=".handlesub"
                                defaultPosition={{ x: 0, y: 0 }}
                                scale={1}
                                bounds={{ left: - window.innerWidth / 2, top: - window.innerHeight / 2, right: window.innerWidth / 2, bottom: window.innerHeight / 2 }}
                            >
                    <Modal
                            isOpen={this.state.nestedModal}
                            toggle={this.toggleNested}
                            onClosed={this.state.closeAll ? this.toggle : undefined}
                            backdrop={ false} // {this.state.backdrop}
                            style={{maxWidth:"600px"} }
                        >
                                    <ModalHeader className="p-1 justify-content-center handlesub">GDT Data Input</ModalHeader>
                        <ModalBody>

                            <Tabs>
                                <TabList>
                                    <Tab>Numeric</Tab>
                                    <Tab>Symbols</Tab>
                                    <Tab>Symbols 2</Tab>
                                    <Tab>Tolerance</Tab>
                                </TabList>
                                   
                                    <TabPanel>   
                                    <Table bordered size="sm" className="ag-cell--math tablec">
                                        <thead></thead>
                                        <tbody>
                                                <tr>
                                                    <td data-value={"0"} onClick={this.onButtonClickHandler}>{"0"}</td>
                                                    <td data-value={"1"} onClick={this.onButtonClickHandler}>{"1"}</td>
                                                    <td data-value={"2"} onClick={this.onButtonClickHandler}>{"2"}</td>
                                                    <td data-value={"3"} onClick={this.onButtonClickHandler}>{"3"}</td>
                                                    <td data-value={"4"} onClick={this.onButtonClickHandler}>{"4"}</td>
                                                    <td data-value={"5"} onClick={this.onButtonClickHandler}>{"5"}</td>
                                                    <td data-value={"6"} onClick={this.onButtonClickHandler}>{"6"}</td>
                                                    <td data-value={"7"} onClick={this.onButtonClickHandler}>{"7"}</td>
                                                    <td data-value={"8"} onClick={this.onButtonClickHandler}>{"8"}</td>
                                                    <td data-value={"9"} onClick={this.onButtonClickHandler}>{"9"}</td>
                                                    <td data-value={"+"} onClick={this.onButtonClickHandler}>{"+"}</td>
                                                    <td data-value={"-"} onClick={this.onButtonClickHandler}>{"-"}</td>
                                            </tr>
                                            <tr>
                                                    <td data-value={"."} onClick={this.onButtonClickHandler}>{"."}</td>
                                                    <td data-value={"%"} onClick={this.onButtonClickHandler}>{"%"}</td>
                                                    <td data-value={"$"} onClick={this.onButtonClickHandler}>{"$"}</td>
                                                    <td data-value={"#"} onClick={this.onButtonClickHandler}>{"#"}</td>
                                                    <td data-value={"*"} onClick={this.onButtonClickHandler}>{"*"}</td>
                                                    <td data-value={"("} onClick={this.onButtonClickHandler}>{"("}</td>
                                                    <td data-value={")"} onClick={this.onButtonClickHandler}>{")"}</td>
                                                    <td data-value={"!"} onClick={this.onButtonClickHandler}>{"!"}</td>
                                                    <td data-value={"^"} onClick={this.onButtonClickHandler}>{"^"}</td>
                                                    <td data-value={"["} onClick={this.onButtonClickHandler}>{"["}</td>
                                                    <td data-value={"]"} onClick={this.onButtonClickHandler}>{"]"}</td>
                                                    <td data-value={"="} onClick={this.onButtonClickHandler}>{"="}</td>
                                            </tr>
                                            <tr>
                                                    <td data-value={"{"} onClick={this.onButtonClickHandler}>{"{"} </td>
                                                    <td data-value={"}"} onClick={this.onButtonClickHandler}>{"}"}</td>
                                                    <td data-value={":"} onClick={this.onButtonClickHandler}>{":"}</td>
                                                    <td data-value={"?"} onClick={this.onButtonClickHandler}>{"?"}</td>
                                                    <td data-value={"/"} onClick={this.onButtonClickHandler}>{"/"}</td>
                                                    <td data-value={"\\"} onClick={this.onButtonClickHandler}>{"\\"}</td>
                                                    <td data-value={"@"} onClick={this.onButtonClickHandler}>{"@"}</td>
                                                    <td colSpan={5} className="dishover"></td>
                                            </tr>
                                        </tbody>
                                    </Table>
                                </TabPanel>
                                    <TabPanel>

                                        <Table bordered size="sm" className="ag-cell--math tablec" >
                                            <thead></thead>
                                            <tbody>
                                                <tr>
                                                    <td data-value={"À"} onClick={this.onButtonClickHandler}>{"À"}</td>
                                                    <td data-value={"Á"} onClick={this.onButtonClickHandler}>{"Á"}</td>
                                                    <td data-value={"Â"} onClick={this.onButtonClickHandler}>{"Â"}</td>
                                                    <td data-value={"Ã"} onClick={this.onButtonClickHandler}>{"Ã"}</td>
                                                    <td data-value={"Ä"} onClick={this.onButtonClickHandler}>{"Ä"}</td>
                                                    <td data-value={"Å"} onClick={this.onButtonClickHandler}>{"Å"}</td>
                                                    <td data-value={"Æ"} onClick={this.onButtonClickHandler}>{"Æ"}</td>
                                                    <td data-value={"Ç"} onClick={this.onButtonClickHandler}>{"Ç"}</td>
                                                    <td data-value={"È"} onClick={this.onButtonClickHandler}>{"È"}</td>
                                                    <td data-value={"É"} onClick={this.onButtonClickHandler}>{"É"}</td>
                                                    <td data-value={"Ê"} onClick={this.onButtonClickHandler}>{"Ê"}</td>
                                                    <td data-value={"Ë"} onClick={this.onButtonClickHandler}>{"Ë"}</td>
                                                    <td data-value={"Ì"} onClick={this.onButtonClickHandler}>{"Ì"}</td>
                                                    <td data-value={"Í"} onClick={this.onButtonClickHandler}>{"Í"}</td>
                                                    <td data-value={"Î"} onClick={this.onButtonClickHandler}>{"Î"}</td>
                                                    <td colSpan={1} className="dishover"></td>
                                                </tr>
                                                <tr> 
                                                    <td data-value={"Ï"} onClick={this.onButtonClickHandler}>{"Ï"}</td>
                                                    <td data-value={"Ð"} onClick={this.onButtonClickHandler}>{"Ð"}</td>
                                                    <td data-value={"Ñ"} onClick={this.onButtonClickHandler}>{"Ñ"}</td>
                                                    <td data-value={"Ò"} onClick={this.onButtonClickHandler}>{"Ò"}</td>
                                                    <td data-value={"Ó"} onClick={this.onButtonClickHandler}>{"Ó"}</td>
                                                    <td data-value={"Ô"} onClick={this.onButtonClickHandler}>{"Ô"}</td>
                                                    <td data-value={"Õ"} onClick={this.onButtonClickHandler}>{"Õ"}</td>
                                                    <td data-value={"Ö"} onClick={this.onButtonClickHandler}>{"Ö"}</td>
                                                    <td data-value={"×"} onClick={this.onButtonClickHandler}>{"×"}</td>
                                                    <td data-value={"Ø"} onClick={this.onButtonClickHandler}>{"Ø"} </td>
                                                    <td data-value={"Ù"} onClick={this.onButtonClickHandler}>{"Ù"}</td>
                                                    <td className="dishover"></td>
                                                    <td data-value={"¡"} onClick={this.onButtonClickHandler}>{"¡"}</td>
                                                    <td data-value={"±"} onClick={this.onButtonClickHandler}>{"±"}</td>
                                                    <td data-value={"°"} onClick={this.onButtonClickHandler}>{"°"}</td>
                                                    <td colSpan={1} className="dishover"></td>
                                                </tr>
                                                <tr>
                                                    <td data-value={"û"} onClick={this.onButtonClickHandler}>{"û"}</td>
                                                    <td data-value={"è"} onClick={this.onButtonClickHandler}>{"è"}</td>
                                                    <td data-value={"ç"} onClick={this.onButtonClickHandler}>{"ç"}</td>
                                                    <td data-value={"á"} onClick={this.onButtonClickHandler}>{"á"}</td>
                                                    <td data-value={"â"} onClick={this.onButtonClickHandler}>{"â"}</td>
                                                    <td data-value={"ã"} onClick={this.onButtonClickHandler}>{"ã"}</td>
                                                    <td data-value={"å"} onClick={this.onButtonClickHandler}>{"å"}</td>
                                                    <td data-value={"ä"} onClick={this.onButtonClickHandler}>{"ä"}</td>
                                                    <td data-value={"æ"} onClick={this.onButtonClickHandler}>{"æ"}</td>
                                                    <td data-value={"²"} onClick={this.onButtonClickHandler}>{"²"}</td>
                                                    <td data-value={"³"} onClick={this.onButtonClickHandler}>{"³"}</td>
                                                    <td data-value={"ÿ"} onClick={this.onButtonClickHandler}>{"ÿ"}</td>
                                                    <td data-value={"–"} onClick={this.onButtonClickHandler}>{"–"}</td>
                                                    <td data-value={"º"} onClick={this.onButtonClickHandler}>{"º"}</td>
                                                    <td colSpan={2} className="dishover"></td>
                                                </tr>
                                                <tr>
                                                    <td data-value={"ë"} onClick={this.onButtonClickHandler}>{"ë"}</td>
                                                    <td data-value={"ì"} onClick={this.onButtonClickHandler}>{"ì"}</td>
                                                    <td data-value={"í"} onClick={this.onButtonClickHandler}>{"í"}</td>
                                                    <td data-value={"î"} onClick={this.onButtonClickHandler}>{"î"}</td>
                                                    <td data-value={"ï"} onClick={this.onButtonClickHandler}>{"ï"}</td>
                                                    <td data-value={"ð"} onClick={this.onButtonClickHandler}>{"ð"}</td>
                                                    <td data-value={"ñ"} onClick={this.onButtonClickHandler}>{"ñ"}</td>
                                                    <td data-value={"ò"} onClick={this.onButtonClickHandler}>{"ò"}</td>
                                                    <td data-value={"ó"} onClick={this.onButtonClickHandler}>{"ó"}</td>
                                                    <td data-value={"ô"} onClick={this.onButtonClickHandler}>{"ô"}</td>
                                                    <td data-value={"õ"} onClick={this.onButtonClickHandler}>{"õ"}</td>
                                                    <td data-value={"ö"} onClick={this.onButtonClickHandler}>{"ö"}</td>
                                                    <td data-value={"÷"} onClick={this.onButtonClickHandler}>{"÷"}</td>
                                                    <td data-value={"ù"} onClick={this.onButtonClickHandler}>{"ù"}</td>
                                                    <td data-value={"ú"} onClick={this.onButtonClickHandler}>{"ú"}</td>
                                                    <td data-value={"ύ"} onClick={this.onButtonClickHandler}>{"ύ"}</td>
                                                </tr>
                                                <tr>
                                                    <td data-value={"é"} onClick={this.onButtonClickHandler}>{"é"}</td>
                                                    <td data-value={"ê"} onClick={this.onButtonClickHandler}>{"ê"}</td>
                                                    <td data-value={"¹"} onClick={this.onButtonClickHandler}>{"¹"}</td>
                                                    <td data-value={"¶"} onClick={this.onButtonClickHandler}>{"¶"}</td>
                                                    <td data-value={"χ"} onClick={this.onButtonClickHandler}>{"χ"}</td>
                                                    <td className="dishover"></td>
                                                    <td data-value={"à"} onClick={this.onButtonClickHandler}>{"à"}</td>
                                                    <td data-value={"•"} onClick={this.onButtonClickHandler}>{"•"}</td>
                                                    <td className="dishover"></td>
                                                    <td data-value={"ß"} onClick={this.onButtonClickHandler}>{"ß"}</td>
                                                    <td data-value={"Þ"} onClick={this.onButtonClickHandler}>{"Þ"}</td>
                                                    <td data-value={"Ý"} onClick={this.onButtonClickHandler}>{"Ý"}</td>
                                                    <td data-value={"…"} onClick={this.onButtonClickHandler}>{"…"}</td>
                                                    <td data-value={"ž"} onClick={this.onButtonClickHandler}>{"ž"}</td>
                                                    <td data-value={"Ÿ"} onClick={this.onButtonClickHandler}>{"Ÿ"}</td>
                                                    <td colSpan={1} className="dishover"></td>
                                                    </tr>                                               
                                            </tbody>
                                        </Table>
                                    </TabPanel>
                                    <TabPanel>
                                        <Table bordered size="sm" className="ag-cell--math tablec" >
                                            <thead></thead>
                                            <tbody>
                                                <tr>
                                                    <td data-value={"£"} onClick={this.onButtonClickHandler}>{"£"}</td>
                                                    <td data-value={"¿"} onClick={this.onButtonClickHandler}>{"¿"}</td>
                                                    <td data-value={"¬"} onClick={this.onButtonClickHandler}>{"¬"}</td>
                                                    <td data-value={"Û"} onClick={this.onButtonClickHandler}>{"Û"}</td>
                                                    <td data-value={"Ú"} onClick={this.onButtonClickHandler}>{"Ú"}</td>
                                                    <td data-value={"´"} onClick={this.onButtonClickHandler}>{"´"}</td>
                                                    <td data-value={"»"} onClick={this.onButtonClickHandler}>{"»"}</td>
                                                    <td data-value={"«"} onClick={this.onButtonClickHandler}>{"«"}</td>
                                                    <td data-value={"Ё"} onClick={this.onButtonClickHandler}>{"Ё"}</td>
                                                    <td data-value={"Ђ"} onClick={this.onButtonClickHandler}>{"Ђ"}</td>
                                                    <td data-value={"Ѓ"} onClick={this.onButtonClickHandler}>{"Ѓ"}</td>
                                                    <td data-value={"Є"} onClick={this.onButtonClickHandler}>{"Є"}</td>
                                                    <td data-value={"ω"} onClick={this.onButtonClickHandler}>{"ω"}</td>
                                                    <td data-value={"ϊ"} onClick={this.onButtonClickHandler}>{"ϊ"}</td>
                                                </tr>
                                                <tr>
                                                    <td data-value={"‚"} onClick={this.onButtonClickHandler}>{"‚"}</td>
                                                    <td data-value={"†"} onClick={this.onButtonClickHandler}>{"†"}</td>
                                                    <td data-value={"‡"} onClick={this.onButtonClickHandler}>{"‡"}</td>
                                                    <td data-value={"ˆ"} onClick={this.onButtonClickHandler}>{"ˆ"}</td>
                                                    <td data-value={"‰"} onClick={this.onButtonClickHandler}>{"‰"}</td>
                                                    <td data-value={"┴"} onClick={this.onButtonClickHandler}>{"┴"}</td>
                                                    <td data-value={"‹"} onClick={this.onButtonClickHandler}>{"‹"}</td>
                                                    <td data-value={"¥"} onClick={this.onButtonClickHandler}>{"¥"}</td>
                                                    <td data-value={"¢"} onClick={this.onButtonClickHandler}>{"¢"}</td>
                                                    <td data-value={"ý"} onClick={this.onButtonClickHandler}>{"ý"}</td>
                                                    <td data-value={"ϋ"} onClick={this.onButtonClickHandler}>{"ϋ"}</td>
                                                    <td data-value={"ό"} onClick={this.onButtonClickHandler}>{"ό"}</td>
                                                    <td data-value={"Ѕ"} onClick={this.onButtonClickHandler}>{"Ѕ"}</td>
                                                    <td colSpan={1} className="dishover"></td>
                                                </tr>
                                                <tr>
                                                    <td data-value={"‘"} onClick={this.onButtonClickHandler}>{"‘"}</td>
                                                    <td data-value={"’"} onClick={this.onButtonClickHandler}>{"’"}</td>
                                                    <td data-value={"“"} onClick={this.onButtonClickHandler}>{"“"}</td>
                                                    <td data-value={"”"} onClick={this.onButtonClickHandler}>{"”"}</td>
                                                    <td data-value={"„"} onClick={this.onButtonClickHandler}>{"„"}</td>
                                                    <td data-value={"¨"} onClick={this.onButtonClickHandler}>{"¨"}</td>
                                                    <td data-value={"¸"} onClick={this.onButtonClickHandler}>{"¸"}</td>
                                                    <td data-value={"œ"} onClick={this.onButtonClickHandler}>{"œ"}</td>
                                                    <td data-value={"®"} onClick={this.onButtonClickHandler}>{"®"}</td>
                                                    <td data-value={"˜"} onClick={this.onButtonClickHandler}>{"˜"}</td>
                                                    <td data-value={"š"} onClick={this.onButtonClickHandler}>{"š"}</td>
                                                    <td data-value={"ˉ"} onClick={this.onButtonClickHandler}>{"ˉ"}</td>
                                                    <td data-value={"ψ"} onClick={this.onButtonClickHandler}>{"ψ"}</td>
                                                    <td data-value={"ώ"} onClick={this.onButtonClickHandler}>{"ώ"}</td>
                                                    </tr>
                                                <tr>
                                                    <td data-value={"─"} onClick={this.onButtonClickHandler}>{"─"}</td>
                                                    <td data-value={"│"} onClick={this.onButtonClickHandler}>{"│"}</td>
                                                    <td data-value={"┌"} onClick={this.onButtonClickHandler}>{"┌"}</td>
                                                    <td data-value={"┐"} onClick={this.onButtonClickHandler}>{"┐"}</td>
                                                    <td data-value={"└"} onClick={this.onButtonClickHandler}>{"└"}</td>
                                                    <td data-value={"┘"} onClick={this.onButtonClickHandler}>{"┘"}</td>
                                                    <td data-value={"├"} onClick={this.onButtonClickHandler}>{"├"}</td>
                                                    <td data-value={"┤"} onClick={this.onButtonClickHandler}>{"┤"}</td>
                                                    <td data-value={"┬"} onClick={this.onButtonClickHandler}>{"┬"}</td>
                                                    <td data-value={"┴"} onClick={this.onButtonClickHandler}>{"┴"}</td>
                                                    <td data-value={"┼"} onClick={this.onButtonClickHandler}>{"┼"}</td>
                                                    <td data-value={"═"} onClick={this.onButtonClickHandler}>{"═"}</td>
                                                    <td data-value={"║"} onClick={this.onButtonClickHandler}>{"║"}</td>
                                                    <td data-value={"╒"} onClick={this.onButtonClickHandler}>{"╒"}</td>
                
                                                </tr>
                                            </tbody>
                                        </Table>
                                    </TabPanel>
                                    <TabPanel>
                                        <Table   size="sm" className="ag-cell--math tablec tolerance">
                                            <thead></thead>
                                            <tbody>
                                                <tr>
                                                    <td className="dishover">
                                                        <fieldset>
                                                            <legend>Symbol</legend>

                                                            <Input
                                                                onMouseDown={(e) => { e.stopPropagation() }}
                                                                onMouseMove={(e) => { e.stopPropagation() }}
                                                                        onMouseUp={(e) => { e.stopPropagation() }}
                                                                        onfocus={(e) => e.stopPropagation() }
                                                                        className="fieldmove"
                                                                id="tolerance_symbol"
                                                                name="tolerance_symbol"
                                                                type="select"
                                                                value={tolerance_symbol}
                                                                onChange={(e) => {
                                                                    this.setState({ tolerance_symbol: e.target.value })
                                                                }}
                                                            >
                                                                {tol_symbol.map((item, i) => (
                                                                    <option key={i}
                                                                        value={item}

                                                                    >
                                                                        {item}
                                                                    </option>
                                                                ))};
                                                                
                                                            </Input>
                                                        </fieldset>
                                                    </td>
                                                    <td className="dishover">
                                                        <fieldset>
                                                            <legend className="m0">Tolerance</legend>
                                                            <div className="d-flex justify-content-around">
                                                             
                                                                    <FormGroup check>
                                                                    <Input
                                                                        id="tolerance_check"
                                                                        name="tolerance_check"
                                                                        type="checkbox"
                                                                        checked={tolerance_check}
                                                                        onChange={(e) => {
                                                                            this.setState({ tolerance_check: e.target.checked })
                                                                        }}
                                                                    />
                                                                    {' '}
                                                                        <Label for="tolerance_check" check>
                                                                        {"ëàí" }
                                                                    </Label>
                                                                </FormGroup>
                                                                    <Input
                                                                       
                                                                    id="cb_tolerance_1"
                                                                    name="cb_tolerance_1"
                                                                    type="select"
                                                                    value={tolerance_1}
                                                                    onChange={(e) => {
                                                                        this.setState({ cb_tolerance_1: e.target.value })
                                                                    }}
                                                                >
                                                                    {cb_tolerance_1.map((item, i) => (
                                                                        <option key={i}
                                                                            value={item}

                                                                        >
                                                                            {item}
                                                                        </option>
                                                                    ))};

                                                                </Input>
                                                                    <Input
                                                                  
                                                                    id="cb_tolerance_2"
                                                                    name="cb_tolerance_2"
                                                                    type="select"
                                                                    value={tolerance_2}
                                                                    onChange={(e) => {
                                                                        this.setState({ cb_tolerance_2: e.target.value })
                                                                    }}
                                                                >
                                                                    {cb_tolerance_2.map((item, i) => (
                                                                        <option key={i}
                                                                            value={item}

                                                                        >
                                                                            {item}
                                                                        </option>
                                                                    ))};

                                                                    </Input>
                                                              
                                                            </div>
                                                            
                                                        </fieldset>
                                                    </td>
                                                    <td className="dishover">
                                                        <fieldset>
                                                            <legend>Datums</legend>
                                                            <div className="d-flex  mb-2 justify-content-around">
                                                                <Input
                                                                   
                                                                id="tolerance_datumsA"
                                                                name="tolerance_datumsA"
                                                                    type="select"
                                                                    value={datum_a}
                                                                    onChange={(e) => {
                                                                        this.setState({ cb_datum_a: e.target.value })
                                                                    }}
                                                            >
                                                                    {cb_datum_a.map((item, i) => (
                                                                    <option key={i}
                                                                        value={item}

                                                                    >
                                                                        {item}
                                                                    </option>
                                                                ))};

                                                                </Input>
                                                                <Input
                                                                    id="tolerance_datums1"
                                                                    name="tolerance_datums1"
                                                                    type="select"
                                                                    value={datum_1}
                                                                    onChange={(e) => {
                                                                        this.setState({ cb_datum_1: e.target.value })
                                                                    }}
                                                                >
                                                                    {cb_datum_1.map((item, i) => (
                                                                        <option key={i}
                                                                            value={item}

                                                                        >
                                                                            {item}
                                                                        </option>
                                                                    ))};

                                                                </Input>
                                                            </div>
                                                       
                                                            <div className="d-flex  mb-2 justify-content-around">
                                                                <Input
                                                                
                                                                    id="tolerance_datumsB"
                                                                    name="tolerance_datumsB"
                                                                    type="select"
                                                                    value={datum_b}
                                                                    onChange={(e) => {
                                                                        this.setState({ cb_datum_b: e.target.value })
                                                                    }}
                                                                >
                                                                    {cb_datum_a.map((item, i) => (
                                                                        <option key={i}
                                                                            value={item}

                                                                        >
                                                                            {item}
                                                                        </option>
                                                                    ))};

                                                                </Input>
                                                                <Input
                                                                 
                                                                    id="tolerance_datums2"
                                                                    name="tolerance_datums2"
                                                                    type="select"
                                                                    value={datum_2}
                                                                    onChange={(e) => {
                                                                        this.setState({ cb_datum_2: e.target.value })
                                                                    }}
                                                                >
                                                                    {cb_datum_1.map((item, i) => (
                                                                        <option key={i}
                                                                            value={item}

                                                                        >
                                                                            {item}
                                                                        </option>
                                                                    ))};

                                                                </Input>
                                                            </div>
                                                     
                                                            <div className="d-flex mb-2 justify-content-around">
                                                                <Input
                                                                   
                                                                    id="tolerance_datumsC"
                                                                    name="tolerance_datumsC"
                                                                    type="select"
                                                                    value={datum_c}
                                                                    onChange={(e) => {
                                                                        this.setState({ cb_datum_c: e.target.value })
                                                                    }}
                                                                >
                                                                    {cb_datum_a.map((item, i) => (
                                                                        <option key={i}
                                                                            value={item}

                                                                        >
                                                                            {item}
                                                                        </option>
                                                                    ))};

                                                                </Input>
                                                                <Input
                                                                    
                                                                    id="tolerance_datums3"
                                                                    name="tolerance_datums3"
                                                                    type="select"
                                                                    value={datum_3}
                                                                    onChange={(e) => {
                                                                        this.setState({ cb_datum_3: e.target.value })
                                                                    }}
                                                                >
                                                                    {cb_datum_1.map((item, i) => (
                                                                        <option key={i}
                                                                            value={item}

                                                                        >
                                                                            {item}
                                                                        </option>
                                                                    ))};

                                                                </Input>
                                                            </div>
                                                        </fieldset>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </Table>
                                        <div className="d-flex mb-2 justify-content-around">
                                        <Button color="light-btn btn buttons primary primary_hover " onClick={this.addSpecification}>
                                            Insert
                                        </Button>
                                        </div>
                                    </TabPanel>
                                        </Tabs>
                                        <Table striped hover borderless className="popSpecification"  >
                                    <thead></thead>
                                    <tbody><tr><td>
                                        <Input
                                            id="gdt_input"
                                            name="gdt_input"
                                            type="textarea"
                                            rows={5}
                                                    className="ag-cell--math gdt_input"
                                            style={{ fontSize: "14px", backgroundColor: "#fff" }}
                                            onSelect={this.handleSelect}
                                            onChange={(e) => {  
                                                this.setState({ popSpecification: e.target.value });
                                                
                                                }
                                            }
                                            value={nspec}
                                        ></Input>
                                    </td></tr>
                                    </tbody>
                                </Table>
                                    </ModalBody>
                                    <ModalFooter className="handlesub">
                                <Button color="light-btn btn buttons secondary" onClick={this.toggleAll}>
                                    Cancel
                                </Button>{"   "}
                                <Button color="light-btn btn buttons primary primary_hover " onClick={this.saveSpecification}>
                                Update
                            </Button>
                              
                        </ModalFooter>
                                </Modal>
                           </Draggable>
                        {newrects.length > 0 && (
                            <>
                                <Form>
                                    <Table   striped hover borderless   >
                                        <thead></thead>
                                        <tbody>
                                            <tr><td>
                                                <Label for="type" className="mt-2">Type</Label>
                                                <Input
                                                    id="type"
                                                    name="type"
                                                    type="select"
                                                        value={type1}
                                                    onChange={(e) => this.setState({ selectedType: e.target.value })}
                                                >
                                                    {lmtype.map((item, i) => (
                                                        <option key={i}
                                                        value={item.type_ID}
                                                             
                                                        >
                                                            {item.type_Name}
                                                        </option>
                                                    ))};
                                                </Input>
                                            </td><td>
                                                <Label for="sub_type" className="mt-2">Sub-Type</Label>
                                                <Input
                                                    id="sub_type"
                                                    name="sub_type"
                                                    type="select"
                                                            value={type2}
                                                    onChange={(e) => this.setState({ selectedSubType: e.target.value })}
                                                >
                                                <option key="" value="">--Select--</option>
                                                {lmsubtype.map((item, i) => (

                                                    <option key={i}
                                                            value={item.subType_ID}
                                                    >
                                                            {item.subType_Name}
                                                    </option>
                                                ))};
                                                </Input>
                                            </td></tr>
                                            <tr><td>
                                                <Label for="units">Units</Label>
                                                <Input
                                                    id="units"
                                                    name="units"
                                                type="select"
                                                        value={type_unit}
                                                onChange={(e) => this.setState({ selectedUnit: e.target.value })}
                                                >
                                                <option key="" value="">--Select--</option>
                                                {units.map((item, i) => (

                                                <option key={i}
                                                    value={item}
                                                >
                                                    {item}
                                                </option>
                                                ))};
                                                </Input>
                                            </td><td>
                                                <FormGroup>
                                                    <Label for="quantity">Quantity</Label>
                                                    <Input
                                                        id="quantity"
                                                        name="quantity"
                                                        type="number"
                                                        min="1"
                                                        onChange={(e) => this.setState({ selectedQuantity: e.target.value })}
                                                                value={quantity }
                                                    >
                                                    </Input>
                                                </FormGroup>
                                            </td></tr>
                                            <tr><td colSpan={2}>
                                                <FormGroup>
                                                    <Label for="Specification">Specification</Label>
                                                    <Row className="dblock" >
                                                        <Col className="col-10">
                                                        <Input
                                                        id="Specification"
                                                        name="Specification"
                                                                type="textarea"
                                                                rows={5}
                                                                className="ag-cell--math"
                                                                style={{ fontSize: "14px", backgroundColor:"#fff"} }
                                                                    onChange={(e) => {
                                                                        this.setState({ Specification: e.target.value });
                                                                        this.setState({ popSpecification: e.target.value });
                                                                        this.setState({ pTolerance: "" });
                                                                        this.setState({ mTolerance: "" });
                                                                       // console.log("Specification", e.target.value)
                                                                        this.autoPopulateDetails(e);
                                                                    }}
                                                        value={spec}
                                                        >
                                                        </Input>
                                                        </Col>
                                                        <Col className="col-2 p-0" style={{ position: "relative", } } >
                                                            <div style={{
                                                                position: "relative",
                                                                top: "50px",
                                                            }}>
                                                                <Button onClick={this.toggleNested} type="button" color="light" className="light-btn buttons primary primary_hover p-2 m-2">
                                                                    GDT
                                                                </Button>
                                                            </div>
                                                        </Col></Row>
                                                </FormGroup>
                                            </td></tr>
                                            <tr><td>
                                                <FormGroup>
                                                    <Label for="cmbTolerance">Tolerance Type</Label>
                                                    <Input
                                                        id="cmbTolerance"
                                                        name="cmbTolerance"
                                                        type="select"
                                                            defaultValue={typeTolerance}
                                                        onChange={(e) => {
                                                            //console.log(e.target.value)
                                                            this.setState({ selectedTolerance: e.target.value })
                                                        }
                                                        }
                                                    >
                                                        <option key="" value="">--Select--</option>
                                                        {cmbTolerance.map((item, i) => (

                                                            <option key={i}
                                                                value={i}
                                                            >
                                                                {item}
                                                            </option>


                                                        ))};
                                                    </Input>
                                                </FormGroup>
                                            </td></tr>
                                            <tr><td>
                                                <FormGroup>
                                                    <Label for="pTolerance">Upper Tolerance</Label>
                                                    <Input
                                                        id="pTolerance"
                                                        name="pTolerance"
                                                        type="text"
                                                         
                                                            onChange={(e) => {
                                                                this.setState({ pTolerance: e.target.value })
                                                               // console.log("pTolerance", e.target.value)
                                                                this.autoPopulateDetails(e);
                                                                    }
                                                                }
                                                            onBlur={(e) => this.autoPopulateDetails(e)}
                                                            value={plusTolerance}
                                                    >
                                                    </Input>
                                                </FormGroup>
                                            </td><td>
                                                <FormGroup>
                                                    <Label for="mTolerance">Lower Tolerance</Label>
                                                    <Input
                                                        id="mTolerance"
                                                        name="mTolerance"
                                                        type="text"
                                                                onBlur={(e) => this.autoPopulateDetails(e)}
                                                                onChange={(e) => {
                                                                    this.setState({ mTolerance: e.target.value })
                                                                   // console.log("mTolerance", e.target.value)
                                                                    this.autoPopulateDetails(e);
                                                                }
                                                                }
                                                                value={minusTolerance}
                                                    >
                                                    </Input>
                                                </FormGroup>
                                            </td></tr>
                                            <tr><td>
                                                <FormGroup>
                                                    <Label for="maxValue">Max Value</Label>
                                                    <Input
                                                        id="maxValue"
                                                        name="maxValue"
                                                        type="number"
                                                        min="0"
                                                        step=".01"
                                                        onChange={(e) => this.setState({ maxValue: e.target.value })}
                                                            value={maxValue}
                                                    >
                                                    </Input>
                                                </FormGroup>
                                            </td><td>
                                                <FormGroup>
                                                    <Label for="minValue">Min Value</Label>
                                                    <Input
                                                        id="minValue"
                                                        name="minValue"
                                                        type="number"
                                                                min="0"
                                                                step={this.state.dynamicMinStepValue}
                                                                onChange={(e) => this.setState({ minValue: e.target.value })}
                                                                
                                                                value={minValue}
                                                    >
                                                    </Input>
                                                </FormGroup>
                                            </td></tr>
                                            
                                    </tbody>
                                    </Table>
                                </Form>
                           </>
                        )}
                             
                    </ModalBody>
                    <ModalFooter className="d-none">
                        <Button color="light"
                            onClick={this.onHidePopup}
                  
                            className=" d-none light-btn primary buttons"
                        >
                            Cancel
                        </Button>
                        
                        </ModalFooter>
                        </Modal>
                    </Draggable>
            );
        } else {
            return <div />;
        }
    }
}
export default PopupModal;
