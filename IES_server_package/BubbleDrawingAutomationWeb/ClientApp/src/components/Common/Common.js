import React from 'react';
import Swal from 'sweetalert2'
import useStore from "../Store/store";
import initialState from "../Store/init";
import del from "../../assets/delete-white.svg"
import { AgGridReact, ValueSetterParams } from 'ag-grid-react';
import 'ag-grid-community/styles/ag-grid.css';
import 'ag-grid-community/styles/ag-theme-alpine.css';
import { v1 as uuid } from "uuid";
import {  instance }  from "../Client/http-common"
import _ from 'lodash';


const fetchConfig = async () => {
    const response = await fetch('/config.json');
    const settings = await response.json();
    useStore.setState({
        AppSettings: settings
    });

};

export const  config = {
    ENVIRONMENT:   process.env.REACT_APP_ENV,
    BASE_URL:  process.env.REACT_APP_SERVER,
    APP_TITLE:  process.env.REACT_APP_TITLE,
    console:  process.env.REACT_APP_CONSOLE === "true",
    };
 

// Begin API process
export async function fetchSearchData(state) {

    try {
        let url = "/api/drawingsearch/GetDrawingByNumber";
        await fetchConfig();
        const newstate = useStore.getState();

        instance.interceptors.request.use(config => {
            const baseURL = (newstate?.AppSettings?.REACT_APP_SERVER !== null) ? newstate?.AppSettings?.REACT_APP_SERVER : process.env.REACT_APP_SERVER;
            config.baseURL = `${baseURL}`;
            return config;
        }, error => {
            return Promise.reject(error);
        });

        const res = await instance.post(url, state);
        //console.log( "fetchSearchData", res)
        
        if (res.status === 204) {
            useStore.setState({ isLoading: false })
           
            showAlert("Invalid Input", "<p> Please check the drawing and revision number.</p>");
            const element = document.getElementById("DrawingNo");
            window.setTimeout(() => element.focus(), 0);
            return false;
        }
        if (res.status !== 200) {
            const message = `An error has occured, Please enter Valid Input.`;
            throw new Error(message);
        }

        return res;
    } catch (e) {
        console.error("catch",e);
    }

}

export async function rotateProcessApi(state) {
    try {
        let url = "/api/drawingsearch/rotate";
        const res = await instance.post(url, state);
        // console.log("rotateProcessApi",res)
        useStore.setState({ isLoading: false });

        if (res.status !== 200) {
            const message = `An error has occured, Please try again.`;
            throw new Error(message);
        }

        return res;
    } catch (e) {
        console.error("catch",e);
    }
}

export async function deleteBalloonProcessApi(state) {
    try {
        let url = "/api/drawingsearch/deleteBalloons";
        const res = await instance.post(url, state);
        //  console.log( "deleteBalloons", res)
        useStore.setState({ isLoading: false });

        if (res.status !== 200) {
            const message = `An error has occured, Please try again.`;
            throw new Error(message);
        }

        return res;
    } catch (e) {
        console.error("catch",e);
    }
     
}

export async function makeAutoballoonApi(state) {

    try {
        let url = "/api/drawingsearch/AutoBalloon";
        await fetchConfig();
        const newstate = useStore.getState();

        instance.interceptors.request.use(config => {
            const baseURL = (newstate?.AppSettings?.REACT_APP_SERVER !== null) ? newstate?.AppSettings?.REACT_APP_SERVER : process.env.REACT_APP_SERVER;
            config.baseURL = `${baseURL}`;
            return config;
        }, error => {
            return Promise.reject(error);
        });
        const res = await instance.post(url, state);
        //  console.log( "makeAutoballoonApi", res)
        useStore.setState({ isLoading: false });

        if (res.status !== 200) {
            const message = `An error has occured, Please try again.`;
            throw new Error(message);
        }

        return res;
    } catch (e) {
        console.error("catch",e);
    }
}

export async function makeSPLballoonApi(state) {

    try {
        let url = "/api/drawingsearch/SplBalloon";
        await fetchConfig();
        const newstate = useStore.getState();

        instance.interceptors.request.use(config => {
            const baseURL = (newstate?.AppSettings?.REACT_APP_SERVER !== null) ? newstate?.AppSettings?.REACT_APP_SERVER : process.env.REACT_APP_SERVER;
            config.baseURL = `${baseURL}`;
            return config;
        }, error => {
            return Promise.reject(error);
        });
        const res = await instance.post(url, state);
        //  console.log( "makeAutoballoonApi", res)
        useStore.setState({ isLoading: false });

        if (res.status === 201) {
            useStore.setState({ isLoading: false })
            //console.log("dummy",res);
            return {data:res.data};
        }

        if (res.status !== 200) {
            const message = `An error has occured, Please try again.`;
            throw new Error(message);
        }

        return res;
    } catch (e) {
        console.error("catch", e);
    }
}

export async function saveBalloonsApi(state) {
    try {
        let url = "/api/drawingsearch/saveBalloons";
        const res = await instance.post(url, state);
        //  console.log(response, "makeAutoballoonApi")
        useStore.setState({ isLoading: false });

        if (res.status !== 200) {
            const message = `An error has occured, Please try again.`;
            throw new Error(message);
        }

        return res;
    } catch (e) {
        console.error("catch",e);
    }
}

export async function resetBalloonsProcessApi(state) {
    try {
        let url = "/api/drawingsearch/resetBalloons";
        const res = await instance.post(url, state);
        //  console.log( "resetBalloonsProcessApi", res)
        useStore.setState({ isLoading: false });

        if (res.status !== 200) {
            const message = `An error has occured, Please try again.`;
            throw new Error(message);
        }

        return res;
    } catch (e) {
        console.error("catch",e);
    }
}

export async function reOrderBalloonsApi(state) {
    try {
        let url = "/api/drawingsearch/reOrderBalloons";
        const res = await instance.post(url, state);
        //  console.log( "resetBalloonsProcessApi", res)
        useStore.setState({ isLoading: false });

        if (res.status !== 200) {
            const message = `An error has occured, Please try again.`;
            throw new Error(message);
        }

        return res;
    } catch (e) {
        console.error("catch", e);
    }
}


export async function specificationUpdateApi(state) {
    try {
        let url = "/api/drawingsearch/specificationUpdate";
        await fetchConfig();
        const newstate = useStore.getState();

        instance.interceptors.request.use(config => {
            const baseURL = (newstate?.AppSettings?.REACT_APP_SERVER !== null) ? newstate?.AppSettings?.REACT_APP_SERVER : process.env.REACT_APP_SERVER;
            config.baseURL = `${baseURL}`;
            return config;
        }, error => {
            return Promise.reject(error);
        });
        const res = await instance.post(url, state);
        //  console.log( "resetBalloonsProcessApi", res)
        useStore.setState({ isLoading: false });

        if (res.status !== 200) {
            const message = `An error has occured, Please try again.`;
            throw new Error(message);
        }

        return res;
    } catch (e) {
        console.error("catch", e);
    }
}

export async function saveAllBalloonsApi(state) {
    try {
        let url = "/api/drawingsearch/saveAllBalloons";
        await fetchConfig();
        const newstate = useStore.getState();

        instance.interceptors.request.use(config => {
            const baseURL = (newstate?.AppSettings?.REACT_APP_SERVER !== null) ? newstate?.AppSettings?.REACT_APP_SERVER : process.env.REACT_APP_SERVER;
            config.baseURL = `${baseURL}`;
            return config;
        }, error => {
            return Promise.reject(error);
        });
        const res = await instance.post(url, state);
        //  console.log(response, "saveAllBalloons")
        useStore.setState({ isLoading: false });

        if (res.status !== 200) {
            const message = `An error has occured, Please try again.`;
            throw new Error(message);
        }

        return res;
    } catch (e) {
        console.error("catch", e);
    }
}

export async function specAutoPopulateApi(state) {
    try {
        let url = "/api/drawingsearch/specAutoPopulate";
        await fetchConfig();
        const newstate = useStore.getState();

        instance.interceptors.request.use(config => {
            const baseURL = (newstate?.AppSettings?.REACT_APP_SERVER !== null) ? newstate?.AppSettings?.REACT_APP_SERVER : process.env.REACT_APP_SERVER;
            config.baseURL = `${baseURL}`;
            return config;
        }, error => {
            return Promise.reject(error);
        });
        const res = await instance.post(url, state);
        //  console.log(response, "specAutoPopulate")
        if (res.status !== 200) {
            const message = `An error has occured, Please try again.`;
            throw new Error(message);
        }

        return res;
    } catch (e) {
        console.error("catch", e);
    }
}



// end API process

export const capitalizeFirstLetter = (string) => {
    return string.charAt(0).toUpperCase() + string.slice(1);
}

// Function to capitalize the first letter of each key in an object
export const capitalizeKeys = (obj) => {
    return _.mapKeys(obj, (_, key) => capitalizeFirstLetter(key));
}

export const recKey = [
    //"drawLineID",
    "baloonDrwID",
    "baloonDrwFileID",
    "productionOrderNumber",
    "part_Revision",
    "page_No",
    "drawingNumber",
    "revision",
    "balloon",
    "spec",
    "nominal",
    "minimum",
    "maximum",
    "measuredBy",
    "measuredOn",
    "circle_X_Axis",
    "circle_Y_Axis",
    "circle_Width",
    "circle_Height",
    "balloon_Thickness",
    "balloon_Text_FontSize",
    "zoomFactor",
    "crop_X_Axis",
    "crop_Y_Axis",
    "crop_Width",
    "crop_Height",
    "type",
    "subType",
    "unit",
    "quantity",
    "toleranceType",
    "plusTolerance",
    "minusTolerance",
    "maxTolerance",
    "minTolerance",
    "cropImage",
    "createdBy",
    "createdDate",
    "modifiedBy",
    "modifiedDate",
    "isCritical",
    "drawLineID",
];
export const orgKey = [
    //"DrawLineID"
     "BaloonDrwID"
    , "BaloonDrwFileID"
    , "ProductionOrderNumber"
    , "Part_Revision"
    , "Page_No"
    , "DrawingNumber"
    , "Revision"
    , "Balloon"
    , "Spec"
    , "Nominal"
    , "Minimum"
    , "Maximum"
    , "MeasuredBy"
    , "MeasuredOn"
    , "Circle_X_Axis"
    , "Circle_Y_Axis"
    , "Circle_Width"
    , "Circle_Height"
    , "Balloon_Thickness"
    , "Balloon_Text_FontSize"
    , "ZoomFactor"
    , "Crop_X_Axis"
    , "Crop_Y_Axis"
    , "Crop_Width"
    , "Crop_Height"
    , "Type"
    , "SubType"
    , "Unit"
    , "Quantity"
    , "ToleranceType"
    , "PlusTolerance"
    , "MinusTolerance"
    , "MaxTolerance"
    , "MinTolerance"
    , "CropImage"
    , "CreatedBy"
    , "CreatedDate"
    , "ModifiedBy"
    , "ModifiedDate"
    , "IsCritical"
    , "DrawLineID"
];


