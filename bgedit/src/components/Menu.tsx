import "./Menu.scss";
import FileMenu, { ExportData } from "./FileMenu";
import { useSelector } from "react-redux";
import { State } from "../store";

function Menu(){
    const store = useSelector<{data:State},State>(e=>e.data);
    const explen = ExportData(store).length;
    return <div className="menu">
        <FileMenu/>
        <span className="lendata">長さ：{explen}b, 残り：{0x1000 - explen}b</span>
    </div>
}

export default Menu;
