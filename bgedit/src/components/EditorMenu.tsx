import "./EditorMenu.scss";
import { useDispatch, useSelector } from "react-redux";
import { edit, Graphic, State } from "../store";
import { TRANSPARENT } from "./_editorutil";
import { useEffect, useState } from "react";

function EditorMenu(props: {index:number, changeZoom: (v:number)=>void}){
    const dispatch = useDispatch();
    const index = props.index;
    const data = useSelector<{data:State}, Graphic[]>(e=>e.data.data);
    const width = data[props.index].data[0]?.length ?? 0;
    const height= data[props.index].data.length;

    const [zoom, changezoom] = useState(5);
    useEffect(()=>props.changeZoom(zoom), [zoom, props]);

    return <div className="editorMenu">
        <span>{data[index]?.name}</span>
        <div className="size">
            <input type="number" value={width} onChange={e=>{
                if(Number.isNaN(parseInt(e.target.value))) return;
                const after:number[][] = [...data[index].data.map(t=>[...t])];
                const widthDiff = parseInt(e.target.value) - width;
                if(widthDiff > 0){
                    for(let i = 0;i < widthDiff;i++)
                        for(let j = 0;j < after.length;j++)
                            after[j].push(TRANSPARENT);
                }else{
                    for(let i = 0;i < -widthDiff;i++)
                        for(let j = 0;j < after.length;j++)
                            after[j].pop();
                }
                dispatch(edit([index, after]));
            }}/>
            <span className="x">x</span>
            <input type="number" value={height} onChange={e=>{
                if(Number.isNaN(parseInt(e.target.value))) return;
                const heightDiff = parseInt(e.target.value) - height;
                if(height > 0){
                    const addition:number[][] = [];
                    for(let i = 0;i < heightDiff;i++){
                        addition.push([]);
                        for(let j = 0;j < width;j++) addition[addition.length-1].push(TRANSPARENT);
                    }
                    dispatch(edit([index, [...data[index].data, ...addition]]));
                }else{
                    const after:number[][] = [...data[index].data.map(t=>[...t])];
                    for(let i = 0;i < -heightDiff;i++)
                        after.pop();
                    dispatch(edit([index, after]));
                }
            }}/>
        </div>

        zoom:<input type="number" max={100} min={1} name="zoom" value={zoom} onChange={e=>changezoom(parseInt(e.target.value))} />
    </div>
}

export default EditorMenu;
