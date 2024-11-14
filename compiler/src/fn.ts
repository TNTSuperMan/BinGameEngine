import { defvar } from "./var";
let fndefines = "";
export type Call = string;
export const defn = <T extends string[]>(name:string, fn:(...vars:number[])=>void):(...args:T)=>Call => {
    let vars:number[] = [];
    for(let i = 0;i < fn.length;i++) vars.push(defvar());
    const realname = ":" +name + crypto.randomUUID();
    fndefines += realname + "\n";
    fndefines += fn(...vars) + "\n";return (...args)=>args.join("") + `\n/ ${realname} call\n`
}
