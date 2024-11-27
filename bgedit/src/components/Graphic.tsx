import "./Graphic.scss";

function Pixel(props: {x:number, y:number, d:number, size:number, i:number}){
    return <div className="pixel" key={props.i} style={{
        left:props.x * props.size,
        top: props.y * props.size,
        width: props.size,
        height:props.size,
        background: (props.d & 0b11000000) >> 6 == 0b01 ? "" :
        `rgb(${((props.d&0b110000)>>4)*85},${((props.d&0b1100)>>2)*85},${(props.d&0b11)*85})`
    }}/>
}

function Graphic(props: {graph:number[], size:number}){
    const pixels:JSX.Element[] = [];
    let x = 0,y = 0,i = 0;
    props.graph.forEach(d=>{
        if((d & 0b11000000) >> 6 == 0b10){
            x = 0;
            y++;
        }
        pixels.push(Pixel({x,y,d,size:props.size,i}));
        x++;
        i++;
    })
    return <div className="graph">{pixels}</div>;
}

export default Graphic;
