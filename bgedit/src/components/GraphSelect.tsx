import Graphic from "./Graphic";

function GraphSelect(props: {name:string, data:number[]}){
    return <div className="graphselect">
        <span>{props.name}</span>
        <Graphic graph={props.data} size={1}/>
    </div>
}

export default GraphSelect;
