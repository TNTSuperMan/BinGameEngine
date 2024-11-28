import { useDispatch, useSelector } from "react-redux";
import "./Editor.scss";
import { edit, Graphic as GraphData, State } from "../store";
import Graphic from "./Graphic";
import { useState } from "react";

const TRANSPARENT = 0b01000000;
const NEXTLINE = 0b10000000;

const isNextLine = (e:number) => (0b10000000 & e) == NEXTLINE;

function EditorMenu({index}: {index:number}){
    const dispatch = useDispatch();
    const data = useSelector<{data:State}, GraphData[]>(e=>e.data.data);
    const lineat = data[index].data.findIndex(isNextLine);
    const width = lineat == -1 ? data[index].data.length : lineat;
    const height= data[index].data.filter(isNextLine).length + 1;

    return <div className="editorMenu">
        <span>{data[index]?.name}</span>
        <input type="number" value={width} onChange={e=>{
            if(Number.isNaN(parseInt(e.target.value))) return;
            const after:number[] = [];
            const widthDiff = parseInt(e.target.value) - width;
            data[index].data.forEach((e,i)=>{
                if(isNextLine(e) || i == data[index].data.length-1){
                    if(widthDiff > 0){
                        for(let i = 0;i < widthDiff;i++)
                            after.push(TRANSPARENT);
                    }else{
                        for(let i = 0;i < (0-widthDiff);i++)
                            after.pop();
                    }
                }
                after.push(e);
            })
            dispatch(edit([index, after]));
        }}/>
        <span className="x">x</span>
        <input type="number" value={height} onChange={e=>{
            if(Number.isNaN(parseInt(e.target.value))) return;
            const addition:number[] = [];
            for(let i = 0;i < parseInt(e.target.value)+1;i++)
                addition.push(i == 0 ? NEXTLINE : TRANSPARENT);
            dispatch(edit([index, [...data[index].data, ...addition]]));
        }} />
    </div>
}

function Editor({index}: {index:number}){
    const data = useSelector<{data:State}, GraphData[]>(e=>e.data.data);
    const [color,changeColor] = useState({R:0,G:0,B:0,isTransparent:false});
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
        </>
    }
}

export default Editor;
