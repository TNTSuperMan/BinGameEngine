import { defvar, toptr, Variable } from "./var";
import { Expr } from "./native";
import { Exprs } from "./control"
export const tags: Set<string> = new Set;
export let  fndefines = "";
export let bindefines = "";
export let resdefines = "";

export const preloadFn = <T extends string[]>(name: string): (...args: T) => Expr => 
    (...args) => args.join("") + `/ :fn_${name} call\n`;

export const preloadResource = (type: "binary" | "res" | "exprres", name: string): Expr =>
    `/ :${type}_${name}\n`;

export const defn = <T extends string[]>(name:string, fn:(...vars:number[])=>Exprs):(...args:T)=>Expr => {
    let vars:Variable[] = [];
    for(let i = 0;i < fn.length;i++) vars.push(defvar());
    const realname = ":fn_" +name;
    if(tags.has(realname)) throw new ReferenceError("Already defined function: "+name);

    fndefines += realname + "\n";
    fndefines += vars.reverse().map(e=>toptr(e).join("")+"/ store\n").join("")
    fndefines += fn(...vars.reverse()).join("");
    return (...args)=>args.join("") + `/ ${realname} call\n`
}

export const loadbinary = (name: string, path: string):Expr => {
    const realname = ":binary_" + name;
    if(tags.has(realname)) throw new ReferenceError("Already defined binary: "+name);
    bindefines += realname;
    bindefines += `
inject ${path}\n`
    return "/ "+realname+"\n"
}

export const defres = (name: string, resource: string):Expr => {
    const realname = ":res_" + name;
    if(tags.has(realname)) throw new ReferenceError("Already defined resource: "+name);
    resdefines += realname + "\n";
    resdefines += "inject_fromB64 " + btoa(resource) + "\n";
    return "/ " + realname + "\n";
}

export const defExprRes = (name: string, res: Expr):Expr => {
    const realname = ":exprres_" + name;
    if(tags.has(realname)) throw new ReferenceError("Already defined exprres: "+name);
    resdefines += realname + "\n";
    resdefines += res;
    return "/ " + realname + "\n";
}
