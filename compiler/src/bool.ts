import { Expr } from "./native.ts";

export const boolify = (a:Expr):Expr => a+"/ 0 greater\n";
export const not = (a:Expr):Expr => a+"/ 0 equal\n";
export const or = (a:Expr, b:Expr):Expr => boolify(a)+boolify(b)+"/ add 0 greater\n"
export const and = (a:Expr, b:Expr):Expr => not(a+b+"/ nand\n")