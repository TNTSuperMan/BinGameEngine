import { Expr, nand } from "native";

export const bit = {
    not: (a:Expr):Expr => nand(a, a),
    nor:  (a:Expr, b:Expr):Expr => bit.and(bit.not(a), bit.not(b)),
    nand,
    xnor: (a:Expr, b:Expr):Expr => bit.or(bit.and(a,b), bit.and(bit.not(a),bit.not(b))),
    or:   (a:Expr, b:Expr):Expr => bit.not(bit.nor(a,b)),
    and:  (a:Expr, b:Expr):Expr => bit.not(nand(a,b)),
    xor:  (a:Expr, b:Expr):Expr => bit.not(bit.xnor(a,b))
}
