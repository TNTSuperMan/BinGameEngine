import "./Menu.scss";
import FileMenu, { ExportData } from "./FileMenu";

function Menu(){
    const explen = ExportData().length;
    return <div className="menu">
        <FileMenu/>
        <span className="lendata">長さ：{explen}b, 残り：{0x1000 - explen}b</span>
    </div>
}

export default Menu;
