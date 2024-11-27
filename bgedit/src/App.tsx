import { useSelector } from "react-redux"
import Menu from "./components/FileMenu"
import GraphSelect from "./components/GraphSelect"
import { Graphic, State } from "./store"

function App() {
  const data = useSelector<{data:State}, Graphic[]>(e=>e.data.data)
  return (
    <>
      <div className="menu"><Menu/></div>
      
      <GraphSelect data={data} />
    </>
  )
}

export default App
