import React, { useEffect, useCallback } from "react";
import { Rect, Circle, Text, Group, Transformer } from "react-konva";
import useStore from "../Store/store";
import initialState from "../Store/init";
import { 
    isProduction, 
    forceStageRedraw, 
    forceLayerRedraw, 
    productionSafeTimeout,
    forceMultipleRedraws 
} from "../Common/ProductionUtils";


const Annotation = ({ movecircle, shapeProps, isSelected, onSelect, onChange, keyplace, rotation, positionWidth, positionLeft, fitscreen, positionscrollTop, props }) => {
    const shapeRef = React.useRef();
    const transformRef = React.useRef();
    const cirRef = React.useRef();
    const groupRef = React.useRef();
    const [forceUpdate, setForceUpdate] = React.useState(0);
    //console.log({ movecircle, shapeProps, isSelected, onSelect, onChange, keyplace, rotation, positionWidth, positionLeft })

    // Force re-render function for production environment issues
    const forceReRender = useCallback(() => {
        try {
            setForceUpdate(prev => prev + 1);
            
            // Use utility functions for consistent production handling
            if (groupRef.current) {
                if (groupRef.current.getLayer()) {
                    forceLayerRedraw(groupRef.current.getLayer());
                }
                if (groupRef.current.getStage()) {
                    forceStageRedraw(groupRef.current.getStage());
                }
            }
            
            // Additional production environment handling
            if (isProduction()) {
                // Force multiple redraws in production for reliability
                const redrawFunctions = [
                    () => groupRef.current?.getLayer()?.batchDraw(),
                    () => groupRef.current?.getStage()?.batchDraw(),
                    () => setForceUpdate(prev => prev + 1)
                ].filter(Boolean);
                
                forceMultipleRedraws(redrawFunctions, 15);
            }
        } catch (error) {
            console.warn('Force re-render failed:', error);
        }
    }, []);

    useEffect(() => {
        if (isSelected) {
            // we need to attach transformer manually
           // transformRef.current.setNode(shapeRef.current);
            transformRef.current.nodes([groupRef.current]);
            transformRef.current.getLayer().batchDraw();
        }
    }, [isSelected]);

    // Force re-render when movecircle changes (production environment fix)
    useEffect(() => {
        if (movecircle && movecircle.length > 0 && (movecircle[0].dx !== 0 || movecircle[0].dy !== 0)) {
            // Small delay to ensure state is properly updated
            const timer = setTimeout(() => {
                forceReRender();
            }, 50);
            return () => clearTimeout(timer);
        }
    }, [movecircle, forceReRender]);

    // Production environment specific handling
    useEffect(() => {
        if (isProduction() && movecircle && movecircle.length > 0) {
            // Additional production environment checks
            const hasPositionChange = movecircle.some(item => item.dx !== 0 || item.dy !== 0);
            if (hasPositionChange) {
                // Force re-render with longer delay for production
                const timer = productionSafeTimeout(() => {
                    forceReRender();
                }, 100);
                return () => {
                    if (timer) clearTimeout(timer);
                };
            }
        }
    }, [movecircle, forceReRender]);

    // Cleanup effect for production environment
    useEffect(() => {
        return () => {
            // Cleanup any pending timers or operations
            if (groupRef.current && groupRef.current.getLayer()) {
                try {
                    groupRef.current.getLayer().batchDraw();
                } catch (error) {
                    // Ignore cleanup errors
                }
            }
        };
    }, []);


    const onMouseEnter = event => {
        event.target.getStage().container().style.cursor = "move";
    };

    const onMouseLeave = event => {
        event.target.getStage().container().style.cursor = "crosshair";
    };
    const onMouseEnterGroup = event => {
        event.target.getStage().container().style.cursor = "initial";
    };
    //let state = useStore.getState();


    // set the circle text to be center
    let circleWidth = 0;
    let textWidth = 0;
    let desiredBalloon = 1;
    if (fitscreen) {
        desiredBalloon = 1.5;
    }
    switch (keyplace.toString().length) {
        case 1:
            circleWidth = 20;
            textWidth = 16;
            break;
        case 2:
            circleWidth = 22;
            textWidth = 22;
            break;
        case 3:
            circleWidth = 25;
            textWidth = 28;
            break;
        case 4:
            circleWidth = 28;
            textWidth = 34;
            break;
        default:
            circleWidth = 20;
            textWidth = 16;
            break;
    }
 
 /*
    const pulseShape = (shape) => {
        // use Konva methods to animate a shape
        shape.to({
            scaleX: 1.5,
            scaleY: 1.5,
            onFinish: () => {
                shape.to({
                    scaleX: 1,
                    scaleY: 1,
                });
            },
        });
    };
    const handleCircleClick = (e) => {
        // another way to access Konva nodes is to just use event object
        const shape = e.target;
        pulseShape(shape);
        // prevent click on stage
        e.cancelBubble = true;
    };
    */

    //console.log(movecircle, movecircle[0].intBalloon, movecircle[0].dx, movecircle[0].dy)
 
    let circle_text_x = shapeProps.Circle_X_Axis -7 + movecircle[0].dx;
    let circle_text_y = shapeProps.Circle_Y_Axis -10 + movecircle[0].dy;
    if (fitscreen) {
        if (circle_text_x < 0)
            circle_text_x = shapeProps.Circle_X_Axis;
        if (circle_text_y < 10)
            circle_text_y = 10;
       //console.log(keyplace, circle_text_x, circle_text_y, circle_text_x - (textWidth / desiredBalloon) / 2 )
    }
    //console.log(keyplace, shapeProps.Circle_X_Axis, shapeProps.Circle_Y_Axis, movecircle, circle_text_x, circle_text_y, circle_text_x - (textWidth / desiredBalloon) / 2)
    return (
        <React.Fragment>      
 
            {(shapeProps.selectedRegion !== "Selected Region" && shapeProps.selectedRegion !== "Unselected Region" && shapeProps.selectedRegion !== "Spl") &&  (
                <>
                    <Group
                        key={`group-${keyplace}-${forceUpdate}`}
                        //x={shapeProps.width / 2} y={shapeProps.height / 2}
                        // offset={{ x: shapeProps.width / 2, y: shapeProps.height / 2 }}
                        onClick={(e) => {
                            //handleClick();
                        }}
                        rotation={rotation}
                        onMouseEnter={onMouseEnterGroup}
                        onDblClick={(e) =>
                            {
                                e.evt.preventDefault(true);
                                e.evt.stopPropagation();                          
                            useStore.setState({ selectedBalloon: shapeProps.Balloon, scrollPosition: parseInt(positionLeft) });
                            }
                        }
                        onMouseLeave={(e) => {
                            var popupNode = document.getElementsByClassName('popup');
                            for (let i = 0; i < popupNode.length; i++) {
                                popupNode[i].style.display = 'none';
                            }
                        } }
                        onMouseOver={(e) => {
                           // handleCircleClick(e)
                            var popupNode = document.getElementsByClassName('popup');
                            let selectedRegion = e.target.attrs.text;
                            let mousePosition = e.target.getStage().getPointerPosition();
                            let containerRect = e.target.getStage().container().getBoundingClientRect();
                            //let scrollElement = document.querySelector('#konva');
                            // console.log(scrollElement.scrollLeft)
                            const props = useStore.getState();
                            var doc = document.documentElement;
                            var top = (window.pageYOffset || doc.scrollTop) - (doc.clientTop || 0);
                            for (let i = 0; i < popupNode.length; i++) {
                                if (popupNode[i].getAttribute("data-value") === selectedRegion) {
                                    popupNode[i].style.display = 'initial';
                                    popupNode[i].style.position = 'absolute';
                                    let bargap = parseInt(positionWidth - containerRect.width);
                                    let multiplex = ((props.win.width === initialState.win.width && props.win.height === initialState.win.height) && (props.imageWidth < 32767 || props.imageHeight < 32767)) ? 1 : 2;
                                    let bardiv = (((bargap / 2) > 0) ? (bargap / 2) : 0) * multiplex;
                                    let pads = (bardiv > 0) ? (props.pad * 2) : 0;
                                    if (positionscrollTop < 130) {
                                        popupNode[i].style.top = containerRect.top + mousePosition.y + top + 4 + 'px';
                                    } else {
                                        popupNode[i].style.top = positionscrollTop + containerRect.top + mousePosition.y + top + 4 + 'px';
                                    }
                                    let adjust = 0;
                                    let intLeft = parseInt(positionLeft);
                                    if (props.bgImgX === 0 || props.bgImgY === 0) {

                                    } else {
                                        if (intLeft === 0) {
                                            adjust = intLeft + (containerRect.width - positionWidth) + bardiv - pads;
                                        } else {
                                            if (intLeft < bargap) {
                                                adjust = intLeft + (containerRect.width - positionWidth) + bardiv - intLeft - pads;
                                            } else if (intLeft === bargap) {
                                                adjust = (containerRect.width - positionWidth) + bardiv - pads;
                                            }
                                        }
                                    }
                                    popupNode[i].style.left = mousePosition.x - (props.bgImgX - props.pad) + ((props.win.width - props.bgImgW) / 2) + adjust - positionLeft +75 + 'px';
                                } else {
                                    popupNode[i].style.display = 'none';
                                }
                            }
                            
                        }
                        }
                       onContextMenu={(e) => {
                        // do not show native context
                        e.evt.preventDefault(true);
                        let mousePosition = e.target.getStage().getPointerPosition();
                        let containerRect = e.target.getStage().container().getBoundingClientRect();
                        //let scrollElement = document.querySelector('#konva');
                       // console.log(scrollElement.scrollLeft)
                        const props = useStore.getState();
                        var doc = document.documentElement;
                        var top = (window.pageYOffset || doc.scrollTop) - (doc.clientTop || 0);
                        var menuNode = document.getElementsByClassName('contextmenu');
                        let selectedRegion = e.target.attrs.text;
                        for (let i = 0; i < menuNode.length; i++) {
                            if (menuNode[i].getAttribute("data-value") === selectedRegion) {
                                menuNode[i].style.display = 'initial';
                                menuNode[i].style.position = 'absolute';
                               // let pr = positionLeft + positionWidth;
                                let bargap = parseInt(positionWidth - containerRect.width);
                                //let scrollElement = document.querySelector('#konvaMain');
                                let multiplex = ((props.win.width === initialState.win.width && props.win.height === initialState.win.height) && (props.imageWidth < 32767 || props.imageHeight < 32767)) ? 1 : 2;
                                let bardiv = (((bargap / 2) > 0) ? (bargap / 2) : 0) * multiplex;
                                let pads = (bardiv > 0) ? (props.pad * 2) : 0;
                                if (positionscrollTop < 130) {
                                    menuNode[i].style.top = containerRect.top + mousePosition.y + top + 4 + 'px';
                                } else {
                                    menuNode[i].style.top = positionscrollTop + containerRect.top + mousePosition.y + top + 4 + 'px';
                                }
                                let adjust = 0;
                                let intLeft = parseInt(positionLeft);
                                if (props.bgImgX === 0 || props.bgImgY === 0)
                                {

                                } else {
                                    if (intLeft === 0) {
                                        adjust = intLeft + (containerRect.width - positionWidth) + bardiv - pads  ;
                                    } else {
                                        if (intLeft < bargap) {
                                            adjust = intLeft + (containerRect.width - positionWidth) + bardiv - intLeft - pads ;
                                        } else if (intLeft === bargap) {
                                            adjust = (containerRect.width - positionWidth) + bardiv - pads  ;
                                        }
                                    }
                                }

                                //console.log(props )
                                //console.log(positionLeft, positionWidth, ((props.win.width - props.bgImgW) / 2), positionLeft + containerRect.width - positionWidth)
                                
                                menuNode[i].style.left =  mousePosition.x + ((props.win.width - props.bgImgW) / 2) + adjust - positionLeft + 4 + 'px';
                            } else {
                                menuNode[i].style.display = 'none';
                            }
                            
                        }

                       // useStore.setState({ selectedBalloon: shapeProps.Balloon });
                        }}
                        draggable={isSelected ? true:false}
                        ref={groupRef}
                        onDragStart={e => {
                            var popupNode = document.getElementsByClassName('popup');
                            for (let i = 0; i < popupNode.length; i++) {
                                popupNode[i].style.display = 'none';
                            }
                        }}
                        onDragEnd={event => {
                            onChange({
                                ...shapeProps,
                                xx: event.target.x(),
                                xy: event.target.y()
                            });
                            useStore.setState({ ItemView: null, isLoading: true, loadingText: "Updating new Position..." })
                            
                            // Force re-render after position update (production environment fix)
                            productionSafeTimeout(() => {
                                forceReRender();
                                // Force Konva stage to redraw using utility
                                if (event.target.getStage()) {
                                    forceStageRedraw(event.target.getStage(), 20);
                                }
                            }, 100);
                        }}
                        onTransformEnd={event => {
                            // transformer is changing scale of the node
                            // and NOT its width or height
                            // but in the store we have only width and height
                            // to match the data better we will reset scale on transform end
                            const node = groupRef.current;
                           // const scaleX = node.scaleX();
                           // const scaleY = node.scaleY();
                            // we will reset it back
                            //node.scaleX(1);
                            //node.scaleY(1);
                            onChange({
                                ...shapeProps,
                                xx: node.x(),
                                xy: node.y()
                            });
                            
                            // Force re-render after transform (production environment fix)
                            productionSafeTimeout(() => {
                                forceReRender();
                                // Force Konva stage to redraw using utility
                                if (node && node.getStage()) {
                                    forceStageRedraw(node.getStage(), 20);
                                }
                            }, 100);
                        }}
                    >
                   
                        <Circle
                            key={`circle-${keyplace}-${forceUpdate}`}
                            ref={cirRef}
                            width={circleWidth / desiredBalloon}
                            height={circleWidth / desiredBalloon}
                            id={keyplace.toString()}
                            //width={shapeProps.Circle_Width}
                            //height={shapeProps.Circle_Height}
                            //x={shapeProps.Crop_X_Axis - 7}
                            x={circle_text_x }
                            //y={shapeProps.Crop_Y_Axis - 10}
                            y={circle_text_y}
                            fill="transparent"
                            strokeWidth={1}
                            stroke="red"
                            className={"balloonC"}
                            
                    />
                        <Text
                            key={`text-${keyplace}-${forceUpdate}`}
                            width={(textWidth / desiredBalloon)}
                            height={20}
                            x={circle_text_x - (textWidth / desiredBalloon) / 2  }
                            y={circle_text_y -10 }
                            text={keyplace}
                            stroke="blue"
                            fill="#000"
                            fontFamily="Calibri"
                            fontSize={12 / desiredBalloon}
                            background="black"
                            strokeWidth={1}
                            draggable={false}
                            align="center"
                            verticalAlign="middle"
                        />
                        </Group>
                    {isSelected && <Transformer rotateEnabled={false} borderDash={[3,3] } borderStroke={"blue"} padding={2} resizeEnabled={false } ref={transformRef} />}
                </>
            )}

            {/* onMouseDown={onSelect} */}
            {(!shapeProps.isballooned /*|| shapeProps.selected */ ) && (    
                <>
                    <Rect
                        rotation={rotation }
                        fill="transparent"
                        stroke="red"
                        onMouseDown={onSelect}
                        ref={shapeRef}
                        {...shapeProps}
                        strokeWidth={1}
                        dash={[3, 3]}
                        draggable={false }
                        onMouseEnter={onMouseEnter}
                        onMouseLeave={onMouseLeave}
                       
                    />
                    
                </>
             )}  
        </React.Fragment>
    );
};

export default Annotation;
