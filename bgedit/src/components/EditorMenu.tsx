import "./EditorMenu.scss";
import { useDispatch, useSelector } from "react-redux";
import { edit, Graphic, State } from "../store";
import { TRANSPARENT } from "./_editorutil";

function EditorMenu({index}: {index:number}){
    const dispatch = useDispatch();
    const data = useSelector<{data:State}, Graphic[]>(e=>e.data.data);
    const width = data[index].data[0]?.length ?? 0;
    const height= data[index].data.length;

    return <div className="editorMenu">
        <span>{data[index]?.name}</span>
        <input type="number" value={width} onChange={e=>{
            if(Number.isNaN(parseInt(e.target.value))) return;
            const after:number[][] = [...data[index].data];
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
                const addition:number[] = [];
                for(let i = 0;i < heightDiff;i++)
                    addition.push(TRANSPARENT);
                dispatch(edit([index, [...data[index].data, ...addition]]));
            }else{
                const after:number[][] = [...data[index].data];
                for(let i = 0;i < -heightDiff;i++)
                    after.pop();
                dispatch(edit([index, after]));
            }
        }} />
    </div>
}

export default EditorMenu;
