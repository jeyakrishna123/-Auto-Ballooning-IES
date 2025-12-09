import React, { useState, useEffect } from "react";
import { Layer, Stage, Image, Group, Circle } from "react-konva";
import { selectedSPLRegionProcess, config, Table, originalPosition, ballonOriginalPosition, fitSize, actualSize, selectedRegionProcess, newBalloonPosition } from "../Common/Common";
import { isProduction, forceStageElementRedraw, productionSafeTimeout } from "../Common/ProductionUtils";
import { v1 as uuid } from "uuid";
import Annotation from "./Annotation";
import PopupModal from "../Common/Modal";
import useStore from "../Store/store";
import initialState from "../Store/init";
import Swal from 'sweetalert2'
import { Button, Nav, NavItem } from "reactstrap";

 
function removeA(arr) {
    var what, a = arguments, L = a.length, ax;
    while (L > 1 && arr.length) {
        what = a[--L];
        while ((ax = arr.indexOf(what)) !== -1) {
            arr.splice(ax, 1);
        }
    }
    return arr;
}



class PatternImage extends React.Component {

    constructor(props) {
        super(props);

        this.state = {
 
            image: null,
        };

      }
    componentDidMount() {
        this.loadImage();
    }
    componentDidUpdate(oldProps) {
        if (oldProps.src !== this.props.src) {
            this.loadImage();
        }
    }
    componentWillUnmount() {
        this.image.removeEventListener('load', this.handleLoad);
 
    }
    loadImage() {
        this.image = new window.Image();
        this.image.src = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAMAAAC6V+0/AAAABlBMVEUAAADY2NjnFMi2AAAAAXRSTlMAQObYZgAAABVJREFUGNNjYIQDBgQY0oLDxBsIQQCltADJNa/7sQAAAABJRU5ErkJggg==";
        this.image.addEventListener('load', this.handleLoad);
    }
    handleLoad = () => {
        this.setState({
            image: this.image,
        });
    };
    render() {
        return (
            <Image
                x={this.props.x}
                y={this.props.y}
                width={this.props.width}
                height={this.props.height}
                fillPatternImage={this.state.image}
                ref={(node) => {
                    this.imageNode = node;
                }}
            />
        );
    }
}


