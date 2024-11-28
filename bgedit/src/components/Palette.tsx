import { useEffect, useState } from "react";
import "./Palette.scss";

export type Color = {R:number, G:number, B:number, isTransparent:boolean};
function Palette(props: {changeState: (c:Color)=>void}){
    const [color, changeColor] = useState<Color>({R:0,G:0,B:0,isTransparent:false});
    useEffect(()=>props.changeState(color), [color]);
    return <div className="palette">
        <div className="prev" style={{background:color.isTransparent ?
            "" : `rgb(${color.R*85},${color.G*85},${color.B*85})`}}/>
        R:<input type="range" name="R" value={color.R} min={0} max={3} onChange={e=>
            changeColor({R:parseInt(e.target.value), G:color.G, B:color.B, isTransparent: color.isTransparent})} /><br/>
        G:<input type="range" name="G" value={color.G} min={0} max={3} onChange={e=>
            changeColor({R:color.R, G:parseInt(e.target.value), B:color.B, isTransparent: color.isTransparent})} /><br/>
        B:<input type="range" name="B" value={color.B} min={0} max={3} onChange={e=>
            changeColor({R:color.R, G:color.G, B:parseInt(e.target.value), isTransparent: color.isTransparent})} /><br/>
        <input type="checkbox" name="transparent" checked={color.isTransparent} onChange={e=>
            changeColor({R:color.R, G:color.G, B:color.B, isTransparent: e.target.checked})} />
    </div>
}

export default Palette;
