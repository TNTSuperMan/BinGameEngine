import { useSelector } from "react-redux";
import "./Editor.scss";
import { Graphic as GraphData, State } from "../store";
import Graphic from "./Graphic";
import { useState } from "react";
import EditorMenu from "./EditorMenu";
import { isNextLine, TRANSPARENT } from "./_editorutil";
import Palette, { Color } from "./Palette";

function Editor({index}: {index:number}){
    const data = useSelector<{data:State}, GraphData[]>(e=>e.data.data);
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
                        data[index].data[i] = TRANSPARENT;
                    }else{
                        data[index].data[i] = (color.R << 4) | (color.G << 2) | (color.B << 0);
                    }
                }} />
            </div>
            <Palette changeState={changeColor}/>
        </>
    }
}

export default Editor;
