import "./GraphSelect.scss"
import Graphic from "./Graphic";

function GraphSelect(props: {data:{name:string, data:number[]}[], setid: (id:number)=>void}){
    return <div className="graphselect">
        {props.data.map((e,i)=>
            <div className="graphitem" key={i} onClick={()=>props.setid(i)}>
                <span>{e.name}</span>
                <Graphic graph={e.data} size={1} onclick={()=>{}}/>
        </div>)}
    </div>
}

export default GraphSelect;
