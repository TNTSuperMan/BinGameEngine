import { Expr } from "./native";

export const boolify = (a:Expr):Expr => a+"/ 2 rem\n";
export const not = (a:Expr):Expr => a+"/ 0 nand 2 rem\n";
export const or = (a:Expr, b:Expr):Expr => boolify(a)+boolify(b)+"/ add 1 greater\n"
export const and = (a:Expr, b:Expr):Expr => not(a+b+"/ nand\n")