import { useEffect, useState } from "react";

export type Color = {R:number, G:number, B:number, isTransparent:boolean};
function Palette(props: {changeState: (c:Color)=>void}){
    const [color, changeColor] = useState<Color>({R:0,G:0,B:0,isTransparent:false});
    useEffect(()=>props.changeState(color), [color]);
    return <div className="palette">
        <input type="range" name="R" value={color.R} onChange={e=>
            changeColor({R:parseInt(e.target.value), G:color.G, B:color.B, isTransparent: false})} />
        <input type="range" name="G" value={color.R} onChange={e=>
            changeColor({R:color.R, G:parseInt(e.target.value), B:color.B, isTransparent: false})} />
        <input type="range" name="B" value={color.R} onChange={e=>
            changeColor({R:color.R, G:color.G, B:parseInt(e.target.value), isTransparent: false})} />
    </div>
}

export default Palette;
