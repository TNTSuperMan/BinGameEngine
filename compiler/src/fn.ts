import { defvar, Variable } from "./var.ts";
import { Expr } from "./native.ts";
import genid from "genid.ts";
let fndefines = "";

export const defn = <T extends string[]>(name:string, fn:(...vars:number[])=>void):(...args:T)=>Expr => {
    let vars:Variable[] = [];
    for(let i = 0;i < fn.length;i++) vars.push(defvar());
    const realname = ":" +name + genid();
    fndefines += realname + "\n";
    fndefines += fn(...vars) + "\n";return (...args)=>args.join("") + `\n/ ${realname} call\n`
}
