import { defvar, toptr, Variable } from "./var";
import { Expr } from "./native";
import { Exprs } from "./control"
import genid from "./genid";
export let  fndefines = "";
export let bindefines = "";
export let resdefines = "";

export const defn = <T extends string[]>(name:string, fn:(...vars:number[])=>Exprs):(...args:T)=>Expr => {
    let vars:Variable[] = [];
    for(let i = 0;i < fn.length;i++) vars.push(defvar());
    const realname = ":fn_" +name + genid();

    fndefines += realname + "\n";
    fndefines += vars.reverse().map(e=>toptr(e).join("")+"/ store\n").join("")
    fndefines += fn(...vars.reverse()).join("");
    return (...args)=>args.join("") + `/ ${realname} call\n`
}

export const loadbinary = (name: string, path: string):Expr => {
    const realname = ":binary_" + name + genid();
    bindefines += realname;
    bindefines += `
inject ${path}\n`
    return "/ "+realname+"\n"
}

export const defres = (name: string, resource: string):Expr => {
    const realname = ":" + name + genid();
    resdefines += realname + "\n";
    resdefines += "inject_fromB64 " + btoa(resource) + "\n";
    return "/ " + realname + "\n";
}

export const defExprRes = (name: string, res: Expr):Expr => {
    const realname = ":" + name + genid();
    resdefines += realname + "\n";
    resdefines += res;
    return "/ " + realname + "\n";
}
