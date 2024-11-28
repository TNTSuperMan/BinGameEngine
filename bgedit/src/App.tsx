import { useSelector } from "react-redux"
import Menu from "./components/Menu"
import GraphSelect from "./components/GraphSelect"
import { Graphic, State } from "./store"
import Editor from "./components/Editor"
import { useState } from "react"

function App() {
  const data = useSelector<{data:State}, Graphic[]>(e=>e.data.data);
  const [activeID, setid] = useState(-1);
  return (
    <>
      <Menu/>
      
      <GraphSelect data={data} setid={setid} />
      <Editor index={activeID}/>
    </>
  )
}

export default App
