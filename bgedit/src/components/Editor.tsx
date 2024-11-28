import "./Editor.scss";
import { useDispatch, useSelector } from "react-redux";
import { useState } from "react";
import { editPixel, Graphic as GraphData, State } from "../store";
import { isNextLine, TRANSPARENT } from "./_editorutil";
import Palette, { Color } from "./Palette";
import EditorMenu from "./EditorMenu";
import Graphic from "./Graphic";

function Editor({index}: {index:number}){
    const data = useSelector<{data:State}, GraphData[]>(e=>e.data.data);
    const dispatch = useDispatch();
    const [color,changeColor] = useState<Color>({R:0,G:0,B:0,isTransparent:false});
    if(index == -1){
        return <></>
    }else{
        return <>
            <EditorMenu index={index}/>
            <div className="editor">
                <Graphic graph={data[index].data} size={5} onclick={(x,y)=>{
                    let i = 0;
                    for(let j = 0;j < y;j++)
                        while(isNextLine(data[index].data[i++]));
                    i += x;
                    if(color.isTransparent){
                        dispatch(editPixel([index, i, TRANSPARENT]));
                    }else{
                        dispatch(editPixel([index, i, (color.R << 4) | (color.G << 2) | (color.B << 0)]));
                    }
                }} />
            </div>
            <Palette changeState={changeColor}/>
        </>
    }
}

export default Editor;
