import "./Menu.scss";
import FileMenu, { ExportData } from "./FileMenu";

function Menu(){
    const explen = ExportData().length;
    return <div className="menu">
        <FileMenu/>
        長さ：{explen}, 残り：{0x1000 - explen}
    </div>
}

export default Menu;
