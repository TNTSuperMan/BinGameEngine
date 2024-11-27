import { init } from "../store";
import { useRef } from "react";
import "./FileMenu.scss";
import { useDispatch } from "react-redux";

type MenuProps = {
    text: string,
    func: ()=>void
}
function MenuBtn(props: MenuProps){
    return (
        <button className="menu" onClick={props.func}>
            {props.text}
        </button>);
}

function Menu(){
    const dispatch = useDispatch();
    const downloader = useRef<HTMLAnchorElement|null>(null);
    const uploader = useRef<HTMLInputElement|null>(null);
    return (<>
        <MenuBtn text="新規" func={()=>dispatch(init(1))}/>
        <MenuBtn text="開く" func={()=>uploader.current?.click()}/>
        <MenuBtn text="保存" func={()=>0}/>
        <MenuBtn text="エクスポート" func={()=>0}/>
        <a ref={downloader} style={{display:"none"}}/>
        <input ref={uploader} type="file" style={{display:"none"}} />
    </>)
}

export default Menu;
