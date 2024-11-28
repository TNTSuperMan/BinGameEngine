import { useState } from "react"
import Menu from "./components/Menu"
import GraphSelect from "./components/GraphSelect"
import Editor from "./components/Editor"

function App() {
  const [activeID, setid] = useState(-1);
  return (
    <>
      <Menu/>
      
      <GraphSelect setid={setid} />
      <Editor index={activeID}/>
    </>
  )
}

export default App