class URLImage extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            image: null,
            err_img:null,
            error:[],
        };
    }
  

    componentDidMount() {
        this.loadImage();
    }
    componentDidUpdate(oldProps) {
        //let state = useStore.getState();
        //console.log(state.ItemView,oldProps.src, this.props.src)
        if (oldProps.src !== this.props.src) {
            this.loadImage();
        }
    }
    componentWillUnmount() {
        this.image.removeEventListener('load', this.handleLoad);
        const state = useStore.getState();
        if (state.isErrImage) {
            this.err_img.removeEventListener('load', () => { console.log("error") });
        }
    }
    fetchConfig = async () => {
        const response = await fetch('/config.json');
        const settings = await response.json();
        useStore.setState({
            AppSettings: settings
        });

    };
    loadImage() {
        this.fetchConfig();
        const loadingstate = useStore.getState();
        
        useStore.setState({ win: initialState.win, isErrImage : false });
        this.setState({ error: [] });
        new Promise((resolve, reject) => {

            const baseURL = (loadingstate?.AppSettings?.REACT_APP_SERVER !== null) ? loadingstate?.AppSettings?.REACT_APP_SERVER : process.env.REACT_APP_SERVER;
            this.image = new window.Image();
            // this.image.addEventListener('load', this.handleLoad);
            let str = this.props.src;
            let xx = "";
            console.log(" baseURL ", baseURL ,"config env", config.ENVIRONMENT, "config",config)
            try {
                if (config.ENVIRONMENT === "production") {
                    console.log("build env production ", str, "xx => " + xx, baseURL);
                    
                    const { sessionId } = useStore.getState();
                     const uri = `/StaticFiles/src/drawing/${sessionId}/${str}`;
                    xx = baseURL + uri + "?a=" + Math.random();
                    console.log("build production ", str, "xx => " + xx, uri, baseURL, sessionId);
                    
                } else {
                    //console.log(" before ", str, "xx => " + xx, process.env)
                    xx = require(`./../../drawing/${str}`);
                    //console.log(" middle ", str, "xx => " + xx, this.props)
                    console.log("build development ", str, "xx => " + xx);
                    
                }

            } catch (e) {
                console.log("build catch error", e);
                
                // const { drawingDetails } = useStore.getState();
               // console.log("Image is Too Large / Damaged." + e)
            }
            console.log("build loadImage", str, "xx => " + xx, baseURL);
            
            this.image.src = xx;
            console.log("build loadImage", "xx => ",   this.image.src);
            
 
            //console.log("final", str, "xx => " + xx, `http://localhost:44436/drawing/${str}`)

            this.image.onload = () => {
                /* console.log("on loadImage")
                
                const state = useStore.getState();
                let iw = this.image.width;
                let ih = this.image.height;
                var minusWidth = 0;

                if (ih > state.win.height) {
                    ih = state.win.height;
                    minusWidth = iw * (state.win.height / parseFloat(ih.toString()));
                    iw = minusWidth;
                }

                // Rescale asset if the with is now bigger than the with of the canvas
                // Keep the earlier scaled values in mind :)
                if (iw > state.win.width) {
                    iw = state.win.width - minusWidth;
                    ih = ih * ((state.win.width - minusWidth) / parseFloat(iw.toString()));
                }
                

                var isValidCanvasSize = canvasSize.test({
                    height: this.image.height,
                    width: this.image.width
                });
                */
                let height = this.image.height;
                let width = this.image.width;
                var maxsize = (width > height) ? width : height;
                let timer = (maxsize/100);
                //console.log(maxsize, timer); 
                console.log("build onload loadImage", "xx => ", xx, "width", width, "height", height, "timer", timer);
                

                    setTimeout(() => {
                        resolve(xx)
                        //console.log(this.image.width, this.image.height)
                        useStore.setState({ bgImgRotation: 0, imageWidth: this.image.width, imageHeight: this.image.height });
                        const state = useStore.getState();
                        if (state.zoomed) {
                            console.log("build onload loadImage zoomed if", state.zoomed, state.originalRegions, state);
                            
                            useStore.setState({ win: loadingstate.win });
                           // console.log(state.history, loadingstate.win, state.win);
                            let newrect = newBalloonPosition(state.originalRegions, state);
                            useStore.setState({
                                savedDetails: false,
                                drawingRegions: newrect,
                                balloonRegions: newrect,
                                //    isDisabledAutoB: true

                            });

                        } else {
                            console.log("build onload loadImage zoomed else", state.zoomed, state.originalRegions, state);
                            
                            useStore.setState({
                                history: []
                            });
                            if (state.fitscreen) {
                                fitSize();
                            } else {
                                actualSize();
                            }
                        }
                       

                    }, timer)

            };


            this.image.onerror = err => {
              //  console.log("on error", err)
                this.setState({ error: [err] });
                
                this.err_img = new window.Image();
                let xx = require(`./../../assets/error.png`);
                this.err_img.src = xx;
console.log("build loadImage error", xx, "=>>>." ,this.err_img.src);

                let state = useStore.getState();
               
                var w = this.err_img.width;
                var h = this.err_img.width;
                 
  
                let x = (state.win.width - 700) / 2;
                let y = (state.win.height - 400) / 2;

                useStore.setState({ isErrImage:true, isLoading: false, history: [], bgImgRotation: 0, bgImgW: 700, bgImgH: 400, bgImgX: x, bgImgY: y , imageWidth: w, imageHeight: h });
                this.err_img.addEventListener("load", () => {
                    this.setState({
                        err_img: this.err_img,
                    });
                    let scrollElement = document.querySelector('#konvaMain');
                    scrollElement.scrollLeft = (scrollElement.scrollWidth - scrollElement.clientWidth) / 2;
                })
               // reject(err);
                }
                
            });

        this.image.addEventListener('load', this.handleLoad);
    }
    handleLoad = () => {
      console.log("build on loadImage handle")
 
        setTimeout(() => {
            const state = useStore.getState();
            this.setState({
                image: this.image,
            });
            useStore.setState({ isLoading: false })

            if (state.savedDetails) {
                let originalRegions = state.originalRegions;
                // console.log("originalRegions",originalRegions)
                let newrect = newBalloonPosition(originalRegions, state);
                useStore.setState({
                    savedDetails: false,
                    drawingRegions: newrect,
                    balloonRegions: newrect,
                    //    isDisabledAutoB: true

                });
            }

            let scrollElement = document.querySelector('#konvaMain');
            scrollElement.scrollLeft = (scrollElement.scrollWidth - scrollElement.clientWidth) / 2;
        }, 500);

        const dstate = useStore.getState();
        if (dstate.scrollPosition !== 0) {
            setTimeout(function () {
                let scrollElement = document.querySelector('#konvaMain');
                if (scrollElement !== null) {
                    scrollElement.scrollLeft = dstate.scrollPosition;
                    scrollElement.scrollTop = dstate.konvaPositionTop;
                    // console.log(dstate)
                }
                document.body.scrollTop = dstate.documentPositionTop

            }, 500);

        }

    };
    render() {
        let state = useStore.getState();
 
        console.log("build on render", state )
        return (
            <>
                {this.state.error.length > 0 && (
                    <Image
                        id="error"
                        name="error"
                        className="error"
                        image={this.state.err_img}
                        x={state.bgImgX}
                        y={state.bgImgY}
                        width={state.bgImgW}
                        height={state.bgImgH}
                        onMouseDown={(e) => {
                            e.evt.preventDefault();
                            return false;
                        }}
                        onMouseMove={(e) => {
                            e.evt.preventDefault();
                            return false;
                        }}
                        onMouseUp={(e) => {
                           e.evt.preventDefault();
                            return false;
                        }}
                        ref={(node) => {
                            this.imageNode = node;
                        }}
                        listening={false}
                    />
                )}
                {this.state.error.length === 0 && (
                    <Image
                        id="product-img"
                            name="product"
                        className="product"
                        x={state.bgImgX}
                        y={state.bgImgY}
                        width={state.bgImgW}
                            height={state.bgImgH}
                          // rotation={state.bgImgRotation}
                        image={this.state.image}
                        ref={(node) => {
                            this.imageNode = node;
                        }}
                    
                        onMouseDown={this.props.onMouseDown}
                    />
                )}
            </>
        );
    }
}

