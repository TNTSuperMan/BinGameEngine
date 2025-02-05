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
    const [size, setSize] = useState(0);
    const [zoom, changeZoom] = useState(5);
    if(index == -1){
        return <></>
    }else{
        return <>
            <EditorMenu index={index} changeZoom={changeZoom}/>
            <div className="editor">
                <Graphic graph={data[index]?.data??[]} size={zoom} onclick={(x,y)=>{
                    const dispatcher = (sx:number, sy:number) => color.isTransparent ?
                        dispatch(editPixel([index, x+sx, y+sy, TRANSPARENT])) :
                        dispatch(editPixel([index, x+sx, y+sy, (color.R << 4) | (color.G << 2) | (color.B << 0)]))
                    dispatcher(0,0)
                    for(let i = 0;i < size;i++){
                        for(let ex = 0;ex < size+1;ex++)
                            for(let ey = 0;ey < size+1;ey++)
                                if(ex+x < (data[index]?.data[0]?.length??0) && ey+y < (data[index]?.data.length))
                                dispatcher(ex, ey);
                    }
                }} />
            </div>
            <Palette changeState={changeColor} changeSize={setSize}/>
        </>
    }
}

export default Editor;
