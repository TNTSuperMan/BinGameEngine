import { Expr } from "./native.ts";

export const varaddr = (a:Variable):Expr=>{
    let h:string = a.toString(16)
    let t = "/ ";
    let up = ((h[h.length-4]??"")+(h[h.length-3]??""))
    t += up ? up : "0"
    t += " "
    let down = ((h[h.length-2]??"")+(h[h.length-1]??""))
    t += down ? down : "0"
    return t + "\n";
}
export const vr =(a:Variable)=>varaddr(a)+"/ load\n";
let i:Variable = 0xa000;

export type Variable = number;
export const defvar = ():Variable => {
    if(i >= 0xffff){
        throw new RangeError("Too many vars")
    }else{
        return i++;
    }
}