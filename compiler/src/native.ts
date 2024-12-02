import { varaddr, Variable } from "./var.ts";

export type Expr = string;

export const num=(a:number)=> "/ " + a.toString(16) + "\n";

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
export const set=(a:Variable,b:Expr):Expr=>b+varaddr(a)+"/ store\n";
export const ret=(...a:Expr[]):Expr=>a.join("\n")+"/ ret\n";

export const rect=(x:Expr,y:Expr,w:Expr,h:Expr,c:Expr):Expr=>x+y+w+h+c+"/ rect\n";
export const redraw=():Expr=>"/ redraw\n";
