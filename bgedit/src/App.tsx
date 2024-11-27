import { useSelector } from "react-redux"
import Menu from "./components/FileMenu"
import GraphSelect from "./components/GraphSelect"
import { Graphic, State } from "./store"

function App() {
  const data = useSelector<{data:State}, Graphic[]>(e=>e.data.data)
  return (
    <>
      <div className="menu"><Menu/></div>
      
      {data.map((e,i)=><GraphSelect key={i} data={e.data} name={e.name}/>)}
    </>
  )
}

export default App
