import Menu from "./components/FileMenu"
import GraphSelect from "./components/GraphSelect"
import { store } from "./store"

function App() {
  return (
    <>
    <div className="menu"><Menu/></div>
      
      {store.getState().data.data.map(e=>GraphSelect(e))}
    </>
  )
}

export default App
