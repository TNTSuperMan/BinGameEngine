import { Expr, num } from "./native";

export const vars: [string, number][] = [];
export const optVars: string[] = [];
export type Pointer = [Expr, Expr];

export const echoVars = () =>
    vars.forEach(e=>console.log(`[Var]${e[0]}: ${e[1].toString(16)}`))

let i:Variable = 0xa000;
export type Variable = number;
export const defvar = (description?: string):Variable => {
    if(i >= 0xf000){
        throw new RangeError("Too many vars")
    }else{
        if(description) vars.push([description, i]);
        optVars.push((new Error).stack ?? "unknown");
        return i++;
    }
}

export const toptr = (a:Variable):Pointer=>{
    const h:string = a.toString(16)
    const up = ((h[h.length-4]??"")+(h[h.length-3]??""))
    const down = ((h[h.length-2]??"")+(h[h.length-1]??""))
    return [`/ ${up}\n`, `/ ${down}\n`]
}
export const vrP = (a:Pointer):Expr=>a.join("")+"/ load\n";
export const vr =(a:Variable):Expr=>vrP(toptr(a));

export const setP = (a:Pointer,b:Expr):Expr=>b+a.join("")+"/ store\n";
export const set=(a:Variable,b:Expr):Expr=>setP(toptr(a), b);
