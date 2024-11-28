import { useDispatch, useSelector } from "react-redux";
import "./GraphSelect.scss"
import Graphic from "./Graphic";
import { add, Graphic as GraphData, remove, rename, State } from "../store";

function GraphSelect(props: {setid: (id:number)=>void}){
  const data = useSelector<{data:State}, GraphData[]>(e=>e.data.data);
  const dispatch = useDispatch();
    return <div className="graphselect">
        {data.map((e,i)=>
            <div className="graphitem" key={i} onClick={()=>props.setid(i)}>
                {i}:<input type="text" name="name" value={e.name}
                    onChange={e=>dispatch(rename([i, e.target.value]))} />
                <button onClick={()=>dispatch(remove(i))}>X</button>
                <Graphic graph={e.data} size={2} onclick={()=>{}}/>
        </div>)}
        <button onClick={()=>dispatch(add(0))}>+</button>
    </div>
}

export default GraphSelect;
