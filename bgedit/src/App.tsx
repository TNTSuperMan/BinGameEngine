import Menu from "./components/FileMenu"
import Graphic from "./components/Graphic"

function App() {
  return (
    <>
      <Menu/>
      <Graphic graph={[0b110000,0b1100,0b11,0b01000000,0b111111,128,
                       0b110011,0b1111,0b111100]} size={1} />
    </>
  )
}

export default App
