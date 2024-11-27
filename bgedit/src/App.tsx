import { useSelector } from "react-redux"
import Menu from "./components/Menu"
import GraphSelect from "./components/GraphSelect"
import { Graphic, State } from "./store"

function App() {
  const data = useSelector<{data:State}, Graphic[]>(e=>e.data.data)
  return (
    <>
      <Menu/>
      
      <GraphSelect data={data} />
    </>
  )
}

export default App
