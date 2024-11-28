import "./EditorMenu.scss";
import { useDispatch, useSelector } from "react-redux";
import { isNextLine, NEXTLINE, TRANSPARENT } from "./_editorutil";
import { edit, Graphic, State } from "../store";

function EditorMenu({index}: {index:number}){
    const dispatch = useDispatch();
    const data = useSelector<{data:State}, Graphic[]>(e=>e.data.data);
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

export default EditorMenu;