export default function Canvas() {
    const stageRef = React.useRef();
    const groupRef = React.useRef();

    const [newAnnotation, setNewAnnotation] = useState([]);
    const [newRegion, setNewRegion] = useState([]);
    const [selectedId, selectAnnotation] = useState(null);
    const props = useStore.getState();

    const myElementRef = React.useRef(null);
    const mypopup = React.useRef(null);
    const [positionLeft, setPositionLeft] = useState(0);
    const [positionWidth, setPositionWidth] = useState(0);
    const [positionTop, setPositionTop] = useState(0);

    useEffect(() => {
        var menuNode = document.getElementsByClassName('contextmenu');
        var popupNode = document.getElementsByClassName('popup');
 
        const handleScroll = () => {
            const el = myElementRef.current;
            setPositionLeft(el.scrollLeft);
            setPositionWidth(el.scrollWidth);
            setPositionTop(el.scrollTop)
            if (config.console)
            console.log("set top", positionTop)
            const props = useStore.getState();
            let selectedRegion = props.selectedBalloon;
            for (let i = 0; i < menuNode.length; i++) {
                if (menuNode[i].getAttribute("data-value") === selectedRegion) {
                    //console.log("sss", el.scrollLeft , i)
                }
            }
        };
        const handlemenuNode = () => {
            for (let i = 0; i < menuNode.length; i++) {
                menuNode[i].style.display = 'none';
            }
        };
        const handlepopupNode = () => {
            for (let i = 0; i < popupNode.length; i++) {
                popupNode[i].style.display = 'none';
            }
        };
        const element = myElementRef.current;
        const popel = mypopup.current;
        if (popel) {
            popel.addEventListener("click", handlepopupNode);
        }
        element.addEventListener("scroll", handleScroll);
        window.addEventListener("click", handlemenuNode);
        

        return () => {
            element.removeEventListener("scroll", handleScroll);
            window.removeEventListener("click", handlemenuNode);
            window.removeEventListener("click", handlepopupNode);
        };
    }, []);

    const [positionscrollTop, setPositionscrollTop] = useState(0);
    useEffect(() => {
        const scrollDemo = document.querySelector("body");
        const getScrollPosition = () => {
            const position = document.body.scrollTop;
            setPositionscrollTop(position);
        };
        scrollDemo.addEventListener("scroll", getScrollPosition, { passive: true });

        return () => {
            scrollDemo.removeEventListener("scroll", getScrollPosition);
        };

    }, []);

 
    let selectedRegion = props.selectedRegion;
    let annotations = props.drawingRegions;
    let originalRegions = props.originalRegions;

    let pageNo = 0;
    let fileName = "";
    if (props.drawingDetails.length > 0 && props.ItemView != null) {
        pageNo = parseInt(Object.values(props.drawingDetails)[parseInt(props.ItemView)].currentPage);
        fileName = Object.values(props.drawingDetails)[parseInt(props.ItemView)].fileName;

    }

    const handleMouseDown = event => {
        if (props.drawingDetails.length === 0) {
            return;
        }
        const state = useStore.getState();
        if (state.isErrImage) {
            return;
        }
        if (selectedId === null && newAnnotation.length === 0) {
            let {x,y } = event.target.getStage().getPointerPosition();
           const { bgImgX, bgImgY, bgImgW, bgImgH } = props;
           // console.log(x, "<", (bgImgX + bgImgW), x, ">", bgImgX, y, ">", bgImgY, y, "<", (bgImgY + bgImgH), selectedRegion )
            const id = uuid();
            let date = new Date();
            date.toISOString().slice(0, 19).replace('T', ' ')

            if ((selectedRegion === "Spl" ||  selectedRegion === "Selected Region" || selectedRegion === "Unselected Region"
            ) && x < (bgImgX + bgImgW) && x > bgImgX && y > bgImgY && y < (bgImgY + bgImgH)) {
                setNewAnnotation([{
                      DrawLineID: ""
                    , BaloonDrwID: 0
                    , BaloonDrwFileID: fileName
                    , ProductionOrderNumber: "N/A"
                    , Part_Revision: ""
                    , Page_No: pageNo
                    , DrawingNumber: props.drawingNo
                    , Revision: props.revNo.toUpperCase()
                    , Balloon: annotations.length + 1
                    , Spec: ""
                    , Nominal: ""
                    , Minimum: ""
                    , Maximum: ""
                    , MeasuredBy: ""
                    , MeasuredOn: date
                    , Circle_X_Axis: x
                    , Circle_Y_Axis: y
                    , Circle_Width: 28
                    , Circle_Height: 28
                    , Balloon_Thickness: 10
                    , Balloon_Text_FontSize: 10
                    , ZoomFactor: "0.0"
                    , Crop_X_Axis: x
                    , Crop_Y_Axis: y
                    , Crop_Width: 0
                    , Crop_Height: 0
                    , Type: ""
                    , SubType: ""
                    , Unit: ""
                    , Quantity: 1
                    , ToleranceType: ""
                    , PlusTolerance: ""
                    , MinusTolerance: ""
                    , MaxTolerance: ""
                    , MinTolerance: ""
                    , CropImage: ""
                    , CreatedBy: ""
                    , CreatedDate: date
                    , ModifiedBy: ""
                    , ModifiedDate: date
                    , IsCritical:0
                    , x
                    , y
                    , width: 0
                    , height: 0
                    , id
                    , selectedRegion
                    , isballooned: false
                    , isDeleted: false
                    , subBalloon:[]
                }]);
                setNewRegion([{
                    DrawLineID: ""
                    , BaloonDrwID: 0
                    , BaloonDrwFileID: fileName
                    , ProductionOrderNumber: "N/A"
                    , Part_Revision: ""
                    , Page_No: pageNo
                    , DrawingNumber: props.drawingNo
                    , Revision: props.revNo.toUpperCase()
                    , Balloon: annotations.length + 1
                    , Spec: ""
                    , Nominal: ""
                    , Minimum: ""
                    , Maximum: ""
                    , MeasuredBy: ""
                    , MeasuredOn: date
                    , Circle_X_Axis: x
                    , Circle_Y_Axis: y
                    , Circle_Width: 28
                    , Circle_Height: 28
                    , Balloon_Thickness: 10
                    , Balloon_Text_FontSize: 10
                    , ZoomFactor: "0.0"
                    , Crop_X_Axis: x
                    , Crop_Y_Axis: y
                    , Crop_Width: 0
                    , Crop_Height: 0
                    , Type: ""
                    , SubType: ""
                    , Unit: ""
                    , Quantity: 1
                    , ToleranceType: ""
                    , PlusTolerance: ""
                    , MinusTolerance: ""
                    , MaxTolerance: ""
                    , MinTolerance: ""
                    , CropImage: ""
                    , CreatedBy: ""
                    , CreatedDate: date
                    , ModifiedBy: ""
                    , ModifiedDate: date
                    , IsCritical: 0
                    , x
                    , y
                    , width: 0
                    , height: 0
                    , id
                    , selectedRegion
                    , isballooned: false
                    , isDeleted: false
                    , subBalloon: []

                }]);
            }
        }
    };

    const handleMouseMove = event => {
        if (props.drawingDetails.length === 0) {
            return;
        }

        if (selectedId === null && newAnnotation.length === 1) {
            const sx = newAnnotation[0].x;
            const sy = newAnnotation[0].y;
            const { x, y } = event.target.getStage().getPointerPosition();
            if (config.console)
            console.log(x, y, sx, sy, groupRef.current.getRelativePointerPosition())
            let date = new Date();
            date.toISOString().slice(0, 19).replace('T', ' ')
         //   if (Math.sign(x - sx) === -1 || Math.sign(y - sy) === -1) { return; }
            const id = uuid();
            setNewAnnotation([
                {
                    DrawLineID: ""
                    , BaloonDrwID: 0
                    , BaloonDrwFileID: fileName
                    , ProductionOrderNumber: "N/A"
                    , Part_Revision: ""
                    , Page_No: pageNo
                    , DrawingNumber: props.drawingNo
                    , Revision: props.revNo.toUpperCase()
                    , Balloon: annotations.length + 1
                    , Spec: ""
                    , Nominal: ""
                    , Minimum: ""
                    , Maximum: ""
                    , MeasuredBy: ""
                    , MeasuredOn: date
                    , Circle_X_Axis: sx
                    , Circle_Y_Axis: sy
                    , Circle_Width: 28
                    , Circle_Height: 28
                    , Balloon_Thickness: 10
                    , Balloon_Text_FontSize: 10
                    , ZoomFactor: "0.0"
                    , Crop_X_Axis: sx
                    , Crop_Y_Axis: sy
                    , Crop_Width: x - sx
                    , Crop_Height: y - sy
                    , Type: ""
                    , SubType: ""
                    , Unit: ""
                    , Quantity: 1
                    , ToleranceType: ""
                    , PlusTolerance: ""
                    , MinusTolerance: ""
                    , MaxTolerance: ""
                    , MinTolerance: ""
                    , CropImage: ""
                    , CreatedBy: ""
                    , CreatedDate: date
                    , ModifiedBy: ""
                    , ModifiedDate: date
                    , IsCritical: 0
                    , x:sx
                    , y:sy
                    , width: x - sx
                    , height: y - sy
                    , id
                    , selectedRegion
                    , isballooned: false
                    , isDeleted: false
                    , subBalloon: []
                }
            ]);
            setNewRegion([{
                DrawLineID: ""
                , BaloonDrwID:0
                , BaloonDrwFileID: fileName
                , ProductionOrderNumber: "N/A"
                , Part_Revision: ""
                , Page_No: pageNo
                , DrawingNumber: props.drawingNo
                , Revision: props.revNo.toUpperCase()
                , Balloon: annotations.length + 1
                , Spec: ""
                , Nominal: ""
                , Minimum: ""
                , Maximum: ""
                , MeasuredBy: ""
                , MeasuredOn: date
                , Circle_X_Axis: sx
                , Circle_Y_Axis: sy
                , Circle_Width:28
                , Circle_Height: 28
                , Balloon_Thickness: 10
                , Balloon_Text_FontSize: 10
                , ZoomFactor: "0.0"
                , Crop_X_Axis: sx
                , Crop_Y_Axis: sy
                , Crop_Width: x - sx
                , Crop_Height: y - sy
                , Type: ""
                , SubType: ""
                , Unit: ""
                , Quantity: 1
                , ToleranceType: ""
                , PlusTolerance: ""
                , MinusTolerance: ""
                , MaxTolerance: ""
                , MinTolerance: ""
                , CropImage: ""
                , CreatedBy: ""
                , CreatedDate: date
                , ModifiedBy: ""
                , ModifiedDate: date
                , IsCritical: 0
                , x:sx
                , y:sy
                , width: x - sx
                , height: y - sy
                , id
                , selectedRegion
                , isballooned: false
                , isDeleted: false
                , subBalloon: []
            }]);
            
        }
    };

    const handleMouseUp = (event) => {
        if (props.drawingDetails.length === 0) {
            return;
        }

        if (selectedId === null && newAnnotation.length === 1) {
            
            let newone = newAnnotation[0];
            const origin = originalPosition(newone);
            
            // y < (props.bgImgX + props.bgImgW) && y > props.bgImgX && x > props.bgImgY && x < (props.bgImgY + props.bgImgH) 
            if (origin.width > 30 && origin.height > 30) {
                // stored values for the current image

                // Assign the original cropeed value to the newly created annotation
                //console.log(origin.x,origin.y,origin.width,origin.height)
                // add newly created to list
                //console.log(newone)
                newRegion[0].x = parseInt(origin.x);
                newRegion[0].y = parseInt(origin.y);
                newRegion[0].width = parseInt(origin.width);
                newRegion[0].height = parseInt(origin.height);

                newRegion[0].Crop_X_Axis = parseInt(origin.x);
                newRegion[0].Crop_Y_Axis = parseInt(origin.y);
                newRegion[0].Crop_Width = parseInt(origin.width);
                newRegion[0].Crop_Height = parseInt(origin.height);

                newRegion[0].Circle_X_Axis = parseInt(origin.x);
                newRegion[0].Circle_Y_Axis = parseInt(origin.y);
             //   newRegion[0].Circle_Width = parseInt(origin.width);
             //   newRegion[0].Circle_Height = parseInt(origin.height);
                annotations.push(...newAnnotation);
                if (config.console)
                console.log("originalRegions", "handleMouseUp", originalRegions, newRegion)
                originalRegions.push(...newRegion);
                useStore.setState({ drawingRegions: annotations })
                useStore.setState({ balloonRegions: annotations })
                useStore.setState({ scrollPosition: parseInt(positionLeft) });
                if (selectedRegion === "Selected Region" || selectedRegion === "Unselected Region") {
                   //  console.log("new region ",newRegion)
                    selectedRegionProcess(originalRegions);
                }
                if (selectedRegion === "Spl") {
                    selectedSPLRegionProcess(originalRegions);
                }
             }
            setNewAnnotation([]);
            setNewRegion([]);
        }
    };
    
    const handleMouseEnter = event => {
        
        if (selectedRegion === "Selected Region"
            || selectedRegion === "Unselected Region"
            || selectedRegion === "Spl"
        ) {
            event.target.getStage().container().style.cursor = "crosshair";
        } 
        const state = useStore.getState();
        if (state.isErrImage) {
            event.target.getStage().container().style.cursor = "auto";
        }

    };

    const handleKeyDown = event => {
        if (props.drawingDetails.length === 0) {
            return;
        }

        if (event.keyCode === 8 || event.keyCode === 46) {
            if (selectedId !== null) {
                useStore.setState({ selectAnnotation: null })
                /**
                const newAnnotations = annotations.filter(
                    annotation => annotation.id !== selectedId
                );
                const newannota = newAnnotations.map((item, i) => {
                    if (item.id !== i) {
                        // Update balloon property when removing/altering an element for UI
                        return { ...item, Balloon: i + 1 };
                    }
                    return item;
                })

                const newRegions = originalRegions.filter(
                reg => reg.id !== selectedId
                );
                const newOrg = newRegions.map((item,i) => {
                    if (item.id !== i) {
                        // Update balloon property when removing/altering an element for Backend
                        return { ...item, Balloon: i + 1 };
                    }
                    return item;
                })
                */
                //console.log("originalRegions","handleKeyDown")
                //   useStore.setState({ originalRegions: newOrg, is_BalloonDrawingSaved:false })
                //   useStore.setState({ drawingRegions: newannota })
                //   useStore.setState({ balloonRegions: newannota })
            }
        }
    };

    let annotationsToDraw = [...annotations, ...newAnnotation];

    //   if (selectedRegion === "Manual Drawn" || selectedRegion === "Full Image") {
    if (selectedRegion === "Full Image" && !props.savedDetails && annotations.length === 0) {
        annotationsToDraw = [];

    }
    

    // useStore({ scrollPosition: positionLeft });
   // if (config.console)
    console.log(props)


    let dim_image = "";
    if ((props.drawingDetails.length > 0 && props.ItemView != null)) {
        dim_image = props.drawingDetails.length > 0 ? Object.values(props.drawingDetails)[parseInt(props.ItemView)].drawing : "";
 
    }
    
    const selectedD = e => {
        e.preventDefault();
        let selected = parseInt(e.target.dataset.value);

        const { ItemView, drawingDetails, originalRegions } = useStore.getState();
        useStore.setState({
            scrollPosition: parseInt(positionLeft),
            konvaPositionTop: parseInt(positionTop),
            documentPositionTop: parseInt(positionscrollTop)
        });
        let pageNo = 0;

        if (drawingDetails.length > 0 && ItemView != null) {
            pageNo = Object.values(drawingDetails)[parseInt(ItemView)].currentPage;
        }

        let PageData = originalRegions.map((item) => {
            if (parseInt(item.Page_No) === parseInt(pageNo) && parseInt(item.Balloon) !== parseInt(selected)) {
                return item;
            }
            return false;
        }).filter(item => item !== false);
        let selectedRowIndex = "";
        if (PageData.length > 0) {
            var overData = [];
            if (parseInt(selected) - 1 > 0) {
                let overTemp = PageData.filter((item) => { return parseInt(item.Balloon) === parseInt(selected) - 1; });
                overData = Object.values(overTemp)[0];
            } else {
                let overTemp = PageData.filter((item) => { return parseInt(item.Balloon) === parseInt(selected) + 1; });
                overData = Object.values(overTemp)[0];
            }
            let prenxtData = 0;
            if (typeof overData == "undefined") {
                let overTemp = originalRegions.filter((item) => { return parseInt(item.Balloon) === parseInt(selected); });
                overData = Object.values(overTemp)[0];
                prenxtData = PageData.indexOf(overData) - 1;
            } else {

                prenxtData = PageData.indexOf(overData);
            }
            if (prenxtData > -1) {
                if (config.console)
                console.log(prenxtData, PageData, overData) 
                selectedRowIndex = PageData[prenxtData].Balloon
                useStore.setState({ selectedRowIndex: selectedRowIndex.toString() });
            }
        }
        if (config.console)
        console.log(PageData, originalRegions,  selectedRowIndex)

        Swal.fire({
            title: `Are you want to delete Balloon (${selected})?`,
            showCancelButton: true,
            confirmButtonText: 'Yes',
            allowOutsideClick: false,
            allowEscapeKey: false
        }).then((result) => {
            
            if (result.isConfirmed) {
                useStore.setState({ isLoading: true, loadingText: "Delete Balloon... Please Wait..." })
               
                setTimeout(() => {
                    let deletedOrg = originalRegions.map((item) => {
                        if (parseInt(item.Balloon) !== parseInt(selected)) {
                            return item;
                        }
                        return false;
                    }).filter(item => item !== false);
                    //const resetOverData = JSON.parse(JSON.stringify(deletedOrg));
                    const resetOverData = [...deletedOrg];

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

                                let setter = { ...e, newarr: { ...e.newarr, Balloon: b }, id: sid, DrawLineID: i, Balloon: b, selectedRegion :""};
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
                                        let pb = parseInt(Balloon).toString() + "." + (qi ).toString();
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
                                            let setter = { ...e, newarr: { ...e.newarr, Balloon: b }, id: sqid, DrawLineID: i, Balloon: b, selectedRegion:"" };
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

                  //  console.log(newitems)

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
              //  return false;

               
                useStore.setState({ selectedBalloon: null });
                setTimeout(() => { useStore.setState({ ItemView: null });}, 200);
                setTimeout(() => { useStore.setState({ ItemView: ItemView }); } , 200);
                const dstate = useStore.getState();
                setTimeout(function () {
                    let scrollElement = document.querySelector('#konvaMain');
                    if (scrollElement !== null) {
                        scrollElement.scrollLeft = dstate.scrollPosition;
                        scrollElement.scrollTop = dstate.konvaPositionTop;
                    }
                    document.body.scrollTop = dstate.documentPositionTop

                }, 500);
            } else {
                
                setTimeout(function () {
                    const dstate = useStore.getState();
                    let scrollElement = document.querySelector('#konvaMain');
                    if (scrollElement !== null) {
                        scrollElement.scrollLeft = dstate.scrollPosition;
                        scrollElement.scrollTop = dstate.konvaPositionTop;
                    }
                    document.body.scrollTop = dstate.documentPositionTop

                }, 500);
            }
        });
        return false;

    }

    const selectedB = e => {
        e.preventDefault();
        let s = (e.target.dataset.value);
        console.log(typeof s)
        selectAnnotation(null);
        const props = useStore.getState();
        let annotations = props.drawingRegions;
        let drawingDetails = annotations.map((item) => {
            return { ...item, selected: false };
        });
        if (config.console)
        console.log("selectedB konvaPositionTop",positionTop)
        useStore.setState({
            scrollPosition: parseInt(positionLeft),
            konvaPositionTop: parseInt(positionTop),
            documentPositionTop: parseInt(positionscrollTop),
            selectedRowIndex:s,
            selectedBalloon: s, drawingRegions: drawingDetails
        })
        
    };

    const selectedMove = e => {
        console.log("build selectedmove function calles")
        e.preventDefault();
        let s = parseInt(e.target.dataset.value);
        let p = (positionLeft > 1) ? positionLeft : 1;
        if (config.console)
        console.log("selectedMove konvaPositionTop", positionTop)
        useStore.setState({
            scrollPosition: parseInt(p),
            konvaPositionTop: parseInt(positionTop),
            documentPositionTop: parseInt(positionscrollTop)
        });
         //console.log("start", s,p)
        const props = useStore.getState();
        let annotations = props.drawingRegions;
        let drawingDetails = annotations.map((item) => {
            if (!item.hasOwnProperty("selected")) {
                item.selected = false;
            }
            return item;
        });
        drawingDetails = drawingDetails.map((item) => {
            if (s  === parseInt(item.Balloon)) {
                item.selected = true;
            } else {
                item.selected = false;
            }
            return item;
        });
        const n = drawingDetails.filter(a => a.selected === true)[0].id;
        //console.log(n)
        setTimeout(() => {
            useStore.setState({
                drawingRegions: drawingDetails,
                selectedRowIndex:s
            })
            selectAnnotation(n);

        }, 500);

    }

    const subBalloon = e => {
        e.preventDefault();
        let s = parseInt(e.target.dataset.value);
        useStore.setState({ scrollPosition: parseInt(positionLeft), selectedRowIndex:s });
        if (config.console)
            console.log(s)

        const { originalRegions  } = useStore.getState();
        
        //const dup = JSON.parse(JSON.stringify(originalRegions));
        const dup = [...originalRegions];
 
        const filtered = dup.reduce((result, item) => {
            if (!result[parseInt(item.Balloon)]) {
                
                result[parseInt(item.Balloon)] = item;
            }
            return result;
        }, {});

        let qtyi = 0;
        // get all quantity parent
        let Qtyparent = dup.reduce((res, item) => {
            if (item.hasOwnProperty("subBalloon") && item.subBalloon.length >= 0 && item.Quantity > 1) {
                res[qtyi] = item;
                qtyi++;
            }
            return res;
        }, []);

        let finalshape = Object.values(filtered);
        if (config.console)
            console.log("finalshape", finalshape)
        //return false;
        var clone = finalshape.filter(function (item) {
            return parseInt(item.Balloon) === parseInt(s);
        });
        var cloneFirst = clone[0];

        let childObject = { ...cloneFirst.newarr };
        let keysToReset = ["Balloon"
            , "Spec"
            , "Nominal"
            , "Minimum"
            , "Maximum"
            , "Type"
            , "SubType"
            , "Unit"
            , "Quantity"
            , "ToleranceType"
            , "PlusTolerance"
            , "MinusTolerance"
            , "MaxTolerance"
            , "MinTolerance"
            , "DrawLineID"];
        keysToReset.forEach((key) => {
            childObject[key] = '';
        });
        let keyValuesToUpdate = {
            Circle_X_Axis: cloneFirst.newarr.Circle_X_Axis,
            Circle_Y_Axis: cloneFirst.newarr.Circle_Y_Axis ,
            Circle_Width: cloneFirst.newarr.Circle_Width,
            Circle_Height: cloneFirst.newarr.Circle_Height,
            Crop_X_Axis: cloneFirst.newarr.Crop_X_Axis,
            Crop_Y_Axis: cloneFirst.newarr.Crop_Y_Axis ,
            Crop_Width: cloneFirst.newarr.Crop_Width,
            Crop_Height: cloneFirst.newarr.Crop_Height,
            Quantity: 1,
            selectedRegion:"",
            isDeleted:false
            //isballooned: false
        };
        childObject = { ...childObject, ...keyValuesToUpdate };
 
        let subBalloon = {};
        subBalloon = { ...childObject, newarr: childObject, isballooned: false };
        clone = clone.map((item) => {
           // item.subBalloon.push(subBalloon);
            return item;
        });
       
        cloneFirst = clone[0];
        if (config.console)
        console.log(cloneFirst)
        let changedsingle = [];
        let selectedBalloon = [];
        const id = uuid();
        if (cloneFirst.Quantity === 1 && cloneFirst.subBalloon.length >= 0) {
           
            let pb = parseInt(cloneFirst.Balloon).toString() + ".1";
            let newarr = { ...cloneFirst.newarr, Balloon:pb };
            changedsingle.push({ ...cloneFirst, newarr: newarr,  id: id, DrawLineID: 0, Balloon: pb });
            let newSubItem = cloneFirst.subBalloon.push(subBalloon);
            newSubItem = cloneFirst.subBalloon.filter(a => {
                return a.isDeleted === false;
            });
            selectedBalloon = newSubItem.reduce(function (p, e, ei) {
                let sno = ei + 2;
                const sid = uuid();
                let b = parseInt(cloneFirst.Balloon).toString() + "." + sno.toString();
                if (ei === newSubItem.length -1)
                p.push(b);
                let newarr = { ...e.newarr, Balloon: b };
                changedsingle.push({ ...e, newarr: newarr, id: sid, DrawLineID: 0, Balloon: b });

                return p;
            },[])
        }
        if (cloneFirst.Quantity > 1 && cloneFirst.subBalloon.length >= 0) {
            for (let qi = 1; qi <= cloneFirst.Quantity; qi++) {
                const qid = uuid();
                let pb = parseInt(cloneFirst.Balloon).toString() + "." + qi.toString();

                let newMainItem = Qtyparent.map(item => {
                    if (parseInt(cloneFirst.Balloon) === parseInt(item.Balloon) && pb === item.Balloon) {

                        let subBalloons = JSON.parse(JSON.stringify(item.subBalloon));
                        //let subBalloons = [...item.subBalloon];
                        if (qi === 1) {
                          //  subBalloons.splice(-1);
                        }
                        subBalloons.push(subBalloon);
                        item.subBalloon = subBalloons;
                        return item;
                    }
                    return false;
                }).filter(x => x !== false);
                if (config.console)
                console.log("newMainItem", newMainItem)
                if (newMainItem.length > 0) {
                    let nmi = newMainItem[0]
                    let newarr = { ...nmi.newarr, Balloon: pb };
                    changedsingle.push({ ...nmi, newarr: newarr, id: qid, DrawLineID: 0, Balloon: pb });
                    let newSubItem = nmi.subBalloon.filter(a => {
                        return a.isDeleted === false;
                    });
                    let selected = newSubItem.reduce(function (p, e, ei) {
                        let sqno = ei + 1;
                        const sqid = uuid();
                        let b = pb + "." + sqno.toString();
                        if (ei === newSubItem.length - 1)
                        p.push(b);
                        let newarr = { ...e.newarr, Balloon: b };
                        changedsingle.push({ ...e, newarr: newarr, id: sqid, DrawLineID: 0, Balloon: b });
                        return p;
                    }, [])
                    if (selected.length > 0) {
                        selectedBalloon.push(selected[0])
                    }
                }
            }
        }
        let deletedOrg = originalRegions.map((item) => {
            
            if (parseInt(item.Balloon) !== parseInt(s)) {
                return item;
            }
            return false;
        }).filter(item => item !== false);


        var overData = Object.values(originalRegions)[0];
        if (parseInt(s) - 1 > 0) {
            let overTemp = originalRegions.filter((item) => { return parseInt(item.Balloon) === parseInt(s); });
            overData = Object.values(overTemp)[0];
        }
        let newStore = deletedOrg.slice(0);
        var fromIndex = originalRegions.indexOf(overData);
        newStore.splice(fromIndex, 0, ...changedsingle);
        if (config.console)
        console.log( selectedBalloon, newStore, fromIndex, overData)
        //return false;
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

        //return false;
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

        if (config.console)
            console.log(selectedBalloon[0], parseInt(positionLeft))

            setTimeout(() => { useStore.setState({   selectedBalloon: null }); }, 1);
            setTimeout(() => { useStore.setState({   selectedBalloon: selectedBalloon.length > 0 ? selectedBalloon[0] : null }); } , 2);

    };


    //const overData = JSON.parse(JSON.stringify(annotationsToDraw));
    const overData = [...annotationsToDraw];
    const newstore = overData.map((i) => {
        return i;
    });
    const filtered = newstore.reduce((result, item) => {
        if (!result[parseInt(item.Balloon)]) {
            result[parseInt(item.Balloon)] = item;
        }
        return result;
    }, {});
    const finalshape = Object.values(filtered);

    let createshape = annotationsToDraw.map(a => {
        return parseInt(a.Balloon);
    }).filter(a => a !== '');
    createshape = [...new Set(createshape)];

    let menushape = annotationsToDraw.map(a => {
        return parseInt(a.Balloon);
    }).filter(a => a !== '');
    menushape = [...new Set(menushape)];

    const subItemList = (e) => {
        return e.map((value, i) => {
            if (!value.isDeleted && value.isballooned) {
                return (
                    <li key={i + "s_spec"} className="spec-button"><label>Sub: </label> {value.Spec} </li>
                );
            }
            return false;
        }).filter(x => x!== false);
    }
    //console.log(finalshape)
    return (
        <>

            <main className="mt-1" >
                {/** <LargeImageCanvas /> */}
                <div className="tools-buttons d-none" width={props.width}
                    height={props.height}>
                    <Nav>
                        <NavItem
                            className="bg-light box  text-right">
                                <Button className="btn btn-primary buttons">top left</Button>
                        </NavItem>
                    </Nav>
 
                    {/** <Buttons drawingDetails={props.drawingDetails} ItemView={props.ItemView} />   */}
                </div>
                
                <section id="konvaMain"
                ref={myElementRef}
                className="canvas scroll-smooth items-center overflow-x-auto overflow-y-auto"
                style={{
                    overflowX: (props.drawingDetails.length > 0 && props.ItemView !== null) ? (!props.fitscreen ? "scroll" : "hidden"): "hidden",
                    overflowY: (props.drawingDetails.length > 0 && props.ItemView !== null) ? (!props.fitscreen ? "scroll" : "hidden") : "hidden",
                    //width: ((!props.fitscreen) ? (props.width - 120) : props.win.width) + "px",
                    height: ((!props.fitscreen) ? 650 : props.win.height) + "px"
                    }}
                    width={!props.fitscreen ? (props.width - 120) : props.win.width}
                    height={!props.fitscreen ? 650 : props.win.height}
            >
                
           
            
            <React.Fragment>
                    <div tabIndex={1} onKeyDown={handleKeyDown}>
                            <PopupModal  {...props}  />
                        
                            {finalshape.map((annotation, i) => {
                                if (annotation.Page_No === pageNo) {
                                    
                                    if (menushape.includes(parseInt(annotation.Balloon))) {
                                        menushape = removeA(menushape, parseInt(annotation.Balloon))
                                        let html = "";
                                        if (annotation.subBalloon.length > 0) {
                                            html = subItemList(annotation.subBalloon);
                                        }
                                        return (
                                            <>
                                                <div key={"pop" + i} data-value={parseInt(annotation.Balloon)} className={"popup"} >
                                                    <div ref={mypopup} key={i +"pop"}>
                                                        <ul key={i + "_childpop"} style={{ width:"max-content", height:"auto" }} className={"overflow-x-auto overflow-y-auto" }>
                                                            <li key={i + "_balloon"} className="balloon-button"><label>Balloon #: </label> <b>{parseInt(annotation.Balloon)}</b> </li>
                                                            <li key={i + "_qty"} className="qty-button"><label>Qty: </label> <b>{annotation.Quantity}</b>  </li>
                                                            <li key={i + "_spec"} className="spec-button"><label>Spec: </label> {annotation.Spec} </li>
                                                            {html}
                                                        </ul>
                                                        
                                                    </div>
                                                </div>
                                                <div key={i+"context"} data-value={parseInt(annotation.Balloon)} className={"contextmenu"} >
                                                    <ul key={i + "_childcontext"}>
                                                        <li key={i + "_select"} data-value={(annotation.Balloon).toString()} onClick={selectedB} className="select-button">Select</li>
                                                        <li key={i + "_delete"} data-value={parseInt(annotation.Balloon)} onClick={selectedD} className="delete-button">Delete</li>
                                                        <li key={i + "_move"} data-value={parseInt(annotation.Balloon)} onClick={selectedMove} className="pulse-button">Move Balloon</li>

                                                        <li key={i + "_duplicate"} data-value={parseInt(annotation.Balloon)} onClick={subBalloon} className="dummy-button">Sub-Balloon</li>

                                                    </ul>
                                                </div>
                                            </>
                                        );
                                    } 
                                } else { return (<div key={i+"unknown"} data-value={annotation.Balloon}></div>); } 
                                return annotation;
                            }
                            
                            )} 
                             
                             

                        <Stage
                                ref={stageRef}
                                width={props.win.width}
                                height={props.win.height}
                                x={0}
                                y={0}
                                id="konva"

                                onMouseEnter={handleMouseEnter}
                                onMouseDown={handleMouseDown}
                                onMouseMove={handleMouseMove}
                                onMouseUp={handleMouseUp}
                                onClick={(e) => {
                                  //  e.evt.preventDefault();
                                    
                                    return false;
                                }}
                            
                >

                                <Layer  >
                                    
                    <PatternImage src="https://i.stack.imgur.com/7nF5K.png"
                        x={0}
                           width={props.win.width}
                           height={props.win.height}
                                        y={0}
                                        scr={myElementRef}
                                    />
                                     
                 
                                   
                    <Group
                                    width={props.bgImgW}
                                    x={props.bgImgW/2}
                                    y={props.bgImgH/2}
                                    height={props.bgImgH}
                                    rotation={props.bgImgRotation}
                                    ref={groupRef}
                                    offset={{ x: props.bgImgW / 2, y: props.bgImgH / 2 }}
                                     
                       
 
                    >
                               
                        {(props.drawingDetails.length > 0 && props.ItemView != null) && (
                                <>
                                  
                                                <URLImage
                                                   
                                    src={dim_image}
                                    ItemView={props.ItemView}
                                    x={props.bgImgX}
                                    width={props.win.width}
                                    height={props.win.height}
                                    y={props.bgImgY}
                                    bgImgRotation={props.bgImgRotation}
                                                onMouseDown={() => {
                                                    const props = useStore.getState();
                                                    let annotations = props.drawingRegions;
                                                    let drawingDetails = annotations.map((item) => {
                                                        return { ...item, selected :false};
                                                    });
                                                    useStore.setState({
                                                        drawingRegions: drawingDetails
                                                    })
                                                   // console.log(drawingDetails)
                                        // deselect when clicked on empty area
                                        selectAnnotation(null);
                                        useStore.setState({ selectAnnotation: null })
                                    }}
                                                />


                                    
                                            </>
                                
                        )}                       

                                      
                   
                                        {annotationsToDraw.map((annotation, i) => {
                            
                                            if (annotation.Page_No === pageNo) {
                                                if (createshape.includes(parseInt(annotation.Balloon))) {
                                                    createshape = removeA(createshape, parseInt(annotation.Balloon))
                                                    let movecircle = props.zoomoriginalRegions.filter(item => item.intBalloon === parseInt(annotation.Balloon));
                                                    if (movecircle.length === 0) {
                                                        movecircle = [{ dx: 0, dy: 0 }];

                                                    }
                                                    //console.log(movecircle)
                                                    return (
                                                        <Annotation
                                                            key={i + "ann"}
                                                            keyplace={parseInt(annotation.Balloon)}
                                                            movecircle={movecircle}
                                                            // rotation={props.bgImgRotation}
                                                            fitscreen={props.fitscreen}
                                                            props={props}
                                                            positionscrollTop={positionscrollTop}
                                                            positionLeft={positionLeft}
                                                            positionWidth={positionWidth}
                                                            shapeProps={annotation}

                                                            isSelected={annotation.id === selectedId}
                                                            onMouseDown={() => {
                                                                //console.log('ssss')
                                                            }}
                                                            onSelect={() => {
                                                                selectAnnotation(annotation.id);
                                                                setNewAnnotation([]);
                                                                useStore.setState({ selectAnnotation: annotation.id })

                                                            }}
                                                            onChange={newAttrs => {
                                                                console.log("build canvs annotation components")
                                                                const rects = finalshape.slice();
                                                                rects[i] = newAttrs;
                                                                let oldvalue = originalRegions.filter((i) => {
                                                                    if (i.id === newAttrs.id) { return i; }
                                                                    else { return false; }
                                                                }).filter((i) => i !== false);
                                                                if (config.console)
                                                                    console.log("oldvalue", oldvalue)
                                                                if (config.console)
                                                                    console.log("newly changed", newAttrs, finalshape)

                                                                const newOriginalAttrs = ballonOriginalPosition(newAttrs);
                                                                if (config.console)
                                                                    console.log(newOriginalAttrs)
                                                                // return false;
                                                                const newrects = originalRegions.map((item) => {
                                                                    if (item.id === newAttrs.id) {
                                                                        if (item.hasOwnProperty("newarr")) {
                                                                            item.newarr.Circle_X_Axis = parseInt(newOriginalAttrs.x);
                                                                            item.newarr.Circle_Y_Axis = parseInt(newOriginalAttrs.y);
                                                                            //delete item.xx;
                                                                            //delete item.xy;
                                                                        }
                                                                        // Update isActive property when removing an element
                                                                        return item;
                                                                    }
                                                                    return item;
                                                                })

                                                                // console.log("after", newrects.filter((i) => { if (i.id === newAttrs.id) return i; }))
                                                                // return false;
                                                                selectAnnotation(null);
                                                                setNewAnnotation([]);

                                                                // Force re-render after balloon position update (production environment fix)
                                                                productionSafeTimeout(() => { 
                                                                    useStore.setState({ ItemView: props.ItemView }); 
                                                                    
                                                                    // Additional production environment handling
                                                                    if (isProduction()) {
                                                                        // Force stage redraw in production using utility
                                                                        forceStageElementRedraw('#konvaMain', 15);
                                                                    }
                                                                }, 50);
                                                                
                                                                setTimeout(() => {
                                                                    let scrollElement = document.querySelector('#konvaMain');
                                                                    //console.log("last", props.scrollPosition)
                                                                    if (scrollElement !== null) {
                                                                        scrollElement.scrollLeft = props.scrollPosition;
                                                                        scrollElement.scrollTop = props.konvaPositionTop;

                                                                    }
                                                                    document.body.scrollTop = props.documentPositionTop
                                                                    
                                                                    // Additional production environment handling for scroll
                                                                    if (isProduction()) {
                                                                        // Force another redraw after scroll positioning
                                                                        productionSafeTimeout(() => {
                                                                            forceStageElementRedraw('#konvaMain', 20);
                                                                        }, 25);
                                                                    }
                                                                }, 50);

                                                                //console.log(newrects[i])
                                                                if (newrects[i].selectedRegion === "Manual Drawn") {
                                                                    if (config.console)
                                                                        console.log("re")

                                                                }


                                                            }

                                                            }
                                                        />
                                                    );
                                                }
                                            }
                                            else {
                                                return (<Circle
                                                    key={i + "unknown"}
                                                    visible={false}
                                                    data-value={annotation.Balloon}
                                                    ></Circle>);
                            } 
                           // else { return false; }
                                            return false;
                            })}
                             
                    </Group>
                        
                           
                            </Layer>
                           
                    </Stage>
                </div>
                </React.Fragment>
                </section> 
                <Table  {...props}  />
        </main >
            
        </>
    );
}