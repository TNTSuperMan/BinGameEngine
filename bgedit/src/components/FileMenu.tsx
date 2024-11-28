import "./FileMenu.scss";
import { useDispatch } from "react-redux";
import { useRef } from "react";
import { init, store, open } from "../store";
import { NEXTLINE } from "./_editorutil";

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
    function download(data:string|Uint8Array, mime:string, link:string){
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
            JSON.stringify(store.getState().data.data),
            "application/json", "graphics.json")}/>
        <MenuBtn text="エクスポート" func={()=>{
            const contents:number[] = [];
            console.log(store.getState().data.data)
            store.getState().data.data.forEach(g=>{
                g.data.forEach(l=>{
                    const line = [...l];
                    while((line[line.length-1] & 0b11000000) == 0b01000000) line.pop();
                    line.forEach(num=>{
                        contents.push(num);
                    })
                    contents.push(NEXTLINE);
                })
                contents.push(0b11000000);
            })
            download(
                new Uint8Array(contents),
                "application/octet-stream", "graphics.bin"
            )
        }}/>
        <a ref={downloader} style={{display:"none"}} target="_blank"/>
        <input ref={uploader} type="file" style={{display:"none"}}
            onChange={e=>e.target.files?.item(0)?.text().then(e=>dispatch(open(e)))} />
    </>)
}

export default Menu;
