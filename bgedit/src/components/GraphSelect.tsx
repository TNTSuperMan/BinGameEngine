import "./GraphSelect.scss"
import Graphic from "./Graphic";

function GraphSelect(props: {data:{name:string, data:number[]}[]}){
    return <div className="graphselect">
        {props.data.map(e=>
            <div className="graphitem">
                <span>{e.name}</span>
                <Graphic graph={e.data} size={1}/>
        </div>)}
    </div>
}

export default GraphSelect;
