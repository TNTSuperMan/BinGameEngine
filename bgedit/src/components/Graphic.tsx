import "./Graphic.scss";
type ClickEvent = (x: number, y: number) => void;
function Pixel(props: {x:number, y:number, d:number, size:number, onclick:ClickEvent}){
    return <div className="pixel"
            onClick={()=>props.onclick(props.x,props.y)}
            style={{
                left:props.x * props.size,
                top: props.y * props.size,
                width: props.size,
                height:props.size,
                background: (props.d & 0b11000000) >> 6 == 0b01 ? "" :
                `rgb(${((props.d&0b110000)>>4)*85},${((props.d&0b1100)>>2)*85},${(props.d&0b11)*85})`
    }}/>
}

function Graphic({graph, size, onclick}: {graph:number[][], size:number, onclick: ClickEvent}){
    const pixels:JSX.Element[] = [];
    let i = 0;
    for(let y = 0;y < graph.length;y++)
        for(let x = 0;x < graph[y].length;x++)
            pixels.push(<Pixel x={x} y={y} size={size} d={graph[y][x]} key={i++} onclick={onclick}/>);
    return <div className="graph">{pixels}</div>;
}

export default Graphic;
