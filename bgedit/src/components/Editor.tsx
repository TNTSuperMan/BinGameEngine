import "./Editor.scss";
import { useDispatch, useSelector } from "react-redux";
import { useState } from "react";
import { editPixel, Graphic as GraphData, State } from "../store";
import { TRANSPARENT } from "./_editorutil";
import Palette, { Color } from "./Palette";
import EditorMenu from "./EditorMenu";
import Graphic from "./Graphic";

function Editor({index}: {index:number}){
    const data = useSelector<{data:State}, GraphData[]>(e=>e.data.data);
    const dispatch = useDispatch();
    const [color,changeColor] = useState<Color>({R:0,G:0,B:0,isTransparent:false});
    const [zoom, changeZoom] = useState(5);
    if(index == -1){
        return <></>
    }else{
        return <>
            <EditorMenu index={index} changeZoom={changeZoom}/>
            <div className="editor">
                <Graphic graph={data[index]?.data??[]} size={zoom} onclick={(x,y)=>{
                    if(color.isTransparent){
                        dispatch(editPixel([index, x, y, TRANSPARENT]));
                    }else{
                        dispatch(editPixel([index, x, y, (color.R << 4) | (color.G << 2) | (color.B << 0)]));
                    }
                }} />
            </div>
            <Palette changeState={changeColor}/>
        </>
    }
}

export default Editor;
