import "./FileMenu.scss";
import { useDispatch } from "react-redux";
import { useRef } from "react";
import { init, store, open } from "../store";

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
    function download(data:string, mime:string, link:string){
        if(downloader.current){
            const url = URL.createObjectURL(
                new Blob([data], {type: mime}));
            downloader.current.href = url;
            downloader.current.download = link;
            downloader.current.click();
            setTimeout(()=>URL.revokeObjectURL(url), 5000);
        }
    }
    return (<>
        <MenuBtn text="新規" func={()=>dispatch(init(1))}/>
        <MenuBtn text="開く" func={()=>uploader.current?.click()}/>
        <MenuBtn text="保存" func={()=>download(
            JSON.stringify(store.getState().data),
            "application/json", "graphics.json")}/>
        <MenuBtn text="エクスポート" func={()=>download(
            store.getState().data.data.map(e=>
                e.data.map(e=>
                    e.map(e=>String.fromCharCode(e)).join(""))
                        .join(String.fromCharCode(0b10000000)))
                        .join(String.fromCharCode(0b11000000)),
            "application/octet-stream", "graphics.bin"
        )}/>
        <a ref={downloader} style={{display:"none"}} target="_blank"/>
        <input ref={uploader} type="file" style={{display:"none"}}
            onChange={e=>e.target.files?.item(0)?.text().then(e=>dispatch(open(e)))} />
    </>)
}

export default Menu;
