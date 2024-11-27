function Pixel(props: {x:number, y:number, d:number, size:number}){
    if((props.d & 0b11000000) >> 6 == 0b01){
        return <></>
    }else{
        return <div className="pixel" style={{
            left:props.x * props.size,
            top: props.y * props.size,
            width: props.size,
            height:props.size
        }}/>
    }
}

function Graphic(props: {graph:number[], size:number}){
    const pixels:JSX.Element[] = [];
    let x = 0,y = 0;
    props.graph.forEach(d=>{
        pixels.push(Pixel({x,y,d,size:props.size}));
        x++;
        if((d & 0b110000) >> 6 == 0b10){
            x = 0;
            y++;
        }
    })
    return <div className="graph">{pixels}</div>;
}

export default Graphic;