export const showAlertOnReAnnotation = (state) => {
    Swal.fire({
        title: 'Do you want to save the changes?',
        showCancelButton: true,
        confirmButtonText: 'Save',
        allowOutsideClick: false,
        allowEscapeKey: false
    }).then((result) => {
            /* Read more about isConfirmed, isDenied below */
            if (result.isConfirmed) {
               // console.log("originalRegions", "handleMouseUp")
                useStore.setState({
                    drawingRegions: state.drawingRegions,
                    originalRegions: state.originalRegions,
                    draft: state.draft,
                });
                selectedRegionProcess(state.originalRegions);
            } 
        })
}


export const resetBalloonsProcess = ( ) => {
    const state = useStore.getState();
    const {
        originalRegions,
        ItemView,
        drawingDetails
    } = state;
    let Page_No = 0;
    if (drawingDetails.length > 0 && ItemView != null) {
        Page_No = parseInt(Object.values(drawingDetails)[parseInt(ItemView)].currentPage);
    }
    useStore.setState({ selectedRowIndex: null });
    useStore.setState({ isLoading: true, loadingText: "Resetting Balloon... Please Wait..." })
    let resetData = originalRegions.map((item) => {
        if (item.Page_No !== Page_No) {
            return item;
        }
        return false;
    }).filter(item => item !== false);
    setTimeout(() => {
        //const resetOverData = JSON.parse(JSON.stringify(resetData));
        const resetOverData = [...resetData];

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
        console.log(unique)
        //useStore.setState({ isLoading: false });
       // return;

        let newitems = [];
    
         unique.reduce((prev, curr, index) => {
            const id = uuid();
            let newarr = [];
            let Balloon = index + 1;
            Balloon = Balloon.toString();
            if (curr.Quantity === 1 && curr.subBalloon.length === 0) {
                prev.push({ b:  (Balloon), c: prev.length + 1 })
                let i = prev.length;
                newarr.push({ ...curr, newarr: { ...curr.newarr, Balloon: Balloon }, id: id, DrawLineID: i, Balloon: Balloon });
            }
            if (curr.Quantity === 1 && curr.subBalloon.length > 0) {
                let pb = parseInt(Balloon).toString() + ".1";
                prev.push({b:pb, c:  prev.length + 1 })
                let i = prev.length;
                newarr.push({ ...curr, newarr: { ...curr.newarr, Balloon: pb },  id: id, DrawLineID: i, Balloon: pb });
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
 
                    let setter = { ...e, newarr: { ...e.newarr, Balloon: b },  id: sid, DrawLineID: i, Balloon: b };
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
                    let pb = parseInt(curr.Balloon).toString() + "." + qi.toString();
                    let newMainItem = [];
                    newMainItem = Qtyparent.map(item => {
                        if (pb === item.Balloon) {
                            return item;
                        }
                        return false;
                    }).filter(x => x !== false);
                    if (newMainItem.length > 0) {

                        newMainItem.map((nmi) => {
                            const qid = uuid();
                            let pb = parseInt(Balloon).toString() + "." + (qi).toString();
                            prev.push({ b: pb, c: prev.length + 1 })
                            let i = prev.length;

                            newarr.push({ ...nmi, newarr: { ...nmi.newarr, Balloon: pb }, id: qid, DrawLineID: i, Balloon: pb });
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
                                let setter = { ...e, newarr: { ...e.newarr, Balloon: b }, id: sqid, DrawLineID: i, Balloon: b };
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
        if (config.console)
        console.log(newitems)
        //useStore.setState({ isLoading: false });
        //return; 
        const newstate = useStore.getState();
        let newrect = newBalloonPosition(newitems, newstate);
        useStore.setState({
            originalRegions: newitems,
            draft: newitems, 
            drawingRegions: newrect,
            balloonRegions: newrect
        });
        useStore.setState({ isLoading: false });
    }, 300);
     
    return false;

}

export const selectedSPLRegionProcess = (newAnnotation) => {
    const state = useStore.getState();
    if (state.isErrImage) {
        return;
    }
    const {
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
    let CrevNo = drawingHeader[0].revision_No;
    let Page_No = 0;
    let totalPage = 0;
    let rotation = 0;
    let rotate_properties = [];
    let origin = [];

    if (drawingDetails.length > 0 && ItemView != null) {
        Page_No = Object.values(drawingDetails)[parseInt(ItemView)].currentPage;
        totalPage = Object.values(drawingDetails)[parseInt(ItemView)].totalPage;
        rotation = Object.values(drawingDetails)[parseInt(ItemView)].rotation;
        let rotate = drawingDetails.map(s => parseInt(s.rotation));
        rotate_properties = JSON.stringify(rotate);
        origin = Object.values(partial_image)[parseInt(ItemView)];
    }
    useStore.setState({ isLoading: true, loadingText: `Processing...` })
    const newDraw = newAnnotation.map((item, i) => {
        if (!item.hasOwnProperty("newarr")) {
            return { ...item };
        }
        return false;
    }).filter(item => item !== false);

    const oldDraw = newAnnotation.map((item, i) => {
        let ii = {};
        if (item.hasOwnProperty("newarr")) {
            const id = uuid();
           
            let w = parseInt(item.newarr.Crop_Width * 1);
            let h = parseInt(item.newarr.Crop_Height * 1);
            let x = parseInt(item.newarr.Crop_X_Axis * 1);
            let y = parseInt(item.newarr.Crop_Y_Axis * 1);
            let cx = parseInt(item.newarr.Circle_X_Axis * 1);
            let cy = parseInt(item.newarr.Circle_Y_Axis * 1);
            return { ...ii, ...item.newarr, Crop_Width: w, Crop_Height: h, Crop_X_Axis: x, Crop_Y_Axis: y, Circle_X_Axis: cx, Circle_Y_Axis: cy, x, y, width:w, height:h, id, isballooned: true, selectedRegion: "" };
        }
        return false;
    }).filter(item => item !== false);
    oldDraw.push(newDraw[0]);
    // console.log("selectedRegionProcess", newDraw, oldDraw)
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
        pageNo: Page_No,
        totalPage: totalPage,
        annotation: oldDraw,
        selectedRegion: selectedRegion,
        balloonRegions: [],
        drawingRegions: [],
        originalRegions: oldDraw,
        rotate: rotate_properties,
        origin: [origin],
        bgImgRotation: rotation

    };
    useStore.setState({ selectedRowIndex: null });
    setTimeout(() =>
        makeSPLballoonApi(req).then(r => {
            return r.data;
        })
            .then(r => {
                useStore.setState({ isLoading: false });

                if (r.length > 0) {
                    if (config.console)
                        console.log("saved data", r)
                     r.map((item, index) => {
                        if (item.hasOwnProperty("drawLineID")) {
                            delete item.drawLineID;
                        }
                        item.balloon = item.balloon.replaceAll("-", ".");
                        return item;
                    });
                    let newrects = [];

                    //clone a array of object
                    // oversearchData = JSON.parse(JSON.stringify(r));
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
                                            items[i] = { ...main[0], id: qid, DrawLineID: i };

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

                    //return false;
                    newrects = newitems.map((item, ind) => {
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


                    let dummy = newrects.slice(-1);
     
                    if (dummy[0].Spec === "") {
                        //alert("The OCR Unable to extract Balloon details on this SPl region.");
                        
                        Swal.fire({
                            title: 'Oops!',
                            icon: "",
                            html: "The OCR Unable to extract Balloon details on this SPl region.",
                            showConfirmButton: true
                        }).then((r) => {
                            if (r.isConfirmed) {
                                const dstate = useStore.getState();
                                setTimeout(function () {
                                    let scrollElement = document.querySelector('#konvaMain');
                                    if (scrollElement !== null) {
                                        scrollElement.scrollLeft = dstate.scrollPosition;
                                    }

                                }, 500);
                            }
                        });
                        newrects.map((item) => {
                            if (parseInt(item.Balloon) === parseInt(dummy[0].Balloon)) {
                                item.isballooned = false;
                                item.isDeleted = false;
                                return item;
                            }
                            return false;
                        }).filter((item) => item !== false);
                        useStore.setState({ selectedBalloon: dummy[0].Balloon });
                    }
                    
                    useStore.setState({
                        originalRegions: newrects,
                        draft: newrects,
                        intBubble: 1
                    });
                    const state1 = useStore.getState();
                    let newDraw = newBalloonPosition(newrects, state1);
                    useStore.setState({
                        drawingRegions: newDraw,
                        balloonRegions: newDraw,
                        intBubble: 1
                    });
                } else {
                    useStore.setState({
                        originalRegions: [],
                        draft: [],
                        drawingRegions: [],
                        balloonRegions: [],
                    });
                }


            }, (error) => {
                let state = useStore.getState();
                const prev = state.originalRegions.filter(r => r.isballooned === true);
                useStore.setState({
                    originalRegions: prev,
                    draft: state.draft,
                })
                let nstate = useStore.getState();
                const nprev = nstate.originalRegions.filter(r => r.isballooned === true);
                let prevDraw = newBalloonPosition(nprev, nstate);
                useStore.setState({
                    drawingRegions: prevDraw,
                    balloonRegions: prevDraw,
                });
                console.log("Error", error);
                useStore.setState({ isLoading: false });
            }).catch(error => {
                console.log(error);
                useStore.setState({ isLoading: false });
            })
        , 100);
    return false;

}

export const selectedRegionProcess = (newAnnotation) => {
    const state = useStore.getState();
    if (state.isErrImage) {
        return;
    }
    const {
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
    let CrevNo = drawingHeader[0].revision_No;
    let Page_No = 0;
    let totalPage = 0;
    let rotation = 0;
    let rotate_properties = [];
    let origin = [];

    if (drawingDetails.length > 0 && ItemView != null) {
        Page_No = Object.values(drawingDetails)[parseInt(ItemView)].currentPage;
        totalPage = Object.values(drawingDetails)[parseInt(ItemView)].totalPage;
        rotation = Object.values(drawingDetails)[parseInt(ItemView)].rotation;
        let rotate = drawingDetails.map(s => parseInt(s.rotation));
        rotate_properties = JSON.stringify(rotate);
        origin = Object.values(partial_image)[parseInt(ItemView)];
    }
    useStore.setState({ isLoading: true, loadingText: `Processing...` })
    const newDraw = newAnnotation.map((item, i) => {
        if (!item.hasOwnProperty("newarr")) {
            return { ...item };
        } 
        return false;
    }).filter(item => item !== false);

    let oldDraw = newAnnotation.map((item, i) => {
  
        if (item.hasOwnProperty("newarr")) {
           // console.log("i am here")
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
            return { ...item, Crop_Width: w, Crop_Height: h, Crop_X_Axis: x, Crop_Y_Axis: y, Circle_X_Axis: cx, Circle_Y_Axis: cy, height: h, width: w, x: x, y: y, id, isballooned: true, selectedRegion :""};
        }

        return false;
    }).filter(item => item !== false);
    oldDraw.push(newDraw[0]);
    //console.log("selectedRegionProcess", newDraw, oldDraw, selectedRegion)
    ///return
 

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
        pageNo: Page_No,
        totalPage: totalPage,
        annotation: oldDraw,
        selectedRegion: selectedRegion,
        balloonRegions: [],
        drawingRegions: [],
        originalRegions: oldDraw,
        rotate: rotate_properties,
        origin: [origin],
        bgImgRotation: rotation

    };
    //console.log("selectedRegionProcess", newDraw, oldDraw)
    //return false;
    useStore.setState({ selectedRowIndex: null });
    setTimeout(() =>
        makeAutoballoonApi(req).then(r => {
            return r.data;
        })
            .then(r => {
                useStore.setState({ isLoading: false });

                if (r.length > 0) {
                    if (config.console)
                        console.log(r, "selectedRegionProcess res")
                    r = r.map((item, index) => {
                        if (item.hasOwnProperty("drawLineID")) {
                            delete item.drawLineID;
                        }
                        item.balloon = item.balloon.replaceAll("-", ".");
                        return item;
                    });
 
                    let newrects = [];

                    //clone a array of object
                    //const oversearchData = JSON.parse(JSON.stringify(r));
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
                                            items[i] = { ...main[0], id: qid, DrawLineID: i };

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

                    //return false;
                    newrects = newitems.map((item, ind) => {
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
                        return { ...res, x, y, width: w, height: h, id: id, isballooned: true, selectedRegion: "", DrawLineID: ind+1 };
                    })
                    newrects = shortBalloon(newrects, "DrawLineID");
                    if (config.console)
                        console.log(newrects)

                    useStore.setState({
                        originalRegions: newrects,
                        draft: newrects,
                        intBubble: 1
                    });
                        const state1 = useStore.getState();
                        let newDraw = newBalloonPosition(newrects, state1);
                        useStore.setState({
                            drawingRegions: newDraw,
                            balloonRegions: newDraw,
                            intBubble: 1
                        });
                    } else {
                        useStore.setState({
                            originalRegions: [],
                            draft: [],
                            drawingRegions: [],
                            balloonRegions: [],
                        });
                    }
                

            }, (error) => {
                let state = useStore.getState();
                const prev = state.originalRegions.filter(r => r.isballooned === true);
                useStore.setState({
                    originalRegions: prev,
                    draft: state.draft,
                })
                let nstate = useStore.getState();
                const nprev = nstate.originalRegions.filter(r => r.isballooned === true);
                let prevDraw = newBalloonPosition(nprev, nstate);
                useStore.setState({
                    drawingRegions: prevDraw,
                    balloonRegions: prevDraw,
                });
                console.log("Error", error);
                useStore.setState({ isLoading: false });
            }).catch(error => {
                console.log(error);
                useStore.setState({ isLoading: false });
            })
        , 100);
    return false;
}

export const actualSize = () => {
    let stte = useStore.getState();
    if (stte.isErrImage) {
        return;
    }
 
    const props = useStore.getState();
    let pageNo = 0;
    let resize = "false";
    let superScale = [];
    if (props.drawingDetails.length > 0 && props.ItemView != null) {
        pageNo = parseInt(Object.values(props.drawingDetails)[parseInt(props.ItemView)].currentPage);
        resize = props.drawingDetails.length > 0 ? Object.values(props.drawingDetails)[parseInt(props.ItemView)].resize : "false";
        superScale = props.partial_image.filter((a) => { return a.item === parseInt(props.ItemView); });
    }
    if (config.console)
    console.log( pageNo, resize, superScale, window.innerWidth, window.innerHeight, superScale );
    
    useStore.setState({ history: [], fitscreen:false, win: initialState.win, isDisabledZoomIn: false, isDisabledFIT: false });
  

       // useStore.setState({ history: [], win: initialState.win, isDisabledZoomIn: false, isDisabledFIT: false });
        setTimeout(function () {
            let state = useStore.getState();
            var padding = state.pad;
            var w = state.imageWidth;
            var h = state.imageHeight;

            // get the aperture we need to fit by taking padding off the stage size.
            var targetW = initialState.win.width - (2 * padding);
            var targetH = initialState.win.height - (2 * padding);

            // compute the ratios of image dimensions to aperture dimensions 
            var widthFit = targetW / w;
            var heightFit = targetH / h;

            // compute a scale for best fit and apply it
            let diffscale = w / h;
            if (diffscale < 1) {

                useStore.setState({ scrollPosition: 0 });
            }
            var scale = Math.max(widthFit, heightFit); // (widthFit > heightFit) ? ((diffscale < 1) ? widthFit : heightFit) : widthFit;


            w = parseInt(w * scale , 10);
            h = parseInt(h * scale , 10);

            let x = 0;
            let y = 0;
          //  let minusWidth = 0
            
            if (w < state.win.width) {
                x = (state.win.width - w) / 2;
            }
            if (h < state.win.height) {
                y = (state.win.height - h) / 2;
            }
            // console.log("diffscale",scale, diffscale)
           /*
            if (h > state.win.height) {
                y = state.win.height
                minusWidth = state.imageWidth * (state.win.height / parseFloat(state.imageHeight.toString()));
                x = minusWidth;
            }
            if (w > state.win.width) {
                x = state.win.height - minusWidth;
                y = state.imageHeight * ((state.win.width - minusWidth) / parseFloat(state.imageWidth.toString()));
            }
            */
            let sub = (1 - scale);
            const step = sub / 5; // 5 step to view
            let rpobj = { scaleStep: step, InitialScale: sub, imgscale: scale, bgImgScale: scale, bgImgW: w, bgImgH: h, bgImgX: x, bgImgY: y };
            useStore.setState(rpobj);
            let nstate = useStore.getState();
            let rescale = 1.15;
            let nscale = nstate.scaleStep + nstate.InitialScale;
            if (resize === "true") {
                if (config.console)
                    console.log(superScale)
                if (nstate.imageHeight >= nstate.maxScaleSize || nstate.imageWidth >= nstate.maxScaleSize) {
                    // rescale = (nstate.imageWidth / nstate.imageHeight) / 2;
                    let maxsize = Math.max(superScale[0].fullWidth , superScale[0].fullHeight); // (superScale[0].fullWidth > superScale[0].fullHeight) ? superScale[0].fullWidth : superScale[0].fullHeight;
                    rescale = (maxsize / nstate.maxScaleSize);
                    let cancassize = Math.max(nstate.imageWidth, nstate.imageHeight);
                    rescale = cancassize / maxsize;
                    if (diffscale > 1) {
                        rescale = (diffscale  * 50 )/100; // 30 percentage initial zoomed
                    }
                    nscale = 0.75;
                    if (config.console)
                    console.log("as", rescale, nstate.imageWidth /superScale[0].fullWidth)
                }
            } else {
                if (nstate.imageHeight >= nstate.maxScaleSize || nstate.imageWidth >= nstate.maxScaleSize) {
                    // rescale = (nstate.imageWidth / nstate.imageHeight) / 2;
                    let maxsize = (nstate.imageWidth > nstate.imageHeight) ? nstate.imageWidth : nstate.imageHeight;
                    rescale = (maxsize / nstate.maxScaleSize);
                    rescale = (rescale * 70 )/ 100;
                    if (config.console)
                    console.log("as", rescale)
                }
            }
            if (config.console)
            console.log("sss", nstate.bgImgW, nstate.bgImgH, nscale,superScale, rescale, nstate.imageWidth , nstate.imageHeight )
            
            let nw = parseInt(nstate.bgImgW * (nscale * rescale), 10);
            let nh = parseInt(nstate.bgImgH * (nscale * rescale), 10);
            let x1 = 0;
            let y1 = 0;
            if (nstate.imageWidth > nw && nstate.imageHeight > nh) {

                if (nw > nstate.win.width || nh > nstate.win.height) {
                    let newwin = {
                        width: (nw > nstate.win.width ? (nw + (2 * padding)) : (nstate.win.width)),
                        height: (nh > nstate.win.height ? (nh + (2 * padding)) : (nstate.win.height))
                        }
                     useStore.setState({ win: newwin });
                }
                let newstate = useStore.getState();
               // if (nw < newstate.win.width) {
                    x1 = (newstate.win.width - nw) / 2;
               // }
               // if (nh < newstate.win.height) {
                    y1 = (newstate.win.height - nh) / 2;
               // }
               // console.log('resized')
            }
           //console.log(nw, nh, x1, y1, scale, nscale, nstate.win, initialState.win, resize)
            let zobj = { bgImgScale: nscale, bgImgW: nw, bgImgH: nh, bgImgX: x1, bgImgY: y1 };
            useStore.setState(zobj);
            // console.log(nscale)
            useStore.setState({ zoomingfactor: nscale });
            
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
                if (scrollElement !== null) {
                   // console.log("fitToActualsize ")
                    scrollElement.scrollLeft = (scrollElement.scrollWidth - scrollElement.clientWidth) / 2;
                }
 
         
 

        }, 200);
 
        const dstate = useStore.getState();
            if (dstate.scrollPosition === 0) {
                setTimeout(function () {
                    let scrollElement = document.querySelector('#konvaMain');
                    if (scrollElement !== null) {
                       // console.log(" common actual size ", dstate)
                        scrollElement.scrollLeft = ((scrollElement.scrollWidth - scrollElement.clientWidth) / 2);
                    }
                   
                }, 350);
            } else {
                setTimeout(function () {
                    if (config.console)
                    console.log(" common actual size ", dstate)
                    let scrollElement = document.querySelector('#konvaMain');
                    if (scrollElement !== null) {
                        scrollElement.scrollLeft = dstate.scrollPosition;
                        //useStore.setState({ scrollPosition: 0 });
                        scrollElement.scrollTop = dstate.konvaPositionTop;
                    }
                 
                }, 350);
            }

};

export const fitSize = () => {
    useStore.setState({ history: [], fitscreen: true, win: { width: window.innerWidth - 100, height: window.innerHeight } });
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
        let winwidth = state.win.width;
        if (nw < state.win.width) {
            x = (winwidth - nw) / 2;
        }
        let win = { width: winwidth, height: nh };
        let rpobj = { win: win, bgImgScale: scale, bgImgW: nw, bgImgH: nh, bgImgX: x, bgImgY: y };
        useStore.setState(rpobj);
        let nstate = useStore.getState();
        let originalRegions = nstate.originalRegions;
        let newrect = newBalloonPosition(originalRegions, nstate);
        useStore.setState({
            savedDetails: false,
            drawingRegions: newrect,
            balloonRegions: newrect,
            //    isDisabledAutoB: true

        });

        let scrollElement = document.querySelector('#konvaMain');
        scrollElement.scrollLeft = (scrollElement.scrollWidth - scrollElement.clientWidth) / 2;
        scrollElement.scrollTop = 0;
        document.body.scrollTop = 0;
        useStore.setState({
            scrollPosition: (scrollElement.scrollWidth - scrollElement.clientWidth) / 2,
            konvaPositionTop: 0,
            documentPositionTop: 0,

        });

    }, 200);
}
export function scalledPosition(item) {
    const props = useStore.getState();
    let scaleFactor = props.bgImgScale;
    const x = item.dx * scaleFactor;
    const y = item.dy * scaleFactor;
    const w = item.dw * scaleFactor;
    const h = item.dh * scaleFactor;
    return { x, y, w, h }
}

export function newBalloonPosition(originalRegions, state) {
    const props = useStore.getState();
  
    let pageNo = 0;
    let resize = "false";
    let superScale = [];
    if (props.drawingDetails.length > 0 && props.ItemView != null) {
        pageNo = parseInt(Object.values(props.drawingDetails)[parseInt(props.ItemView)].currentPage);
        resize = props.drawingDetails.length > 0 ? Object.values(props.drawingDetails)[parseInt(props.ItemView)].resize : "false";
        superScale = props.partial_image.filter((a) => { return a.item === pageNo; });
    }
    if (config.console)
    console.log(pageNo, resize, superScale )

    // Scalling the Original values and assign to drawingRegions
    const newballoon = originalRegions.map((item) => {
        if (config.console)
        console.log("newballoon all", item);
        if (item.Page_No === pageNo && item.isballooned === true) {
            // Calculate scaling factors
           //  console.log("newballoon page", item)
            //if (item.hasOwnProperty("newarr") && item.hasOwnProperty("subBalloon")  ) {
                const scaleX = props.bgImgW / props.imageWidth;
                const scaleY = props.bgImgH / props.imageHeight;
                let x = item.newarr.Crop_X_Axis * scaleX + props.bgImgX;
                let y = item.newarr.Crop_Y_Axis * scaleY + props.bgImgY;
                let w = item.newarr.Crop_Width * scaleX;
                let h = item.newarr.Crop_Height * scaleY;

                if (item.newarr.Crop_X_Axis === 0) { x = 28; w = item.newarr.Crop_Width; }
                if (item.newarr.Crop_Y_Axis === 0) { y = 28; h = item.newarr.Crop_Height; }

                if (scaleX === Infinity || scaleY === Infinity || x === Infinity || y === Infinity || w === Infinity || h === Infinity ) {
                    item.dx = 0;
                    item.dy = 0;
                    item.radius = 10;
                    return item;
            }
            item.Circle_X_Axis = 0;
            item.Circle_Y_Axis = 0;
            let a = item.newarr.Circle_X_Axis * scaleX + props.bgImgX;
            let b = item.newarr.Circle_Y_Axis * scaleY + props.bgImgY;
            //console.log("newballoon page", parseInt(item.Balloon), a, b)
                item.dx = 0;
                item.dy = 0;
                item.radius = 10;
                item.x = x;
                item.y = y;
                item.width = w;
                item.height = h;
                item.Crop_X_Axis = x;
                item.Crop_Y_Axis = y;
                item.Crop_Width = w;
                item.Crop_Height = h;
                item.Circle_X_Axis = a;
                item.Circle_Y_Axis = b;
                item.intBalloon = parseInt(item.Balloon);
            //}
            return item;
        }
        return item;
    });
    if (config.console)
    console.log("newballoon", newballoon)

    // Get all original values from API and remove part value by int of balloon to remove partial balloons
    const real = originalRegions.map(a => {
        if (config.console)
        console.log("real a", a.Page_No, pageNo, a.isballooned, props.drawingDetails)
        if (a.Page_No === pageNo && a.isballooned === true) {
            let intBalloon = parseInt(a.Balloon);
            return { ...a.newarr, intBalloon: intBalloon };
        }
        return false;
        })
        // remove false item
        .filter(item => item !== false)
        // remove duplicates propert by key as intBalloon
        .reduce((resArr, currentArr) => {
        let other = resArr.some((ele) => currentArr.intBalloon === ele.intBalloon)
        if (!other) resArr.push(currentArr)
        return resArr
    }, [])
    if (config.console)
        console.log("real", real)

    useStore.setState({ zoomoriginalRegions: [] });
    // reset the movable balloon for current page and create move action
    let thiscircles = props.zoomoriginalRegions;

    thiscircles = newballoon.map(a => {
        if (a.Page_No === pageNo) {
            a.intBalloon = parseInt(a.Balloon);
            return a;
        }
        return false;
        })
        // remove false item
        .filter(item => item !== false)
        // remove duplicates propert by key
        .reduce((resArr, currentArr) => {
            let other = resArr.some((ele) => currentArr.intBalloon === ele.intBalloon)
            if (!other) resArr.push(currentArr)
            return resArr
        }, [])
    if (config.console)
    console.log("thiscircles", thiscircles)
    let circles = [];
    circles = real.map(item => {
        const scaleX = state.bgImgW / state.imageWidth;
        const scaleY = state.bgImgH / state.imageHeight;
        let a = item.Circle_X_Axis * scaleX + state.bgImgX;
        let b = item.Circle_Y_Axis * scaleY + state.bgImgY ;
        let intBalloon = parseInt(item.Balloon)
        let radius = 10;

        switch (intBalloon.toString().length) {
            case 1:
                radius = 10;
                break;
            case 2:
                radius = 11;
                break;
            case 3:
                radius = 12;
                break;
            case 4:
                radius = 13;
                break;
            default:
                radius = 10;
                break;
        }
        if (state.fitscreen) {
            radius = radius / 1.5;
        }
        return { x: a, y: b, id: intBalloon, radius: radius, intBalloon: intBalloon }
    }).reduce((resArr, currentArr) => {
        let other = resArr.some((ele) => currentArr.intBalloon === ele.intBalloon)
        if (!other) resArr.push(currentArr)
        return resArr
    }, [])
    if (config.console)
    console.log("extracted" ,circles)
    thiscircles = movablec(circles, thiscircles);
 
    useStore.setState({
        zoomoriginalRegions: thiscircles
});

    return newballoon; 

}
 

export const movablec = (circles, thiscircles) => {
    if (config.console)
    console.log('before Clicked Shapes: ', circles, thiscircles)
   
    if (circles) {
        for (let i = 0; i < circles.length; i++) {
            for (let j = i + 1; j < circles.length; j++) {
                const circle1 = circles[i];
                const circle2 = circles[j];

                const distance = Math.sqrt(Math.pow(circle2.x - circle1.x, 2) + Math.pow(circle2.y - circle1.y, 2));
                const sumOfRadii = circle1.radius + circle2.radius+2;

                if (distance < sumOfRadii) {
                    const overlap = circle1.radius + circle2.radius - distance;
                    const dx = (circle2.x - circle1.x) / distance;
                    const dy = (circle2.y - circle1.y) / distance;
                    const moveDistance = overlap + 1;
                    if (config.console)
                    console.log(`Overlap detected between circles ${circle1.id} and ${circle2.id}`, circle1, circle2, overlap, dx, dy);
                    thiscircles = thiscircles.map(item => {

                        if (item.intBalloon === parseInt(circle1.id)) {
                           // console.log(`${circle1.id}`)
                            let dx1 = item.dx - (dx * moveDistance);
                            let dy1 = item.dy - (dy * moveDistance);
                            if (isNaN(dx1) || isNaN(dy1)) { dx1 = 10; dy1 = 0; }
                            return { ...item, dx: dx1, dy: dy1 };
                        }
                        if (item.intBalloon === parseInt(circle2.id)) {
                           // console.log(`${circle2.id}`)
                            let dx2 = item.dx + (dx * moveDistance);
                            let dy2 = item.dy + (dy * moveDistance);
                            if (isNaN(dx2) || isNaN(dy2)) { dx2 = 0; dy2 = 10; }
                            return { ...item, dx: dx2, dy: dy2 };
                        }
                        return item;
                    });

                }
            }
        }
    }
    if (config.console)
   console.log('after Clicked Shapes: ', thiscircles)
    return thiscircles;
}
export function ballonOriginalPosition(newone) {
    const props = useStore.getState();
    const scaleX = props.bgImgW / props.imageWidth;
    const scaleY = props.bgImgH / props.imageHeight;
    let old_Circle_X_Axis = 0;
    let old_Circle_Y_Axis = 0;
    if (newone.hasOwnProperty("newarr")) {
        old_Circle_X_Axis = newone.newarr.Circle_X_Axis;
        old_Circle_Y_Axis = newone.newarr.Circle_Y_Axis;
    }
    let scaledX = old_Circle_X_Axis + (newone.xx / scaleX);
    let scaledY = old_Circle_Y_Axis + (newone.xy / scaleY);
    //console.log(newone, scaleX, scaleY)
    return { x: scaledX, y: scaledY };
}

export function originalPosition(newone) {
    const props = useStore.getState();
 
    let pageNo = 0;
    let resize = "false";
    if (props.drawingDetails.length > 0 && props.ItemView != null) {
        pageNo = parseInt(Object.values(props.drawingDetails)[parseInt(props.ItemView)].currentPage);
        resize = Object.values(props.drawingDetails)[parseInt(props.ItemView)].resize ;
    }
    if (config.console)
    console.log(pageNo, resize)
    //return false;
    let originalImageWidth = props.imageWidth;
    let originalImageHeight = props.imageHeight;
    let scaledImageWidth = props.bgImgW;
    let scaledImageHeight = props.bgImgH;
    // Rectangle position and size on the scaled image
    let scaledX = newone.x;
    let scaledY = newone.y;
    let scaledWidth = newone.width;
    let scaledHeight = newone.height;
    // Calculate scaling factors
    const scaleX = originalImageWidth / scaledImageWidth;
    const scaleY = originalImageHeight / scaledImageHeight;
    // Calculate original position and size
    let x = 0; let y = 0; let width = 0; let height = 0;
    let newX = scaledX - props.bgImgX;
    let newY = scaledY - props.bgImgY;
    let newW = scaledWidth * scaleX;
    let newH = scaledHeight * scaleY;
    const originalX = newX * scaleX;
    const originalY = newY * scaleY;

    // console.log("Original",originalX, originalY, newW, newH)
    // Find x position from the Canvas
    if (Math.sign(newX) === -1) {
        x = 0;
    } else if (newX < props.imageWidth) {
        x = originalX;
    } else {
        x = props.imageWidth;
    }
    // Find y position from the Canvas
    if (Math.sign(newY) === -1) {
        y = 0;
    } else if (newY < props.imageHeight) {
        y = originalY;
    } else {
        y = props.imageHeight;
    }
    // Find Width position from the Canvas
    if (Math.sign(newW) === -1) {
        width = 0;
    } else if (newW < props.imageWidth) {
        width = newW;
    } else {
        width = props.imageWidth;
    }
    // Find Height position from the Canvas
    if (Math.sign(newH) === -1) {
        height = 0;
    } else if (newH < props.imageHeight) {
        height = newH;
    } else {
        height = props.imageHeight;
    }
    return { x, y, width, height };
}

export const rotatePoint = ({ x, y }, rad) => {
    const rcos = Math.cos(rad);
    const rsin = Math.sin(rad);
    return { x: x * rcos - y * rsin, y: y * rcos + x * rsin };
};

export const rotateAroundCenter = (node, rotation) => {
    const topLeft = { x: -node.width / 2, y: -node.height / 2 };
    const current = this.rotatePoint(topLeft.x, topLeft.y, 0);
    const rotated = this.rotatePoint(topLeft.x, topLeft.y, 90);
    const dx = rotated.x - current.x,
        dy = rotated.y - current.y;

    node.rotation(rotation);

    node.x(node.x() + dx);
    node.y(node.y() + dy);

    return node;
}

export const GetRotatePosition = (rotation) => {
    const state = useStore.getState();
    // let bgImgRotation = state.bgImgRotation;
    var padding = state.pad;
    var w = state.imageWidth;
    var h = state.imageHeight;

    // get the aperture we need to fit by taking padding off the stage size.
    var targetW = state.win.width - (2 * padding);
    var targetH = state.win.height - (2 * padding);

    // compute the ratios of image dimensions to aperture dimensions 
    var widthFit = targetH / w;
    var heightFit = targetW / h;

    // compute a scale for best fit and apply it
    var scale = (widthFit > heightFit) ? heightFit : widthFit;

    w = parseInt(w * scale, 10);
    h = parseInt(h * scale, 10);

    let x = 0;
    let y = 0;
    if (w < state.win.height) {
        x = (state.win.height - w) / 2;
    }
    if (h < state.win.width) {
        y = (state.win.width - h) / 2;
    }
    let rpobj = { x, y, w, h }
    if (rotation === 360 || rotation === 0) {
        rpobj = { bgImgX: 0, bgImgY: 0 };
        //  useStore.setState(rpobj);
    } else {
        rpobj = { bgImgX: 1315, bgImgY: 0 };
        //  useStore.setState(rpobj);
    }

    return rpobj;
}

export const validate = (drawingNo, revNo) => {

    const errors = [];
    if (drawingNo.length === 0) {

        errors.push({ field: "drawingNo", "message": "Drawing.No can't be empty" });
    }
    if (revNo.length === 0) {

        errors.push({ field: "revNo", "message": "Revision.No can't be empty" });
    }
    return errors;
}

export const shortBalloon = (json, prop) => {

    return json.sort(function (a, b) {
        if (parseFloat(a[prop]) > parseFloat(b[prop])) {
            return 1;
        } else if (parseFloat(a[prop]) < parseFloat(b[prop])) {
            return -1;
        }
        return 0;
    });

}

export const showAlert = (title, text) => {
  return  Swal.fire({
        title: title,
        html: text,
        icon: "",
        confirmButtonText: "OK",
    });
}

export const showAlertOnReset = () => {
    Swal.fire({
        title: 'Do you want to Clear Drawing?',
        showCancelButton: true,
        confirmButtonText: 'Yes',
        allowOutsideClick: false,
        allowEscapeKey: false
    }).then((result) => {
        if (result.isConfirmed) {
            const { user, sessionId } = useStore.getState();
            useStore.setState({
                ...initialState, originalRegions: [],
                draft: [],
                user: user, sessionId: sessionId
            });
            seo({
                title: '',
                metaDescription: config.APP_TITLE
            });
            const element = document.getElementById("DrawingNo");
            window.setTimeout(() => element.focus(), 0);
        }
    })
}

export const seo = (data = {}) => {
    data.title = data.title || config.APP_TITLE;
    data.metaDescription = data.metaDescription || config.APP_TITLE;
    if (config.console)
    console.log(data)
    document.title = (data.title !== '' && data.title !== config.APP_TITLE ) ? `${data.title} | ${config.APP_TITLE}` : config.APP_TITLE;
   // document.querySelector('meta[name="description"]').setAttribute('content', data.metaDescription);
}

export const simulateMouseClick = (el) => {
    let opts = { view: window, bubbles: true, cancelable: true, buttons: 1 };
    el.dispatchEvent(new MouseEvent("mousedown", opts));
    el.dispatchEvent(new MouseEvent("mouseup", opts));
    el.dispatchEvent(new MouseEvent("click", opts));
}
export const nthElement = (arr, n = 0) =>
    (n === -1 ? arr.slice(n) : arr.slice(n, n + 1))[0];

// Data Table Start
export const DoublingEditor = React.memo(
    React.forwardRef((props, ref) => {
        const [value, setValue] = React.useState(props.value);
        const refInput = React.useRef(null);

        React.useEffect(() => {
            refInput.current.focus();
        }, []);

        /* Component Editor Lifecycle methods */
        React.useImperativeHandle(ref, () => {
            return {
                getValue() {
                    return value;
                },
                isCancelBeforeStart() {
                    return false;
                },
                isCancelAfterEnd() {
                    return false;
                },
            };
        });

        return (
            <input
                type="text"
                ref={refInput}
                value={value}
                onChange={(event) => setValue(event.target.value)}
                className="doubling-input"
            />
        );
    })
);

export const transactionData = () => {
    const props = useStore.getState();
    let originalRegions = props.originalRegions;
    let pageNo = 0;

    if (props.drawingDetails.length > 0 && props.ItemView != null) {
        pageNo = parseInt(Object.values(props.drawingDetails)[parseInt(props.ItemView)].currentPage);
    }

    const newrects = originalRegions.map((item) => {
        if (!item.hasOwnProperty("newarr")) {
            return false;
        } 
        if (item.hasOwnProperty("isDeleted") && item.isDeleted) {
            return false;
        }
        if (item.isballooned !== true) {
            //return false;
        }
        //console.log(item)
        if (item.Page_No === pageNo ) {
            const scaleX = props.bgImgW / props.imageWidth;
            const scaleY = props.bgImgH / props.imageHeight;
            let x = item.newarr.Crop_X_Axis * scaleX + props.bgImgX;
            let y = item.newarr.Crop_Y_Axis * scaleY + props.bgImgY;
            let w = item.newarr.Crop_Width * scaleX;
            let h = item.newarr.Crop_Height * scaleY;
            if (item.newarr.Crop_X_Axis === 0) { x = 28; w = item.newarr.Crop_Width; }
            if (item.newarr.Crop_Y_Axis === 0) { y = 28; h = item.newarr.Crop_Height; }
           // let cx = item.newarr.Circle_X_Axis * scaleX + props.bgImgX;
           // let cy = item.newarr.Circle_Y_Axis * scaleY + props.bgImgY;
            item.x = x;
            item.y = y;
            item.width = w;
            item.height = h;
            item.Crop_X_Axis = x;
            item.Crop_Y_Axis = y;
            item.Crop_Width = w;
            item.Crop_Height = h;
            if (item.newarr.MinusTolerance === "0" || item.newarr.MinusTolerance === "-0" ) {
                item.MinusTolerance = "-0"
            }
            if (item.newarr.PlusTolerance === "0" || item.newarr.PlusTolerance === "+0" ) {
                item.PlusTolerance = "+0"
            }
            
            //item.Circle_X_Axis = cx;
            //item.Circle_Y_Axis = cy;
            item.intBalloon = parseInt(item.Balloon);
            const isInteger = item.Balloon % 1 === 0;
            if (isInteger) {
                item.hypenBalloon = item.Balloon;
            } else {
                item.hypenBalloon = item.Balloon.replaceAll(".","-");
            }
            return item;
        }
        return false;
    }).filter(item => item !== false);
    
    let a = shortBalloon(newrects, "DrawLineID")
    //console.log(a)
    return a;
};

export const myCellRenderer = params => {
    return '';
};

/*
function onRowDragEnter(e) {
    console.log('onRowDragEnter', e);
    console.log('onRowDragEnter', e.overIndex, "overNode baloon", e.overNode.data.Balloon);
}

function onRowDragLeave(e) {
    console.log('onRowDragLeave', e);
}
*/
export const moveInArray = (arr, fromIndex, toIndex) => {
    if (toIndex === 0) {
      //  toIndex = 1;
    }
    var element = arr[fromIndex];
    arr.splice(fromIndex, 1);
    arr.splice(toIndex, 0, element);
}
function getGridBalloon(data) {
    let unique = data.map(d => d.intBalloon).filter(a => a !== '');
    unique = [...new Set(unique)];
    return unique;
}
function getMin(data) {
    return Math.min(...getGridBalloon(data));
}
function getMax(data) {
    return Math.max(...getGridBalloon(data));
}

function getdataList(pageData, Listballoon) {
    const resetOverData = [...pageData];

    let resetOvergroup = resetOverData.reduce((acc, obj) => {
        let key = obj.Balloon.toString().split('.')[0];
        acc[key] = acc[key] || [];
        acc[key].push(obj);
        return acc;
    }, {});
    let grouped = Object.values(resetOvergroup);
    let ui = 1;
    let resetOverSingle = resetOverData.reduce((res, item) => {
        if (!res[parseInt(item.Balloon)] && item.hasOwnProperty("subBalloon")) {
            let Balloon = ui.toString();
            ui++;
            res[parseInt(item.Balloon)] = { ...item, Balloon: Balloon, newarr: { ...item.newarr, Balloon: Balloon }, Old: parseInt(item.Balloon) };
        }
        return res;
    }, []).filter((a) => a);
    let unique = Object.values(resetOverSingle);
    let dragunique = shortBalloon(unique, "Balloon");
    let c = grouped.reduce((res, curr) => {
        if (!res[parseInt(curr[0].Balloon)] && curr[0].hasOwnProperty("subBalloon") && curr[0].subBalloon.length > 0 && curr[0].Quantity > 1) {
            res[parseInt(curr[0].Balloon)] = { key: parseInt(curr[0].Balloon), value: curr }
        }
        return res;
    }, []);
    let qtygroup = c.filter((a) => a);
    let newitems = [];
    let counter = dragunique.reduce((prev, curr,index) => {
        const id = uuid();
        let newarr = [];
        let Balloon = Listballoon.length + 1 + index;
        Balloon = Balloon.toString();
        if (curr.Quantity === 1 && curr.subBalloon.length === 0) {
            prev.push({ b: (Balloon), c: prev.length + 1 })
            let i = prev.length;
            newarr.push({ ...curr, newarr: { ...curr.newarr, Balloon: Balloon }, id: id, DrawLineID: i, Balloon: Balloon });
        }
        if (curr.Quantity === 1 && curr.subBalloon.length > 0) {
            let newsubItem = curr.subBalloon.filter(a => { return a.isDeleted === false; });
            let pb = parseInt(Balloon).toString();
            if (newsubItem.length > 0) {
                pb = parseInt(Balloon).toString() + ".1";
            }

            prev.push({ b: pb, c: prev.length + 1 })
            let i = prev.length;
            newarr.push({ ...curr, newarr: { ...curr.newarr, Balloon: pb }, id: id, DrawLineID: i, Balloon: pb });
            newsubItem.map(function (e, ei) {
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
            let line = qtygroup.filter(a => {
                if (parseInt(a.key) === parseInt(curr.Old)) { return a.value; }
                return false;
            }).filter(a => a);
            if (line.length > 0) {
                line[0].value.map(item => {
                    let key = item.Balloon.toString().split('.');
                    key[0] = Balloon.toString();
                    let pb = key.join('.');
                    if (config.console)
                        console.log(pb, parseInt(curr.Balloon), key, item)
                    prev.push({ b: pb, c: prev.length + 1 })
                    let i = prev.length
                    const qid = uuid();
                    newarr.push({ ...item, newarr: { ...item.newarr, Balloon: pb }, id: qid, DrawLineID: i, Balloon: pb });
                    return item;
                });
            }


        }

        newitems = newitems.slice();
        newitems.splice(newitems.length, 0, ...newarr);

        return prev;
    }, []);
    let prevOvergroup = newitems.reduce((acc, obj) => {
        let key = obj.Balloon.toString().split('.')[0];
        acc[key] = acc[key] || [];
        acc[key].push(obj);
        return acc;
    }, {});
    let prevgrouped = Object.values(prevOvergroup);

    return { items: [...newitems], group: [...prevgrouped], counter: [...counter] };
}

// Function to set the scroll position from outside the component

export const transactionDataColumns = (ref) => {

    let _window: Window = window;
    _window['agHandleDelClick'] = (event) => {
        event.preventDefault();
        event.stopPropagation();
        event.stopImmediatePropagation();
        const selectedData = ref.current.api.getSelectedRows();
        // delete balloon issue - durga
        // var deleteItem = selectedData.map(s => parseInt(s.Balloon)).reduce((resArr, currentArr) => {
        //     let other = resArr.some((ele) => currentArr === ele)
        //     if (!other) resArr.push(currentArr)
        //     return resArr
        // }, []);
         var deleteItem = selectedData.map(s => (s.Balloon.replaceAll(".", "-")))
        if (deleteItem.length === 0) return false;
        Swal.fire({
            title: `Are you want to delete Balloon (${deleteItem})?`,
            showCancelButton: true,
            confirmButtonText: 'Yes',
            allowOutsideClick: false,
            allowEscapeKey: false
        }).then((result) => {

            if (result.isConfirmed) {
               // const { drawingHeader, ItemView, drawingDetails } = useStore.getState();
                const { originalRegions } = useStore.getState();
 
                useStore.setState({ isLoading: true, loadingText: "Delete Balloon... Please Wait..." });
 
                // let newrects = originalRegions.filter((item) => !deleteItem.includes(parseInt(item.Balloon)) );
                let newrects = originalRegions.filter((item) => !deleteItem.includes(item.Balloon.replaceAll(".", "-")) );

                setTimeout(() => {
                    //const agOverData = JSON.parse(JSON.stringify(newrects));
                    const agOverData = [...newrects];
                    let agOverSingle = agOverData.reduce((res, item) => {
                        if (!res[parseInt(item.Balloon)]) {
                            res[parseInt(item.Balloon)] = item;
                        }
                        return res;
                    }, []);


                    let unique = Object.values(agOverSingle);
                    if (config.console)
                    console.log("td",unique)

                    //useStore.setState({ isLoading: false });
                    //return false;
                    let qtyi = 0;
                    // get all quantity parent
                    let Qtyparent = agOverData.reduce((res, item) => {
                        if (item.hasOwnProperty("subBalloon") && item.subBalloon.length >= 0 && item.Quantity > 1) {
                            res[qtyi] = item;
                            qtyi++;
                        }
                        return res;
                    }, []);
                    if (config.console)
                    console.log("td", Qtyparent, newrects)
                    
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
                            newarr.push({ ...curr, newarr: { ...curr.newarr, Balloon: pb }, id: id, DrawLineID: i, Balloon: pb });
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
                                const qid = uuid();
                                let pb = parseInt(curr.Balloon).toString() + "." + qi.toString();
                               

                                let newMainItem = Qtyparent.map(item => {
                                    if (pb === item.Balloon) {
                                        return item;
                                    }
                                    return false;
                                }).filter(x => x !== false);
                                if (newMainItem.length > 0) {
                                    let nmi = newMainItem[0];
                                    pb = parseInt(Balloon).toString() + "." + qi.toString();
                                    prev.push({ b: pb, c: prev.length + 1 })
                                    let i = prev.length;
                                   
                                    newarr.push({ ...nmi, newarr: { ...nmi.newarr, Balloon: pb }, id: qid, DrawLineID: i, Balloon: pb });
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
                                        let setter = { ...e, newarr: { ...e.newarr, Balloon: b }, id: sqid, DrawLineID: i, Balloon: b };
                                        newarr.push(setter);
                                        return e;
                                    })

                                }
                                if (config.console)
                                console.log("td",newMainItem,pb)
                                /*
                                newarr.push({ ...curr, newarr: { ...curr.newarr, Balloon: pb }, id: qid, DrawLineID: i, Balloon: pb });

                                curr.subBalloon.map(function (e, ei) {
                                    let sqno = ei + 1;
                                    const sqid = uuid();
                                    let b = pb + "." + sqno.toString();
                                    prev.push({ b: b, c: prev.length + 1 })
                                    let i = prev.length;
                                    if (e.hasOwnProperty("Isballooned"))
                                        delete e.Isballooned;
                                    if (e.hasOwnProperty("Id"))
                                        delete e.Id;
                                    let setter = { ...e, newarr: { ...e.newarr, Balloon: b }, id: sqid, DrawLineID: i, Balloon: b };
                                    newarr.push(setter);
                                    return e;
                                })
                                */
                            }

                        }

                        newitems = newitems.slice();
                        newitems.splice(newitems.length, 0, ...newarr);

                        return prev;
                    }, []);
                    if (config.console)
                    console.log("Table delete", newitems)
                    const newstate = useStore.getState();
                    let newrect = newBalloonPosition(newrects, newstate);         //newitem - newrects
                    useStore.setState({
                        originalRegions: newrects,                         //newitem - newrects
                        draft: newrects,                                    //newitem - newrects
                        drawingRegions: newrect,
                        balloonRegions: newrect
                    }); 
                    setTimeout(() => {
                        useStore.setState({ ItemView: null });
                    }
                        , 200);
                    setTimeout(() => {
                        useStore.setState({ ItemView: newstate.ItemView });
                    }
                        , 200);
                    useStore.setState({ isLoading: false });

                }, 300);
               
            }
        });
        return false;

    };
    return [
        {
            field: 'Balloon',
            headerName: "Balloon No",
            checkboxSelection: true,
           // suppressMovable: true,
            cellRenderer: myCellRenderer,
            headerCheckboxSelection: true,
            headerComponentParams: {
                template:
                    '<div>' +

                    '<button onclick="agHandleDelClick(event)" ref="eCustomButton" class="light-btn btn buttons primary primary_hover ">' +
                    `<img src="${del}" alt="Delete Selected" class="icon">` +
                    '</button> ' +
                    '</div>'
            },
            maxWidth: 120,
         //   lockPosition: 'left',
            resizable: false,
            rowDrag: true
        },

        {
            field: 'hypenBalloon',
            headerName: 'Balloon No',
            cellClass: ["ag-cell--normal"],
            //   suppressMovable: true,
            maxWidth: 200,

            editable: (params) => {
                const isInteger = params.data.Balloon % 1 === 0;
                //console.log(isInteger, params.data.Balloon);
                return isInteger;
            },
            lockPosition: 'right',
            valueSetter: (params: ValueSetterParams) => {
                //console.log('valueSetter: ', params);
                let allBalloon = getGridBalloon(transactionData());
                let getMinv = getMin(transactionData());
                let getMaxv = getMax(transactionData());
                var newValInt = parseInt(params.newValue);
                var oldValInt = parseInt(params.oldValue);
                if (config.console)
                    console.log(getMinv, getMaxv)
               
                if (newValInt && allBalloon.includes(newValInt) && oldValInt !== newValInt) {
                    Swal.fire({
                        title: "Are you sure?",
                        html: `To move the Balloon from ${oldValInt} to ${newValInt}`,
                        icon: "",
                        showCancelButton: true,
                        confirmButtonText: "OK",
                    }).then((r) => {
                        if (r.isConfirmed) {
                            let immutableStore = transactionData();
                            let sortable = params.api.rowModel.rootNode.allLeafChildren.map(a => a.data);
                            var movingData = params.node.data;
                            var overTemp = sortable.filter((item) => { return parseInt(item.Balloon) === newValInt; });
                            var overData = Object.values(overTemp)[0];
                            if (oldValInt > newValInt) {
                                overData = Object.values(overTemp)[0];
                            } else {
                                overData = Object.values(overTemp)[overTemp.length - 1];
                            }
                            
                            var fromIndex = immutableStore.indexOf(movingData);
                            var toIndex = immutableStore.indexOf(overData);
                            var newStore = immutableStore.slice();
                            
                            moveInArray(newStore, fromIndex, toIndex);
                            useStore.setState({ isLoading: true, loadingText: "Saving Balloons... Please Wait..." });
                            setTimeout(() => {

                                const { ItemView, drawingDetails, originalRegions } = useStore.getState();

                                let pageNo = 0;


                                if (drawingDetails.length > 0 && ItemView != null) {
                                    pageNo = Object.values(drawingDetails)[parseInt(ItemView)].currentPage;
                                }

                                let prevPageData = originalRegions.map((item) => {
                                    if (parseInt(item.Page_No) < parseInt(pageNo)) {
                                        return item;
                                    }
                                    return false;
                                }).filter(item => item !== false);

                                let nextPageData = originalRegions.map((item) => {
                                    if (parseInt(item.Page_No) > parseInt(pageNo)) {
                                        return item;
                                    }
                                    return false;
                                }).filter(item => item !== false);

                                let p = getdataList(prevPageData, []);
                                //console.log(p)
                                let prevgrouped = p.group;
                                //const resetOverData = JSON.parse(JSON.stringify(newStore));
                                const resetOverData = [...newStore];

                                let resetOvergroup = resetOverData.reduce((acc, obj) => {
                                    let key = obj.Balloon.toString().split('.')[0];
                                    acc[key] = acc[key] || [];
                                    acc[key].push(obj);
                                    return acc;
                                }, {});
                                let grouped = Object.values(resetOvergroup);
                                let uii = 1;
                                let OverSingle = grouped.reduce((res, curr) => {
                                    if (!res[parseInt(uii)] && curr[0].hasOwnProperty("subBalloon")) {
                                        res[parseInt(uii)] = { uii: uii, key: parseInt(curr[0].Balloon), value: curr }
                                        uii++;
                                    }
                                    return res;
                                }, []).filter((a) => a);

                                let ui = 1;
                                let resetOverSingle = resetOverData.reduce((res, item) => {
                                    if (!res[parseInt(item.Balloon)] && item.hasOwnProperty("subBalloon")) {
                                        let Balloon = ui.toString();
                                        ui++;
                                        res[parseInt(item.Balloon)] = { ...item, Balloon: Balloon, newarr: { ...item.newarr, Balloon: Balloon }, Old: parseInt(item.Balloon) };
                                    }
                                    return res;
                                }, []).filter((a) => a);


                                let unique = Object.values(resetOverSingle);
                                let dragunique = shortBalloon(unique, "Balloon");
                                if (config.console)
                                    console.log(dragunique)

                                let c = grouped.reduce((res, curr) => {
                                    if (!res[parseInt(curr[0].Balloon)] && curr[0].hasOwnProperty("subBalloon") && curr[0].subBalloon.length > 0 && curr[0].Quantity > 1) {
                                        res[parseInt(curr[0].Balloon)] = { key: parseInt(curr[0].Balloon), value: curr }
                                    }
                                    return res;
                                }, []);
                                let qtygroup = c.filter((a) => a);
                                if (config.console)
                                    console.log(resetOverSingle, OverSingle, unique, dragunique, qtygroup)
                                //useStore.setState({ isLoading: false });
                                //return false;
                                let newitems = [];
                                //15�
                             
                                dragunique.reduce((prev, curr,index) => {
                                    const id = uuid();
                                    let newarr = [];
                                    let Balloon = prevgrouped.length + 1 + index;
                                    Balloon = Balloon.toString();
                                    if (curr.Quantity === 1 && curr.subBalloon.length === 0) {
                                        prev.push({ b: (Balloon), c: prev.length + 1 })
                                        let i = prev.length;
                                        newarr.push({ ...curr, newarr: { ...curr.newarr, Balloon: Balloon }, id: id, DrawLineID: i, Balloon: Balloon });
                                    }
                                    if (curr.Quantity === 1 && curr.subBalloon.length > 0) {
                                        let newsubItem = curr.subBalloon.filter(a => { return a.isDeleted === false; });
                                        let pb = parseInt(Balloon).toString();
                                        if (newsubItem.length > 0) {
                                            pb = parseInt(Balloon).toString() + ".1";
                                        }
                                       // console.log(curr, pb, newsubItem)
                                        prev.push({ b: pb, c: prev.length + 1 })
                                        let i = prev.length;
                                        newarr.push({ ...curr, newarr: { ...curr.newarr, Balloon: pb }, id: id, DrawLineID: i, Balloon: pb });
                                        newsubItem.map(function (e, ei) {
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
                                        let line = qtygroup.filter(a => {
                                            if (parseInt(a.key) === parseInt(curr.Old)) { return a.value; }
                                            return false;
                                        }).filter(a => a);
                                        if (line.length > 0) {
                                            line[0].value.map(item => {
                                                let key = item.Balloon.toString().split('.');
                                                key[0] = Balloon.toString();
                                                let pb = key.join('.');
                                                if (config.console)
                                                    console.log(pb, parseInt(curr.Balloon), key, item)
                                                prev.push({ b: pb, c: prev.length + 1 })
                                                let i = prev.length
                                                const qid = uuid();
                                                newarr.push({ ...item, newarr: { ...item.newarr, Balloon: pb }, id: qid, DrawLineID: i, Balloon: pb });
                                                return item;
                                            });
                                        }


                                    }

                                    newitems = newitems.slice();
                                    newitems.splice(newitems.length, 0, ...newarr);

                                    return prev;
                                }, []);
                                if (config.console)
                                    console.log(newitems)
                                let prevOvergroup = newitems.reduce((acc, obj) => {
                                    let key = obj.Balloon.toString().split('.')[0];
                                    acc[key] = acc[key] || [];
                                    acc[key].push(obj);
                                    return acc;
                                }, {});
                                let prevovergrouped = Object.values(prevOvergroup);

                                let ne = getdataList(nextPageData, [...p.group,...prevovergrouped]);

                                //console.log([...prevovergrouped, ...p.group])
                                newitems = [...p.items, ...newitems, ...ne.items];
                                //console.log(newitems)
                               // useStore.setState({ isLoading: false });
                               // return;
                               
                                let currentPageData = newitems.map((item) => {
                                    if (parseInt(item.Page_No) === parseInt(pageNo)) {
                                        return item;
                                    }
                                    return false;
                                }).filter(item => item !== false);
                                params.api.setRowData(currentPageData);
                                params.api.clearFocusedCell();
                                useStore.setState({
                                    originalRegions: newitems,
                                    savedDetails: ((newitems.length > 0) ? true : false),
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
                                setTimeout(() => { useStore.setState({ ItemView: null }); }, 1);
                                setTimeout(() => { useStore.setState({ ItemView: newstate.ItemView }); }, 2);
                                useStore.setState({ isLoading: false });
                            }, 300);
                        }

                    });
                    return newValInt;
                } else {
                    return oldValInt;
                }
                
            }
        },
       // { field: 'hypenBalloon', headerName: 'Specification', editable: false,  },
        
        {
            field: 'Spec',
            headerName: 'Specification',
            editable: false,
        //    suppressMovable: true,
            cellClass: ["ag-cell--math"],
          //  cellStyle: { 'text-overflow': 'ellipsis', 'white-space': 'nowrap', 'overflow': 'hidden', 'padding': 0 },
            lockPosition: 'right',
            cellEditor: DoublingEditor
        },
        {
            field: 'ToleranceType',
            headerName: 'Tolerance Type',
            editable: false,
         //   suppressMovable: true,
            cellClass: ["ag-cell--math"],
            //  cellStyle: { 'text-overflow': 'ellipsis', 'white-space': 'nowrap', 'overflow': 'hidden', 'padding': 0 },
            lockPosition: 'right',

        },
        {
            field: 'Minimum',
            headerName: 'Min Value',
            editable: false,
        //    suppressMovable: true,
            cellClass: ["ag-cell--math"],
            //  cellStyle: { 'text-overflow': 'ellipsis', 'white-space': 'nowrap', 'overflow': 'hidden', 'padding': 0 },
            lockPosition: 'right',
  
        },
        
        {
            field: 'Maximum',
            headerName: 'Max Value',
            editable: false,
        //    suppressMovable: true,
            cellClass: ["ag-cell--math"],
            //  cellStyle: { 'text-overflow': 'ellipsis', 'white-space': 'nowrap', 'overflow': 'hidden', 'padding': 0 },
            lockPosition: 'right',

        }
        
    ];
};

export const Table: React.FunctionComponent = (props) => {
   
   // let state = useStore.getState();
    const gridRef = React.useRef(null);
    const defaultColDef = React.useMemo(() => {
        return {
            editable: false,
            sortable: false,
            resizable: true,
            filter: false,
            flex: 1,
            minWidth: 100,
        };
    }, []);
    const DeSelectDataGrid = () => {
        gridRef.current.api.deselectAll();
        useStore.setState({ selectedBalloon: null });
    }
 
    const onGridReady = React.useCallback((params) => {
        params.api.setRowData(transactionData());
    }, []);
 
     
    /*
    const onRowDragMove = (e) => {
        if (config.console)
            console.log('onRowDragMove', e);
    }
    */
    const onRowDragEnd = (e) => {
        if (config.console)
            console.log('onRowDragEnd', e);
        let immutableStore = transactionData();
        let movingNodes = e.nodes.map((e) => e.data);
        let sortable = e.api.rowModel.rootNode.allLeafChildren.map(a => a.data);
        var movingNode = e.node;
        var overNode = e.overNode;
        let compare = [];
        let error = [];
        let reqTotal = 0;
        if (config.console)
        console.log('onRowDragEnd', e.api.rowModel.rootNode);
        movingNodes.map((a, i) => {
            const isInteger = a.Balloon % 1 === 0;
            if (!isInteger) {
                compare[parseInt(a.Balloon)] = immutableStore.filter(word => word.Balloon.includes(parseInt(a.Balloon).toString() + "."));
            } else {
                compare[parseInt(a.Balloon)] = [a.Balloon];
            }
            return a;
        });
        compare = compare.filter(function (el) { return el != null; });
        compare.forEach((el) => { reqTotal = reqTotal + el.length; });

        if (movingNodes.length !== reqTotal && e.overIndex !== -1) {
            var movingData = movingNode.data;
            var overData = overNode.data;
            var fromIndex = immutableStore.indexOf(movingData);
            var toIndex = immutableStore.indexOf(overData);
            var newStore = immutableStore.slice();
            moveInArray(newStore, fromIndex, toIndex);
            if (config.console)
                console.log("error group item.")
            error[0] = 'error group item';
            Swal.fire({
                title: "Error",
                html: "You can't move the group items seperately.",
                icon: "",
                confirmButtonText: "OK",
            }).then((r) => {
                if (r.isConfirmed) {
                    e.api.setRowData(immutableStore);
                    e.api.clearFocusedCell();
                }

            });
        }

        let prev, next;
        let diff = [];
        prev = e.nodes[0].rowIndex - 1;
        next = e.nodes[0].rowIndex + movingNodes.length;

        gridRef.current.api.forEachNode((node) => {
            if (node.rowIndex === prev) {
                diff.push(node.data.intBalloon);
                if (config.console)
                console.log("prev " + prev, node.data, node.rowIndex, e, e.nodes[0].rowIndex)
            }
            if (node.rowIndex === next) {
                diff.push(node.data.intBalloon);
                 if (config.console)
                console.log("next " + next, node.data, node.rowIndex, e, e.nodes[0].rowIndex)
            }
        });

        let variable = diff.reduce((resArr, currentArr) => {
            let other = resArr.some((ele) => currentArr === ele)
            if (!other) resArr.push(currentArr)
            return resArr
        }, []);
        if (config.console)
        console.log(prev, next, diff, variable)
        if (prev !== -1 && next !== sortable.length) {

            if (variable.length !== 2) {
            if (config.console)
                console.log("error moving between group item.")
            error[0] = 'error moving between group item.';
            Swal.fire({
                title: "Error",
                html: "You can't move the Balloon in between group item.",
                icon: "",
                confirmButtonText: "OK",
            }).then((r) => {
                if (r.isConfirmed) {
                    e.api.setRowData(immutableStore);
                    e.api.clearFocusedCell();
                }

            });

            }
        }

        
        if (error.length === 0) {
            if (JSON.stringify(immutableStore) !== JSON.stringify(sortable)) {
                if (config.console)
                    console.log("save", immutableStore, sortable)

                const { ItemView, drawingDetails, originalRegions } = useStore.getState();

                let pageNo = 0;


                if (drawingDetails.length > 0 && ItemView != null) {
                    pageNo = Object.values(drawingDetails)[parseInt(ItemView)].currentPage;
                }
                /***
                 * 
                 */

                let prevPageData = originalRegions.map((item) => {
                    if (parseInt(item.Page_No) < parseInt(pageNo)) {
                        return item;
                    }
                    return false;
                }).filter(item => item !== false);

                let nextPageData = originalRegions.map((item) => {
                    if (parseInt(item.Page_No) > parseInt(pageNo)) {
                        return item;
                    }
                    return false;
                }).filter(item => item !== false);
                let p = getdataList(prevPageData, []);
                //console.log(p)
                let prevgrouped = p.group;
                /* 
                * 
                */
                // const resetOverData = JSON.parse(JSON.stringify(sortable));
                const resetOverData = [...sortable];

                let ui = 1;
                let resetOverSingle = resetOverData.reduce((res, item) => {
                    if (!res[parseInt(item.Balloon)] && item.hasOwnProperty("subBalloon")) {
                        let Balloon = ui.toString();
                        ui++;
                        res[parseInt(item.Balloon)] = { ...item, Balloon: Balloon, newarr: { ...item.newarr, Balloon: Balloon }, Old: parseInt(item.Balloon) };
                    }
                    return res;
                }, []);
                let resetOvergroup = resetOverData.reduce((acc, obj) => {
                    let key = obj.Balloon.toString().split('.')[0];
                    acc[key] = acc[key] || [];
                    acc[key].push(obj);
                    return acc;
                }, {});

                let unique = Object.values(resetOverSingle);
                let grouped = Object.values(resetOvergroup);
                let dragunique = shortBalloon(unique, "Balloon");
                if (config.console)
                console.log( dragunique)

                let c = grouped.reduce((res, curr) => {
                    if (!res[parseInt(curr[0].Balloon)] && curr[0].hasOwnProperty("subBalloon") && curr[0].subBalloon.length > 0 && curr[0].Quantity > 1) {
                        res[parseInt(curr[0].Balloon)] = { key: parseInt(curr[0].Balloon), value: curr }
                    }
                    return res;
                }, []);
                let qtygroup = c.filter((a) => a);


                let newitems = [];

                dragunique.reduce((prev, curr,index) => {
                    const id = uuid();
                    //console.log(curr, index)
                    let newarr = [];
                    let Balloon = prevgrouped.length + 1 + index;
                    Balloon = Balloon.toString();
                    if (curr.Quantity === 1 && curr.subBalloon.length === 0) {
                        prev.push({ b: (Balloon), c: prev.length + 1 })
                        let i = prev.length;
                        newarr.push({ ...curr, newarr: { ...curr.newarr, Balloon: Balloon }, id: id, DrawLineID: i, Balloon: Balloon });
                    }
                    if (curr.Quantity === 1 && curr.subBalloon.length > 0) {
                        let newSubItem = curr.subBalloon.filter(a => { return a.isDeleted === false; });
                        let pb = parseInt(Balloon).toString();
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
                        //for (let qi = 1; qi <= curr.Quantity; qi++) {
                        // const qid = uuid();
                        //let pb = parseInt(curr.Balloon).toString() + "." + qi.toString();
                        let line = qtygroup.filter(a => {
                            if (parseInt(a.key) === parseInt(curr.Old)) { return a.value; }
                            return false;
                        }).filter(a=>a);
                        if (line.length > 0) {
                            line[0].value.map(item => {
                                let key = item.Balloon.toString().split('.');
                                key[0] = Balloon.toString();
                                let pb = key.join('.');
                                if (config.console)
                                console.log(pb, parseInt(curr.Balloon), key, item)
                                prev.push({ b: pb, c: prev.length + 1 })
                                let i = prev.length
                                const qid = uuid();
                                newarr.push({ ...item, newarr: { ...item.newarr, Balloon: pb }, id: qid, DrawLineID: i, Balloon: pb });
                                return item;
                            });
                        }


                        /*

                        newarr.push({ ...curr, newarr: { ...curr.newarr, Balloon: pb }, id: qid, DrawLineID: i, Balloon: pb });

                        curr.subBalloon.filter(a => { return a.isDeleted === false; }).map(function (e, ei) {
                            let sqno = ei + 1;
                            const sqid = uuid();
                            let b = pb + "." + sqno.toString();
                            prev.push({ b: b, c: prev.length + 1 })
                            let i = prev.length;
                            if (e.hasOwnProperty("Isballooned"))
                                delete e.Isballooned;
                            if (e.hasOwnProperty("Id"))
                                delete e.Id;
                            let setter = { ...e, newarr: { ...e.newarr, Balloon: b }, id: sqid, DrawLineID: i, Balloon: b };
                            newarr.push(setter);
                            return e;
                        })
                        */
                        //}

                    }

                    newitems = newitems.slice();
                    newitems.splice(newitems.length, 0, ...newarr);

                    return prev;
                }, []);

                if (config.console)
                    console.log("new", newitems)
                //useStore.setState({ isLoading: false });
                // return;
                let prevOvergroup = newitems.reduce((acc, obj) => {
                    let key = obj.Balloon.toString().split('.')[0];
                    acc[key] = acc[key] || [];
                    acc[key].push(obj);
                    return acc;
                }, {});
                let prevovergrouped = Object.values(prevOvergroup);

                let ne = getdataList(nextPageData, [...p.group, ...prevovergrouped]);

                //console.log([...prevovergrouped, ...p.group])
                newitems = [...p.items, ...newitems, ...ne.items];
                let currentPageData = newitems.map((item) => {
                    if (parseInt(item.Page_No) === parseInt(pageNo)) {
                        return item;
                    }
                    return false;
                }).filter(item => item !== false);


                useStore.setState({ isLoading: true, loadingText: "Saving Balloons... Please Wait..." });
                setTimeout(() => {
                    e.api.setRowData(currentPageData);
                    e.api.clearFocusedCell();
                    useStore.setState({
                        originalRegions: newitems,
                        savedDetails: ((newitems.length > 0) ? true : false),
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
                    useStore.setState({ isLoading: false });
                }, 300);
                
            }
        }
    }


    const onSelectionChanged = React.useCallback((params) => {
        const selectedRows = params.api.getSelectedRows();
        useStore.setState({
            selectedRowIndex: null
        });
        if (config.console)
            console.log(selectedRows)
    })
    /*
    const onSelectionChanged = React.useCallback(() => {
        //let state = useStore.getState();
        const selectedRows = gridRef.current.api.getSelectedRows();
       //console.log(selectedRows)
       // console.log(selectedRows, state.immutableStore)
       /*
        let immutableStore = transactionData();
        let createshape = immutableStore.map(a => a.Balloon);
        let movingNodes = selectedRows.map(a => a.Balloon);
        
        let compare = [];
        movingNodes.map((a, i) => {
            const isInteger = a % 1 === 0;
            if (!isInteger) {
                compare = createshape.filter(word => word.includes(parseInt(a).toString() + "."));
            } else {
                compare = [a];
            }
           // console.log(compare)
           
            gridRef.current.api.forEachNode((node) => {
                compare.forEach((el) => {
                    if (el === node.data.Balloon) {
                       // console.log(node.data.Balloon, node.rowIndex, el)
                        gridRef.current.api.getRowNode(node.rowIndex).setSelected(true);
                    }
                });
               
            });
            const selectedRows1 = gridRef.current.api.getSelectedRows();
            console.log(selectedRows1)
            useStore.setState({
                immutableStore: selectedRows1.map(a => a.Balloon)
            });
            
        })
        /
        
    }, []);
    */

    let colElements = transactionDataColumns(gridRef);
 
    const rowData = transactionData();
    //const gridStyle = React.useMemo(() => ({ height: '100%', width: '100%' }), []);
    const containerStyle = React.useMemo(() => ({ width: '100%', height: '100%' }), []);
    setTimeout(() => {
        if (props.selectedRowIndex !== null && gridRef.current && gridRef.current.api) {
            const gridApi = gridRef.current.api;
            gridApi.forEachNode((node) => {
                if (node.data.Balloon === props.selectedRowIndex) {
                    gridApi.ensureIndexVisible(node.rowIndex, 'top');
                    //console.log("rowData", props.selectedRowIndex)
                    gridApi.deselectAll();
                    gridApi.getRowNode(node.rowIndex).setSelected(true);
                }
            })
        }
    }, 10);

  
    return (
        <>
            {(rowData.length > 0) && (
                <>
                    <button className="d-none" onClick={DeSelectDataGrid}>Clear</button>

                    <div style={containerStyle}>
                    <div
                            className="ag-theme-alpine"
                            style={{
                                height: 500+ "px", width:"100%"
                            }}
                            
                    >
                            <AgGridReact
                                id="myAgGrid"
                                ref={gridRef}
                                rowData={rowData}
                                onGridReady={onGridReady}
                                // immutableData={true}
                                columnDefs={colElements}
                                defaultColDef={defaultColDef}
                                rowSelection={'multiple'}
                                rowDragManaged={true}
                                // rowDragEntireRow={true}
                                animateRows={true}
                                //  onRowDragEnter={onRowDragEnter}
                                onRowDragEnd={onRowDragEnd}
                                // onRowDragMove={onRowDragMove}
                                //  onRowDragLeave={onRowDragLeave}
                                rowDragMultiRow={true}
                                rowDeselection={true}
                                //onFirstDataRendered={onFirstDataRendered }
                                //  groupDefaultExpanded={false}
                                onSelectionChanged={onSelectionChanged}
                                onRowDoubleClicked={(e) => {
                                   // e.preventDefault();
                                    const cell = gridRef.current.api.getEditingCells();
                                   // console.log("handleCellDoubleClicked", cell)
                                    if (cell.length === 0) {
                                        useStore.setState({ selectedBalloon: e.data.Balloon });
                                    } else {
                                        if (cell[0].column.colDef.field === 'Balloon') {
                                            e.event.stopPropagation();
                                            useStore.setState({ selectedBalloon: null });
                                        }
                                    }
                              //  useStore.setState({ selectedBalloon: e.data.Balloon });
                                 }}
                            pagination={false}
                        >
                        </AgGridReact>
                    </div>
                    </div>
                </>
            )}
        </>
    );
};
// Data Table end
