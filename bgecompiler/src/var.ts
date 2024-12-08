import { Expr, num } from "./native.ts";

export const vars: [string, number][] = [];
export type Pointer = [Expr, Expr];

export const varaddr = (a:Variable):Pointer=>{
    const h:string = a.toString(16)
    const up = ((h[h.length-4]??"")+(h[h.length-3]??""))
    const down = ((h[h.length-2]??"")+(h[h.length-1]??""))
    return [`/ ${up}\n`, `/ ${down}\n`]
}
export const vr =(a:Variable)=>varaddr(a).join("")+"/ load\n";
let i:Variable = 0xa000;

export type Variable = number;
export const defvar = (description?: string):Variable => {
    if(i >= 0xffff){
        throw new RangeError("Too many vars")
    }else{
        if(description) vars.push([description, i]);
        return i++;
    }
}

export const echoVars = () =>
    vars.forEach(e=>console.log(`[Var]${e[0]}: ${e[1].toString(16)}`))
