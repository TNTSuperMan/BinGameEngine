import { defn } from "./fn";
import { Variable } from "./var";

export type Expr = string;

//Number to Hex
const n2h = (e:number) => "/ " + e.toString(16) + "\n";

export const num=(a:number)=>n2h(a);

export const pop=()=>"/ pop\n";
export const cls=()=>"/ cls\n";

export const add=    (a:Expr,b:Expr):Expr=>a+b+"/ add\n";
export const sub=    (a:Expr,b:Expr):Expr=>a+b+"/ sub\n";
export const mul=    (a:Expr,b:Expr):Expr=>a+b+"/ mul\n";
export const div=    (a:Expr,b:Expr):Expr=>a+b+"/ div\n";
export const rem=    (a:Expr,b:Expr):Expr=>a+b+"/ rem\n";
export const nand=   (a:Expr,b:Expr):Expr=>a+b+"/ nand\n";
export const equal=  (a:Expr,b:Expr):Expr=>a+b+"/ equal\n";
export const greater=(a:Expr,b:Expr):Expr=>a+b+"/ greater\n";
export const set=(a:Variable,b:Expr):Expr=>n2h(a)+b+"store\n";