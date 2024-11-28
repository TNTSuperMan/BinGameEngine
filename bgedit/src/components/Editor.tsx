import { useDispatch, useSelector } from "react-redux";
import "./Editor.scss";
import { edit, Graphic, State } from "../store";

function Editor({index}: {index:number}){
    const data = useSelector<{data:State}, Graphic[]>(e=>e.data.data);
    const dispatch = useDispatch();
    if(index == -1){
        return <></>
    }else{
        const isNextLine = (e:number) => (0b11000000 & e) >> 6 == 0b10;
        const lineat = data[index].data.findIndex(isNextLine);
        const width = lineat == -1 ? data[index].data.length : lineat;
        const height= data[index].data.filter(isNextLine).length + 1;
        console.log("redraw!")
        return <>
            <div className="editorMenu">
                <span>{data[index]?.name}</span>
                <input type="number" value={width} onChange={e=>{
                    if(Number.isNaN(parseInt(e.target.value))) return;
                    const after:number[] = [];
                    const widthDiff = parseInt(e.target.value) - width;
                    console.log(widthDiff);
                    data[index].data.forEach((e,i)=>{
                        if(isNextLine(e) || i == data[index].data.length-1){
                            if(widthDiff > 0){
                                for(let i = 0;i < widthDiff;i++)
                                    after.push(0b10000000);
                            }else{
                                for(let i = 0;i < (0-widthDiff);i++)
                                    after.pop();
                            }
                        }
                        after.push(e);
                    })
                    console.log(after)
                    dispatch(edit([index, after]));
                }}/>
                <span className="x">x</span>
                <input type="number" value={height} onChange={e=>{
                    if(Number.isNaN(parseInt(e.target.value))) return;
                    const addition:number[] = [];
                    for(let i = 0;i < parseInt(e.target.value);i++)
                        addition.push(0b10000000);
                    dispatch(edit([index, [...data[index].data, ...addition]]));
                }} />
            </div>
            <div className="editor">
            </div>
        </>
    }
}

export default Editor;
